using Teacher_Dashboard_Backend.Models;
using Teacher_Dashboard_Backend.Services;

namespace Teacher_Dashboard_Backend.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DatabaseService _context;

    public StudentRepository(DatabaseService context)
    {
        _context = context;
    }

    public List<Student> GetAll() => _context.Students;

    public Student? GetById(int id) => _context.Students.FirstOrDefault(s => s.Id == id);

    public void Add(Student student)
    {
        student.Id = _context.Students.Any() ? _context.Students.Max(s => s.Id) + 1 : 1;
        _context.Students.Add(student);
    }

    public bool Delete(int id)
    {
        var student = GetById(id);
        if (student == null) return false;

        // Удаляем из групп (ручная работа с связями, так как это мок)
        foreach (var group in _context.Groups)
        {
            group.Students.Remove(student);
        }
        
        // Удаляем оценки
        _context.CompletedAssignments.RemoveAll(ca => ca.StudentId == id);
        
        // Удаляем студента
        _context.Students.Remove(student);
        return true;
    }
}