using Teacher_Dashboard_Backend.Repositories;
using Teacher_Dashboard_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

builder.Services.AddEndpointsApiExplorer(); // <--- Нужно для минимальных API, но полезно оставить
builder.Services.AddSwaggerGen();           // <--- Сам генератор Swagger

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
