using LocalsService.Application.Internal.CommandServices;
using LocalsService.Application.Internal.QueryServices;
using LocalsService.Domain.Model.Commands;
using LocalsService.Domain.Repositories;
using LocalsService.Domain.Services;
using LocalsService.Infrastructure.Persistence.EFC.Configuration;
using LocalsService.Infrastructure.Persistence.EFC.Repositories;
using LocalsService.Locals.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Shared.Application.External.OutboundServices;
using Shared.Domain.Repositories;
using Shared.Infrastructure.Persistence.EFC.Configuration;
using Shared.Infrastructure.Persistence.EFC.Repositories;
using Shared.Interfaces.ACL.Facades;
using Shared.Interfaces.ACL.Facades.Services;
using Shared.Interfaces.ASP.Configuration;

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
                Title = "LocalsService.API",
                Version = "v1",
                Description = "Locals Service API",
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
            Url = builder.Environment.IsDevelopment() ? "/" : "/locals"
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

builder.Services.AddScoped<ILocalCategoryRepository, LocalCategoryRepository>();

builder.Services.AddScoped<ILocalCategoryCommandService, LocalCategoryCommandService>();
builder.Services.AddScoped<ILocalCategoryQueryService, LocalCategoryQueryService>();

builder.Services.AddScoped<ILocalCommandService, LocalCommandService>();
builder.Services.AddScoped<ILocalQueryService, LocalQueryService>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();

builder.Services.AddScoped<ICommentCommandService, CommentCommandService>();
builder.Services.AddScoped<ICommentQueryService, CommentQueryService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();


builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

builder.Services.AddScoped<IUserExternalService, UserExternalService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

builder.Services.AddHttpClient();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8012); 
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    var localCategoryTypeCommandService = services.GetRequiredService<ILocalCategoryCommandService>();
    await localCategoryTypeCommandService.Handle(new SeedLocalCategoriesCommand());
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