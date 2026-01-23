using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Teacher_Dashboard_Backend.Models;

[Table("students")]
public class Student
{
    [Column("id")]
    public int Id { get; set; }
    [Column("first_name")]
    public required string FirstName { get; set; }
    [Column("last_name")]
    public required string LastName { get; set; }
    [Column("email")]
    public required string Email { get; set; }
    [Column("date_of_birth")]
    public DateTime DateOfBirth { get; set; }
    [Column("group_id")]
    public int GroupId { get; set; }
    [JsonIgnore]
    [ForeignKey("GroupId")]
    public Group? Group { get; set; }
    [JsonIgnore]
    public List<CompletedAssignment> CompletedAssignments { get; set; } = new();
}
