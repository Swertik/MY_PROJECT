using Teacher_Dashboard_Backend.Models;
using Teacher_Dashboard_Backend.Services;

namespace Teacher_Dashboard_Backend.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly DatabaseService _context;

    public GroupRepository(DatabaseService context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        _context = context;
    }

    public List<Group> GetAll()
    {
        return _context.Groups;
    }

    public Group? GetById(int id)
    {
        return _context.Groups.FirstOrDefault(g => g.Id == id);
    }
}