using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.Application.External.OutboundServices;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Shared.Interfaces.ACL.Facades;
using Shared.Interfaces.ACL.Facades.Services;
using Shared.Interfaces.ASP.Configuration;
using SubscriptionsService.Application.Internal.CommandServices;
using SubscriptionsService.Application.Internal.QueryServices;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;
using SubscriptionsService.Infrastructure.Persistence.EFC.Repositories;
using SubscriptionsService.Infrastructure.Persistence.EFC.Configuration;
using SubscriptionsService.Domain.Model.Commands;
using System.Reflection;

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
        Title = "SubscriptionsService.API",
        Version = "v1",
        Description = "Subscriptions Service API - Gestión de suscripciones y planes",
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

    // CORREGIR: Configuración del servidor base
    if (builder.Environment.IsDevelopment())
    {
        c.AddServer(new OpenApiServer
        {
            Url = "https://localhost:8016",
            Description = "Development Server"
        });
    }
    else
    {
        c.AddServer(new OpenApiServer
        {
            Url = "/subscriptions",
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


// Subscriptions Bounded Context Injection Configuration
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryServices, SubscriptionQueryService>();
builder.Services.AddScoped<ISubscriptionStatusRepository, SubscriptionStatusRepository>();
builder.Services.AddScoped<ISubscriptionStatusCommandService, SubscriptionStatusCommandService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<IUserExternalService, UserExternalService>();

builder.Services.AddScoped<ISeedSubscriptionPlanCommandService, SeedSubscriptionPlanCommandService>();

builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();


builder.Services.AddHttpClient();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8016);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    var planCommandService = services.GetRequiredService<ISeedSubscriptionPlanCommandService>();
    await planCommandService.Handle(new SeedSubscriptionPlanCommand());
    
    var subscriptionStatusCommandService = services.GetRequiredService<ISubscriptionStatusCommandService>();
    await subscriptionStatusCommandService.Handle(new SeedSubscriptionStatusCommand());
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscriptions Service API V1");
        }
        else
        {
            c.SwaggerEndpoint("/subscriptions/swagger/v1/swagger.json", "Subscriptions Service API V1");
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