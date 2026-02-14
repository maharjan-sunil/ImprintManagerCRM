using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("OcelotConfig/ocelot.global.json", optional: false, reloadOnChange: true)
    .AddOcelot("OcelotConfig");
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Ocelot api gateway is running").ExcludeFromDescription();

await app.UseOcelot();

app.Run();
