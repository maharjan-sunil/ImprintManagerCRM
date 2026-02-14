using SharedServices.API.Extensions;
using SharedServices.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "AllowFrontend";
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// Add services to the container.
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Shared Service API V1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseCors(corsPolicy);

app.UseAuthentication();
app.UseAuthorization();

await DatabaseSeeder.SeedAsync(app.Services);

app.MapControllers();

app.Run();
