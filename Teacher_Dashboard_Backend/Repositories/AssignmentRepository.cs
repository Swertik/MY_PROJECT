using Teacher_Dashboard_Backend.Models;
using Teacher_Dashboard_Backend.Services;

namespace Teacher_Dashboard_Backend.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly DatabaseService _context;

    public AssignmentRepository(DatabaseService context)
    {
        _context = context;
    }

    public List<Assignment> GetByGroupId(int groupId)
    {
        return _context.Assignments.Where(a => a.GroupId == groupId).ToList();
    }
}