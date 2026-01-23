namespace Teacher_Dashboard_Backend.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CompletedAssignmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompletedAssignmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("by-assignment/{assignmentId}")]
    public IActionResult GetByAssignmentId(int assignmentId)
    {
        var completedAssignments = _context.CompletedAssignments.Where(ca => ca.AssignmentId == assignmentId).ToList();
        return Ok(completedAssignments);
    }

    [HttpGet("student-progress/{studentId}")]
    public IActionResult GetStudentProgress(int studentId)
    {
        var completedAssignments = _context.CompletedAssignments.Where(ca => ca.StudentId == studentId).ToList();
        return Ok(completedAssignments);
    }

    [HttpPost("toggle")]
    public IActionResult ToggleStatus(int studentId, int assignmentId, bool isCompleted)
    {
        var result = _context.CompletedAssignments.Where(ca => ca.StudentId == studentId && ca.AssignmentId == assignmentId).ToList();
        return Ok(result);
    }

    [HttpPost("massCreate")]
    public IActionResult CreateMassCompletedAssignments(int[] studentIds, int assignmentId,  bool isCompleted)
    {
        var result = _context.CompletedAssignments.Where(ca => ca.StudentId == studentIds[0] && ca.AssignmentId == assignmentId).ToList();
        return Ok(result);
    }
}