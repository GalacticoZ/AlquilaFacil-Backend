using IAMService.Application.External.OutboundServices;
using IAMService.Application.Internal.CommandServices;
using IAMService.Application.Internal.OutboundServices;
using IAMService.Application.Internal.QueryServices;
using IAMService.Domain.Model.Commands;
using IAMService.Domain.Repositories;
using IAMService.Domain.Services;
using IAMService.Infrastructure.Hashing.BCrypt.Services;
using IAMService.Infrastructure.Persistence.EFC.Configuration;
using IAMService.Infrastructure.Persistence.EFC.Repositories;
using IAMService.Infrastructure.Tokens.JWT.Configuration;
using IAMService.Infrastructure.Tokens.JWT.Services;
using Shared.Interfaces.ACL.Facades.Services;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProfilesService.Interfaces.ACL;
using Shared.Infrastructure.Persistence.EFC.Configuration;
using System.Reflection;
using System.Text;
using IAMService.Infrastructure.Pipeline.Middleware.Extensions;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var developmentString = builder.Configuration.GetConnectionString("DevelopmentConnection");

// Configure Database Context and Logging Levels

builder.Services.AddDbContext<BaseDbContext, AppDbContext>(
    options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.UseMySql(developmentString, ServerVersion.AutoDetect(developmentString))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        else if (builder.Environment.IsProduction())
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        }
    });
// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IAMService.API",
        Version = "v1",
        Description = "IAM Service API - Gestión de identidad y acceso",
        TermsOfService = new Uri("https://alquila-facil.com/tos"),
        Contact = new OpenApiContact
        {
            Name = "Alquila Fácil",
            Email = "contact@alquilaf.com"
        },
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT en el formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    if (builder.Environment.IsDevelopment())
    {
        c.AddServer(new OpenApiServer
        {
            Url = "https://localhost:8011",
            Description = "Development Server"
        });
    }
    else
    {
        c.AddServer(new OpenApiServer
        {
            Url = "/iam",
            Description = "Production Server"
        });
    }

    try
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
            Console.WriteLine($"XML documentation loaded from: {xmlPath}");
        }
        else
        {
            Console.WriteLine($"XML documentation not found at: {xmlPath}");
            Console.WriteLine("Make sure GenerateDocumentationFile is set to true in your .csproj file");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading XML documentation: {ex.Message}");
    }

    // Configurar esquemas de respuesta
    c.EnableAnnotations();
    
    // AGREGAR: Configuraciones adicionales para evitar errores
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    c.DescribeAllParametersInCamelCase();
});

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<ISeedUserRoleCommandService, SeedUserRoleCommandService>();
builder.Services.AddScoped<ISeedAdminCommandService, SeedAdminCommandService>();

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IProfilesUserExternalService, ProfilesUserExternalService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

builder.Services.AddHttpClient();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8011);
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["TokenSettings:Secret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    var userRoleCommandService = services.GetRequiredService<ISeedUserRoleCommandService>();
    await userRoleCommandService.Handle(new SeedUserRolesCommand());
    
    var adminCommandService = services.GetRequiredService<ISeedAdminCommandService>();
    await adminCommandService.Handle(new SeedAdminCommand());
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger(c =>
    {
        c.RouteTemplate = "swagger/{documentName}/swagger.json";
    });
    
    app.UseSwaggerUI(c =>
    {
        // CORREGIR: Configuración más específica de la URL del endpoint
        if (app.Environment.IsDevelopment())
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "IAM Service API V1");
        }
        else
        {
            c.SwaggerEndpoint("/iam/swagger/v1/swagger.json", "IAM Service API V1");
        }
        
        c.DisplayRequestDuration();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.EnableDeepLinking();
        c.EnableValidator();
        
        // AGREGAR: Configuraciones adicionales para debugging
        c.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
        c.ConfigObject.AdditionalItems.Add("syntaxHighlight.activated", true);
    });
}

app.UseCors("AllowAllPolicy");

app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();