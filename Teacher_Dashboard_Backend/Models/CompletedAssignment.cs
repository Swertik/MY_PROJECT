using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Teacher_Dashboard_Backend.Models;

    
[Table("completed_assignments")]
public class CompletedAssignment
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("student_id")]
    public int StudentId { get; set; }

    [JsonIgnore]
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }

    [Column("assignment_id")]
    public int AssignmentId { get; set; }
    [JsonIgnore]
    [ForeignKey("AssignmentId")]
    public Assignment? Assignment { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; }

    [Column("completed_at")]
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}