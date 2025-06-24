using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationService.Application.Internal.CommandServices;
using NotificationService.Application.Internal.QueryServices;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Infrastructure.Persistence.EFC.Repositories;
using NotificationService.Infrastructure.Persistence.EFC.Configuration;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Shared.Interfaces.ASP.Configuration;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Application.Internal.EventServices;
using NotificationService.Infrastructure.Messaging.Kafka;
using NotificationService.Interfaces.Messaging;
using NotificationService.Interfaces.Messaging.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers( options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var developmentString = builder.Configuration.GetConnectionString("DevelopmentConnection");

// Configure Database Context and Logging Levels

builder.Services.AddDbContext<BaseDbContext, AppDbContext>(
    options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
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
        Title = "NotificationService.API",
        Version = "v1",
        Description = "Notification Service API - Gestión de notificaciones",
        TermsOfService = new Uri("https://alquila-facil.com/tos"),
        Contact = new OpenApiContact
        {
            Name = "Alquila Facil",
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

    // CORREGIR: Configuración del servidor base
    if (builder.Environment.IsDevelopment())
    {
        c.AddServer(new OpenApiServer
        {
            Url = "https://localhost:8015",
            Description = "Development Server"
        });
    }
    else
    {
        c.AddServer(new OpenApiServer
        {
            Url = "/notification",
            Description = "Production Server"
        });
    }

    // MEJORAR: Configuración de comentarios XML más robusta
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

// Notification Bounded Context Injection Configuration
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();

builder.Services.AddScoped<INotificationEventService, NotificationEventService>();
builder.Services.AddScoped<IBookingCreatedConsumer, BookingCreatedConsumer>();
builder.Services.AddHostedService<ConsumerHostedService>();

builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8015);
});


// Configuración JWT Authentication
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Service API V1");
        }
        else
        {
            c.SwaggerEndpoint("/notification/swagger/v1/swagger.json", "Notification Service API V1");
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();