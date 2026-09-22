using Notes.Data;
using Notes.Repositories.Interfaces;
using Notes.Services.Interfaces;
using Notes.Repositories.Implementations;
using Notes.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);


// Configuration des dépendances
builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddControllers();


// Configuration de MongoDb
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSingleton<MongoDbSettings>(sp =>
    sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoDbSettings>>().Value);


// Construction de l'API
var app = builder.Build();


// Pipeline HTTP
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
