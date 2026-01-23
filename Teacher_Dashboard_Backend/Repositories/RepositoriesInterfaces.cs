using Teacher_Dashboard_Backend.DTOs;
using Teacher_Dashboard_Backend.Models;

namespace Teacher_Dashboard_Backend.Repositories;

public interface IStudentRepository
{
    List<Student> GetAll();
    Student? GetById(int id);
    void Add(Student student);
    bool Delete(int id);
}

public interface IGroupRepository
{
    List<Group> GetAll();
    Group? GetById(int id);
}

public interface IAssignmentRepository
{
    List<Assignment> GetByGroupId(int groupId);
}

public interface ICompletedAssigmentRepository
{
    List<CompletedAssignment> GetByAssignmentId(int assignmentId);
    List<CompletedAssignment> GetStudentProgress(int studentId);
    CompletedAssignment ToggleStatus(int studentId, int assignmentId, bool isCompleted);
    CompletedAssignment? AddBulk(int[] studentIds, int assignmentId, bool isCompleted);
}

public interface IDashboardRepository
{
    // Возвращает агрегированные данные для всех студентов
    List<StudentDashboardItemDto> GetFullDashboard();
}