using System.Text.Json.Serialization;
namespace Teacher_Dashboard_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("assignments")]
public class Assignment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    [Required]
    public string Title { get; set; } = string.Empty;
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    [Column("group_id")]
    public int GroupId { get; set; }
    [JsonIgnore]
    [ForeignKey("GroupId")]
    public Group? Group { get; set; }
}