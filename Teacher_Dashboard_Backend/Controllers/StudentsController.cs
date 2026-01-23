using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Models;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        try
        {
            var students = await _context.Students
                .Include(s => s.Group)
                .Where(s => s.Group != null) // Только студенты с группами
                .ToListAsync();
            return Ok(students);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении студентов", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        try
        {
            var student = await _context.Students
                .Include(s => s.Group)
                .Include(s => s.CompletedAssignments)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new { message = "Студент не найден" });
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении студента", error = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto dto)
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

            // Проверяем уникальность email
            var emailExists = await _context.Students.AnyAsync(s => s.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "Студент с таким email уже существует" });
            }

            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                GroupId = dto.GroupId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Возвращаем созданного студента с группой
            var createdStudent = await _context.Students
                .Include(s => s.Group)
                .FirstAsync(s => s.Id == student.Id);

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, createdStudent);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при создании студента", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Студент не найден" });
            }

            // Проверяем существование группы
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == dto.GroupId);
            if (!groupExists)
            {
                return BadRequest(new { message = "Группа не найдена" });
            }

            // Проверяем уникальность email (исключая текущего студента)
            var emailExists = await _context.Students
                .AnyAsync(s => s.Email == dto.Email && s.Id != id);
            if (emailExists)
            {
                return BadRequest(new { message = "Студент с таким email уже существует" });
            }

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.DateOfBirth = dto.DateOfBirth;
            student.GroupId = dto.GroupId;

            await _context.SaveChangesAsync();

            var updatedStudent = await _context.Students
                .Include(s => s.Group)
                .FirstAsync(s => s.Id == id);

            return Ok(updatedStudent);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при обновлении студента", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        try
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Студент не найден" });
            }

            // Проверяем, есть ли связанные записи
            var hasCompletedAssignments = await _context.CompletedAssignments
                .AnyAsync(ca => ca.StudentId == id);

            if (hasCompletedAssignments)
            {
                return BadRequest(new { message = "Нельзя удалить студента с выполненными заданиями" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Студент успешно удален" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при удалении студента", error = ex.Message });
        }
    }

    // SQL запрос для поиска студентов
    [HttpGet("search")]
    public async Task<IActionResult> SearchStudents([FromQuery] string? query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await GetStudents();
            }

            var sql = @"
                SELECT s.*, g.name as GroupName
                FROM students s
                INNER JOIN groups g ON s.group_id = g.id
                WHERE LOWER(s.first_name) LIKE LOWER({0}) 
                   OR LOWER(s.last_name) LIKE LOWER({0})
                   OR LOWER(s.email) LIKE LOWER({0})
                   OR LOWER(g.name) LIKE LOWER({0})
                ORDER BY s.last_name, s.first_name";

            var searchPattern = $"%{query}%";
            
            var students = await _context.Students
                .FromSqlRaw(sql, searchPattern)
                .Include(s => s.Group)
                .ToListAsync();

            return Ok(students);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при поиске студентов", error = ex.Message });
        }
    }
}

// DTOs для студентов
public class CreateStudentDto
{
    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата рождения обязательна")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "ID группы обязателен")]
    public int GroupId { get; set; }
}

public class UpdateStudentDto
{
    [Required(ErrorMessage = "Имя обязательно")]
    [StringLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия обязательна")]
    [StringLength(50, ErrorMessage = "Фамилия не должна превышать 50 символов")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Дата рождения обязательна")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "ID группы обязателен")]
    public int GroupId { get; set; }
}