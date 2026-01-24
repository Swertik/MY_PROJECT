namespace Teacher_Dashboard_Backend.Models;
    
public class CompletedAssignment
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }

    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }
}