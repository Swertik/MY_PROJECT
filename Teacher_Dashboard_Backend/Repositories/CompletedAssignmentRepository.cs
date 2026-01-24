namespace Teacher_Dashboard_Backend.Repositories;

using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.Models;
using Teacher_Dashboard_Backend.Services;

public class CompletedAssignmentRepository : ICompletedAssigmentRepository
{
    private readonly DatabaseService _context;

    public CompletedAssignmentRepository(DatabaseService context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
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
            var student = _context.Students.FirstOrDefault(s => s.Id == studentId);
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId);

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

    public CompletedAssignment? AddBulk(int[] studentIds, int assignmentId, bool isCompleted)
    {
        if (studentIds == null || studentIds.Length == 0)
        {
            return null;
        }
        CompletedAssignment? lastRecord = null;
        foreach (var studentId in studentIds)
        {
            lastRecord = ToggleStatus(studentId, assignmentId, isCompleted);
        }
        return lastRecord;
    }
}