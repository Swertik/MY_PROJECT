using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Models;


[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    // Внедряем контекст напрямую
    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Student>> GetStudents()
    {
        // Используем DbSet как репозиторий
        return await _context.Students
            .Include(s => s.Group) // Сразу подгружаем группу (JOIN)
            .ToListAsync();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync(); // Unit of Work: сохраняем транзакцию
        return Ok(student);
    }
}