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
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "BookingService.API",
                Version = "v1",
                Description = "Booking Service API",
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
            }
        );
        c.AddServer(new OpenApiServer
        {
            Url = builder.Environment.IsDevelopment() ? "/" : "/booking"
        });
    });

builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8013); // Permite escuchar en cualquier IP en el puerto 8013
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();