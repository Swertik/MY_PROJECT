using Teacher_Dashboard_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Data; // Твой namespace с контекстом

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешить доступ с любого URL
              .AllowAnyMethod()  // Разрешить GET, POST, PUT, DELETE и т.д.
              .AllowAnyHeader(); // Разрешить любые заголовки
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Только твой Angular
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer(); // <--- Нужно для минимальных API, но полезно оставить
builder.Services.AddSwaggerGen();           // <--- Сам генератор Swagger

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowAll");
} else
{
    app.UseCors("AllowAngularClient");
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Запускаем наш инициализатор
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
