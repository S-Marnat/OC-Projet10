using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Configuration d'Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies");
builder.Services.AddAuthorization();

builder.Services.AddOcelot();


// Construction de l'API
var app = builder.Build();


// Pipeline HTTP
app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();