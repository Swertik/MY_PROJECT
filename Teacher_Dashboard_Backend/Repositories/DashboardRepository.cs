using Teacher_Dashboard_Backend.DTOs;
using Teacher_Dashboard_Backend.Services;

namespace Teacher_Dashboard_Backend.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly DatabaseService _context;

    public DashboardRepository(DatabaseService context)
    {
        _context = context;
    }

    public List<StudentDashboardItemDto> GetFullDashboard()
    {
        var result = new List<StudentDashboardItemDto>();

        foreach (var student in _context.Students)
        {
            // 1. Ищем группу студента
            // Так как у нас mock и нет прямого GroupId в Student, ищем, в чьем списке он находится
            var group = _context.Groups.FirstOrDefault(g => g.Students.Any(s => s.Id == student.Id));
            
            // 2. Собираем все задания этой группы
            var groupTasks = new List<TaskDto>();
            if (group != null)
            {
                // Берем задания, привязанные к группе
                groupTasks = _context.Assignments
                    .Where(a => a.GroupId == group.Id)
                    .Select(a => new TaskDto { Id = a.Id, Name = a.Title })
                    .ToList();
            }

            // 3. Собираем историю сдач этого конкретного студента
            var history = _context.CompletedAssignments
                .Where(ca => ca.StudentId == student.Id)
                .Select(ca => new HistoryDto
                {
                    RecordId = ca.Id,
                    AssignmentId = ca.AssignmentId,
                    TaskName = ca.Assignment?.Title ?? "Unknown", // Безопасное получение имени
                    IsCompleted = ca.IsCompleted,
                    CompletedAt = ca.CompletedAt
                })
                .ToList();

            // 4. Формируем итоговый объект
            result.Add(new StudentDashboardItemDto
            {
                Id = student.Id,
                Fio = $"{student.LastName} {student.FirstName}", // Собираем ФИО обратно
                GroupName = group?.Name ?? "Без группы",
                AllGroupTasks = groupTasks,
                TasksHistory = history
            });
        }

        return result;
    }
}