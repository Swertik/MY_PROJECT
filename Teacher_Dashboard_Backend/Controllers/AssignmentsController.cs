using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class AssignmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    // Внедряем интерфейс
    public AssignmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("by-group/{groupId}")]
    public IActionResult GetByGroupId(int groupId)
    {
        var assignments = _context.Assignments.Where(a => a.GroupId == groupId).ToList();
        return Ok(assignments);
    }
}