using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Teacher_Dashboard_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Требуем авторизацию для всех методов
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<StudentDashboardItemDto>> GetDashboardAsync()
{
    var result = await _context.Students
        .AsNoTracking() // Важно: ускоряет чтение, так как мы не собираемся менять данные тут
        .Select(student => new StudentDashboardItemDto
        {
            Id = student.Id,
            Fio = $"{student.LastName} {student.FirstName}",
            
            // 1. Берем имя группы через связь (JOIN)
            GroupName = student.Group != null ? student.Group.Name : "Без группы",

            // 2. Берем задания группы (подзапрос к Assignments)
            AllGroupTasks = student.Group != null ? student.Group.Assignments.Select(a => new TaskDto 
            {
                Id = a.Id,
                Name = a.Title
            }).ToList() : new List<TaskDto>(),

            // 3. Берем историю конкретного студента (подзапрос к CompletedAssignments)
            TasksHistory = student.CompletedAssignments.Select(ca => new HistoryDto
            {
                RecordId = ca.Id,
                AssignmentId = ca.AssignmentId,
                // Тут EF сам подтянет Title из связанной таблицы Assignment
                TaskName = ca.Assignment != null ? ca.Assignment.Title : "Неизвестное задание", 
                IsCompleted = ca.IsCompleted,
                CompletedAt = ca.CompletedAt
            }).ToList()
        })
        // Опционально: AsSplitQuery предотвращает "взрыв" данных при множественных Join
        .AsSplitQuery() 
        .ToListAsync();

    return result;
}
}