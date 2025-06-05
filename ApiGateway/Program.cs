var builder = WebApplication.CreateBuilder(args);

var reverseProxySection = builder.Configuration.GetSection("ReverseProxy");

builder.Services.AddReverseProxy().LoadFromConfig(reverseProxySection);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

var app = builder.Build();

app.MapReverseProxy();

app.Run();