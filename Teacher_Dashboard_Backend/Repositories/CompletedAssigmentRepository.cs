namespace Teacher_Dashboard_Backend.Repositories;

using Teacher_Dashboard_Backend.Models;
using Teacher_Dashboard_Backend.Services;

public class CompletedAssigmentRepository : ICompletedAssigmentRepository
{
    private readonly DatabaseService _context;

    public CompletedAssigmentRepository(DatabaseService context)
    {
        _context = context;
    }

    public List<CompletedAssignment> GetByAssignmentId(int assignmentId)
    {
        return _context.CompletedAssignments.Where(ca => ca.AssignmentId == assignmentId).ToList();
    }

    
    public List<CompletedAssignment> GetStudentProgress(int studentId)
    {
        return _context.CompletedAssignments.Where(ca => ca.StudentId == studentId).ToList();
    }

    public CompletedAssignment ToggleStatus(int studentId, int assignmentId, bool isCompleted)
    {
        var record = _context.CompletedAssignments
            .FirstOrDefault(ca => ca.StudentId == studentId && ca.AssignmentId == assignmentId);

        if (record == null)
        {
            // Создаем новую запись, если её не было
            var student = _context.Students.First(s => s.Id == studentId);
            var assignment = _context.Assignments.First(a => a.Id == assignmentId);

            int newId = _context.CompletedAssignments.Any() ? _context.CompletedAssignments.Max(c => c.Id) + 1 : 1;

            record = new CompletedAssignment
            {
                Id = newId,
                StudentId = studentId,
                Student = student,
                AssignmentId = assignmentId,
                Assignment = assignment,
                IsCompleted = isCompleted,
                CompletedAt = DateTime.UtcNow
            };
            _context.CompletedAssignments.Add(record);
        }
        else
        {
            // Обновляем существующую
            record.IsCompleted = isCompleted;
            record.CompletedAt = DateTime.UtcNow;
        }

        return record;
    }
}