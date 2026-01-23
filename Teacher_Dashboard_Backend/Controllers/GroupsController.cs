using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Models;
using System.ComponentModel.DataAnnotations;

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
    public async Task<IActionResult> GetAllGroups()
    {
        try
        {
            var groups = await _context.Groups
                .Include(g => g.Students)
                .Include(g => g.Assignments)
                .ToListAsync();
            return Ok(groups);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении групп", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        try
        {
            var group = await _context.Groups
                .Include(g => g.Students)
                .Include(g => g.Assignments)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound(new { message = "Группа не найдена" });
            }

            return Ok(group);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении группы", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Проверяем уникальность названия группы
            var groupExists = await _context.Groups.AnyAsync(g => g.Name == dto.Name);
            if (groupExists)
            {
                return BadRequest(new { message = "Группа с таким названием уже существует" });
            }

            var group = new Group
            {
                Name = dto.Name
            };

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при создании группы", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound(new { message = "Группа не найдена" });
            }

            // Проверяем уникальность названия (исключая текущую группу)
            var groupExists = await _context.Groups
                .AnyAsync(g => g.Name == dto.Name && g.Id != id);
            if (groupExists)
            {
                return BadRequest(new { message = "Группа с таким названием уже существует" });
            }

            group.Name = dto.Name;
            await _context.SaveChangesAsync();

            return Ok(group);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при обновлении группы", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        try
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound(new { message = "Группа не найдена" });
            }

            // Проверяем, есть ли студенты в группе
            var hasStudents = await _context.Students.AnyAsync(s => s.GroupId == id);
            if (hasStudents)
            {
                return BadRequest(new { message = "Нельзя удалить группу со студентами" });
            }

            // Проверяем, есть ли задания у группы
            var hasAssignments = await _context.Assignments.AnyAsync(a => a.GroupId == id);
            if (hasAssignments)
            {
                return BadRequest(new { message = "Нельзя удалить группу с заданиями" });
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Группа успешно удалена" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при удалении группы", error = ex.Message });
        }
    }
}

// DTOs для групп
public class CreateGroupDto
{
    [Required(ErrorMessage = "Название группы обязательно")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название группы должно быть от 2 до 50 символов")]
    public string Name { get; set; } = string.Empty;
}

public class UpdateGroupDto
{
    [Required(ErrorMessage = "Название группы обязательно")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название группы должно быть от 2 до 50 символов")]
    public string Name { get; set; } = string.Empty;
}