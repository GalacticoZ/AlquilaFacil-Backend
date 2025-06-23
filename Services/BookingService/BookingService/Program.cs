using BookingService.Application.Internal.CommandServices;
using BookingService.Application.External.OutboundServices;
using BookingService.Application.Internal.QueryServices;
using BookingService.Domain.Repositories;
using BookingService.Domain.Services;
using BookingService.Infrastructure.Persistence.EFC.Repositories;
using BookingService.Infrastructure.Persistence.EFC.Configuration;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Shared.Interfaces.ASP.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.Application.External.OutboundServices;
using Shared.Infrastructure.Persistence.EFC.Configuration;
using Shared.Interfaces.ACL.Facades;
using Shared.Interfaces.ACL.Facades.Services;
using System.Reflection;
using System.Text;
using BookingService.Domain.Messaging;
using BookingService.Infrastructure.Messaging.Kafka.BookingEventPublisher;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

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
        Title = "BookingService.API",
        Version = "v1",
        Description = "Booking Service API - Gestión de reservaciones",
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
            Url = "https://localhost:8013",
            Description = "Development Server"
        });
    }
    else
    {
        c.AddServer(new OpenApiServer
        {
            Url = "/booking",
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

//builder.Services.AddScoped<ISubscriptionInfoExternalService,SubscriptionInfoExternalService>();

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Booking Bounded Context Injection Configuration
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationCommandService, ReservationCommandService>();
builder.Services.AddScoped<IReservationQueryService, ReservationQueryService>();

builder.Services.AddScoped<IReservationLocalExternalService, ReservationLocalExternalService>();
builder.Services.AddScoped<ISubscriptionInfoExternalService, SubscriptionInfoExternalService>();
builder.Services.AddScoped<IUserExternalService, UserExternalService>();

builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<ILocalsContextFacade, LocalsContextFacade>();
builder.Services.AddScoped<ISubscriptionContextFacade, SubscriptionContextFacade>();

builder.Services.AddScoped<IBookingEventPublisher, KafkaBookingEventPublisher>();

builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8013); // Permite escuchar en cualquier IP en el puerto 8013
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
                Encoding.ASCII.GetBytes(builder.Configuration["JWT_SECRET"])),
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Service API V1");
        }
        else
        {
            c.SwaggerEndpoint("/booking/swagger/v1/swagger.json", "Booking Service API V1");
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