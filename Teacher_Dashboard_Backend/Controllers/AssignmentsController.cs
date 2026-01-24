using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.Repositories;

[ApiController]
[Route("api/[controller]")]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentRepository _repo;

    // Внедряем интерфейс
    public AssignmentsController(IAssignmentRepository repo)
    {
        if (repo == null)
        {
            throw new ArgumentNullException(nameof(repo));
        }
        _repo = repo;
    }

    [HttpGet("by-group/{groupId}")]
    public IActionResult GetByGroupId(int groupId)
    {
        var assignments = _repo.GetByGroupId(groupId);
        return Ok(assignments);
    }
}