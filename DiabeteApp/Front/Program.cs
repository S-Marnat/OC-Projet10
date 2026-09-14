var builder = WebApplication.CreateBuilder(args);

// Configuration des dépendances
builder.Services.AddControllersWithViews();


// Construction de l'application
var app = builder.Build();


// Pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
