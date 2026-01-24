using Teacher_Dashboard_Backend.Repositories;
using Teacher_Dashboard_Backend.Services;

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

builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<ICompletedAssigmentRepository, CompletedAssignmentRepository>();

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

app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
