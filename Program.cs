using Microsoft.EntityFrameworkCore;
using EdzesKovetoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// MySQL kapcsolat beallitasa
var connectionString = "Server=localhost;Port=3306;Database=EdzesKovetoDb;Uid=root;Pwd=titkosjelszo;";
builder.Services.AddDbContext<EdzesDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Adatbazis automatikus letrehozasa inditaskor
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EdzesDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
