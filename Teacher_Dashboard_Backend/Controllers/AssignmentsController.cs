using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Models;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AssignmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssignmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAssignments()
    {
        try
        {
            var assignments = await _context.Assignments
                .Include(a => a.Group)
                .ToListAsync();
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении заданий", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAssignment(int id)
    {
        try
        {
            var assignment = await _context.Assignments
                .Include(a => a.Group)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound(new { message = "Задание не найдено" });
            }

            return Ok(assignment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении задания", error = ex.Message });
        }
    }

    [HttpGet("by-group/{groupId}")]
    public async Task<IActionResult> GetByGroupId(int groupId)
    {
        try
        {
            var assignments = await _context.Assignments
                .Where(a => a.GroupId == groupId)
                .Include(a => a.Group)
                .ToListAsync();
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении заданий группы", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Проверяем существование группы
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == dto.GroupId);
            if (!groupExists)
            {
                return BadRequest(new { message = "Группа не найдена" });
            }

            var assignment = new Assignment
            {
                Title = dto.Title,
                Description = dto.Description,
                GroupId = dto.GroupId
            };

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            var createdAssignment = await _context.Assignments
                .Include(a => a.Group)
                .FirstAsync(a => a.Id == assignment.Id);

            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, createdAssignment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при создании задания", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssignment(int id, [FromBody] UpdateAssignmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound(new { message = "Задание не найдено" });
            }

            // Проверяем существование группы
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == dto.GroupId);
            if (!groupExists)
            {
                return BadRequest(new { message = "Группа не найдена" });
            }

            assignment.Title = dto.Title;
            assignment.Description = dto.Description;
            assignment.GroupId = dto.GroupId;

            await _context.SaveChangesAsync();

            var updatedAssignment = await _context.Assignments
                .Include(a => a.Group)
                .FirstAsync(a => a.Id == id);

            return Ok(updatedAssignment);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при обновлении задания", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        try
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound(new { message = "Задание не найдено" });
            }

            // Проверяем, есть ли связанные выполненные задания
            var hasCompletedAssignments = await _context.CompletedAssignments
                .AnyAsync(ca => ca.AssignmentId == id);

            if (hasCompletedAssignments)
            {
                return BadRequest(new { message = "Нельзя удалить задание с выполненными работами" });
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Задание успешно удалено" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при удалении задания", error = ex.Message });
        }
    }
}

// DTOs для заданий
public class CreateAssignmentDto
{
    [Required(ErrorMessage = "Название задания обязательно")]
    [StringLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "ID группы обязателен")]
    public int GroupId { get; set; }
}

public class UpdateAssignmentDto
{
    [Required(ErrorMessage = "Название задания обязательно")]
    [StringLength(200, ErrorMessage = "Название не должно превышать 200 символов")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "ID группы обязателен")]
    public int GroupId { get; set; }
}