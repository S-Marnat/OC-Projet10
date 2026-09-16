using Front.Services;

var builder = WebApplication.CreateBuilder(args);


// Configuration des dépendances
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<PatientApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7181");
});


// Configuration de l'authentification et de l'autorisation
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();


// Construction de l'application
var app = builder.Build();


// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}")
    .WithStaticAssets();

app.Run();
