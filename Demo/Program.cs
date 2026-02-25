using Ocelot.DependencyInjection;
using Ocelot.Middleware;

/* 
 * This is the default DotNet project without any configuration 
 * for route and authentication
 */

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot configuration
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//builder.Services.AddOcelot(builder.Configuration);


//builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

/*
 * Registers controller-related services inside the Dependency Injection container.

It enables:
Controller discovery
Model binding
JSON serialization,Validation, Filters, API behavior, IActionResult execution, Attribute routing metadata

Hire Chef and kitchen setup
*/

var app = builder.Build();

//Since MVC need static files (CSS, JS, Images)
app.UseStaticFiles();

//app.MapGet("/", () => "Hello World!");

//app.MapGet("/display", () => "Learning how to integarte azure");

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

//app.MapControllers();
/*Maps controller routes to the endpoint routing system.
 * open door to let the customer place an order
 */


// Use Ocelot middleware
//await app.UseOcelot();


app.Run();
