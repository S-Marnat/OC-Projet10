using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Patients.Data;
using Patients.Repositories.Implementations;
using Patients.Repositories.Interfaces;
using Patients.Services.Implementations;
using Patients.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configuration des dépendances
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Configuration de Swagger
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("cookieAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Name = "Cookie",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Description = "Cookie d'authentification Identity"
//    });

//    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//    {
//        {
//            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "cookieAuth"
//                }
//            },
//            new string[] {}
//        }
//    });
//});


// Configuration d'EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configuration d'Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configuration des règles de mot de passe
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization();


// Configuration d'un organisateur par défaut
async Task SeedRolesAndUsers(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // Rôle Organisateur
    if (!await roleManager.RoleExistsAsync("Organisateur"))
        await roleManager.CreateAsync(new IdentityRole("Organisateur"));

    // Utilisateur Organisateur
    var user = await userManager.FindByEmailAsync("organisateur@demo.com");
    if (user == null)
    {
        user = new IdentityUser
        {
            UserName = "organisateur@demo.com",
            Email = "organisateur@demo.com"
        };

        await userManager.CreateAsync(user, "Password123!");
        await userManager.AddToRoleAsync(user, "Organisateur");
    }
}


// Construction de l'API
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndUsers(services);
}


// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
