namespace Teacher_Dashboard_Backend.Controllers;

using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.Repositories;

[ApiController]
[Route("api/[controller]")]
public class CompletedAssignmentController : ControllerBase
{
    private readonly ICompletedAssigmentRepository _repo;

    public CompletedAssignmentController(ICompletedAssigmentRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("by-assignment/{assignmentId}")]
    public IActionResult GetByAssignmentId(int assignmentId)
    {
        var completedAssignments = _repo.GetByAssignmentId(assignmentId);
        return Ok(completedAssignments);
    }

    [HttpGet("student-progress/{studentId}")]
    public IActionResult GetStudentProgress(int studentId)
    {
        var completedAssignments = _repo.GetStudentProgress(studentId);
        return Ok(completedAssignments);
    }

    [HttpPost("toggle")]
    public IActionResult ToggleStatus(int studentId, int assignmentId, bool isCompleted)
    {
        var result = _repo.ToggleStatus(studentId, assignmentId, isCompleted);
        return Ok(result);
    }

    [HttpPost("massCreate")]
    public IActionResult CreateMassCompletedAssignments(int[] studentIds, int assignmentId,  bool isCompleted)
    {
        var result = _repo.AddBulk(studentIds, assignmentId, isCompleted);
        return Ok(result);
    }
}