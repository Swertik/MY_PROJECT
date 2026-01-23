namespace Teacher_Dashboard_Backend.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Models;

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
    public async Task<IActionResult> GetByAssignmentId(int assignmentId)
    {
        var completedAssignments = await _context.CompletedAssignments
            .Where(ca => ca.AssignmentId == assignmentId && ca.DeletedAt == null)
            .ToListAsync();
        return Ok(completedAssignments);
    }

    [HttpGet("student-progress/{studentId}")]
    public async Task<IActionResult> GetStudentProgress(int studentId)
    {
        var completedAssignments = await _context.CompletedAssignments
            .Where(ca => ca.StudentId == studentId && ca.DeletedAt == null)
            .Include(ca => ca.Assignment)
            .ToListAsync();
        return Ok(completedAssignments);
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleStatus([FromBody] ToggleStatusDto dto)
    {
        try
        {
            var existingRecord = await _context.CompletedAssignments
                .FirstOrDefaultAsync(ca => ca.StudentId == dto.StudentId && 
                                         ca.AssignmentId == dto.AssignmentId && 
                                         ca.DeletedAt == null);

            if (existingRecord != null)
            {
                // Обновляем существующую запись
                existingRecord.IsCompleted = dto.IsCompleted;
                existingRecord.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : DateTime.MinValue;
            }
            else
            {
                // Создаем новую запись
                var newRecord = new CompletedAssignment
                {
                    StudentId = dto.StudentId,
                    AssignmentId = dto.AssignmentId,
                    IsCompleted = dto.IsCompleted,
                    CompletedAt = dto.IsCompleted ? DateTime.UtcNow : DateTime.MinValue
                };
                _context.CompletedAssignments.Add(newRecord);
            }

            await _context.SaveChangesAsync();
            
            var recordId = existingRecord?.Id ?? 
                          (await _context.CompletedAssignments
                              .Where(ca => ca.StudentId == dto.StudentId && ca.AssignmentId == dto.AssignmentId)
                              .OrderByDescending(ca => ca.Id)
                              .FirstAsync()).Id;

            return Ok(recordId);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при обновлении статуса", error = ex.Message });
        }
    }

    [HttpPost("massCreate")]
    public async Task<IActionResult> CreateMassCompletedAssignments([FromBody] MassCreateDto dto)
    {
        try
        {
            var recordsToAdd = new List<CompletedAssignment>();
            var recordsToUpdate = new List<CompletedAssignment>();

            foreach (var studentId in dto.StudentIds)
            {
                var existingRecord = await _context.CompletedAssignments
                    .FirstOrDefaultAsync(ca => ca.StudentId == studentId && 
                                             ca.AssignmentId == dto.AssignmentId && 
                                             ca.DeletedAt == null);

                if (existingRecord != null)
                {
                    existingRecord.IsCompleted = dto.IsCompleted;
                    existingRecord.CompletedAt = dto.IsCompleted ? DateTime.UtcNow : DateTime.MinValue;
                    recordsToUpdate.Add(existingRecord);
                }
                else
                {
                    var newRecord = new CompletedAssignment
                    {
                        StudentId = studentId,
                        AssignmentId = dto.AssignmentId,
                        IsCompleted = dto.IsCompleted,
                        CompletedAt = dto.IsCompleted ? DateTime.UtcNow : DateTime.MinValue
                    };
                    recordsToAdd.Add(newRecord);
                }
            }

            if (recordsToAdd.Any())
            {
                _context.CompletedAssignments.AddRange(recordsToAdd);
            }

            await _context.SaveChangesAsync();

            return Ok(new { 
                message = $"Обновлено {recordsToUpdate.Count + recordsToAdd.Count} записей",
                created = recordsToAdd.Count,
                updated = recordsToUpdate.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при массовом создании", error = ex.Message });
        }
    }

    [HttpDelete("{recordId}")]
    public async Task<IActionResult> DeleteRecord(int recordId)
    {
        try
        {
            var record = await _context.CompletedAssignments.FindAsync(recordId);
            
            if (record == null)
            {
                return NotFound(new { message = "Запись не найдена" });
            }

            // Soft delete
            record.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Запись успешно удалена" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при удалении записи", error = ex.Message });
        }
    }

    // SQL запрос для получения статистики по группам
    [HttpGet("group-statistics")]
    public async Task<IActionResult> GetGroupStatistics()
    {
        try
        {
            var sql = @"
                SELECT 
                    g.name as GroupName,
                    COUNT(DISTINCT s.id) as TotalStudents,
                    COUNT(DISTINCT a.id) as TotalAssignments,
                    COUNT(ca.id) as CompletedAssignments,
                    ROUND(
                        CASE 
                            WHEN COUNT(DISTINCT s.id) * COUNT(DISTINCT a.id) = 0 THEN 0
                            ELSE (COUNT(ca.id)::decimal / (COUNT(DISTINCT s.id) * COUNT(DISTINCT a.id))) * 100
                        END, 2
                    ) as CompletionPercentage
                FROM groups g
                LEFT JOIN students s ON g.id = s.group_id
                LEFT JOIN assignments a ON g.id = a.group_id
                LEFT JOIN completed_assignments ca ON s.id = ca.student_id 
                    AND a.id = ca.assignment_id 
                    AND ca.is_completed = true 
                    AND ca.deleted_at IS NULL
                GROUP BY g.id, g.name
                ORDER BY CompletionPercentage DESC";

            var result = await _context.Database.SqlQueryRaw<GroupStatisticsDto>(sql).ToListAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Ошибка при получении статистики", error = ex.Message });
        }
    }
}

// DTOs для новых методов
public class ToggleStatusDto
{
    public int StudentId { get; set; }
    public int AssignmentId { get; set; }
    public bool IsCompleted { get; set; }
}

public class MassCreateDto
{
    public int[] StudentIds { get; set; } = Array.Empty<int>();
    public int AssignmentId { get; set; }
    public bool IsCompleted { get; set; }
}

public class GroupStatisticsDto
{
    public string GroupName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalAssignments { get; set; }
    public int CompletedAssignments { get; set; }
    public decimal CompletionPercentage { get; set; }
}