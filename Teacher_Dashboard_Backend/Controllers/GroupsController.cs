using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly AppDbContext _context;

    public GroupsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllGroups()
    {
        var groups = _context.Groups.ToList();
        return Ok(groups);
    }

    [HttpGet("{id}")]
    public IActionResult GetGroup(int id)
    {
        var group = _context.Groups.FirstOrDefault(g => g.Id == id);

        if (group == null)
            return NotFound();

        return Ok(group);
    }
}