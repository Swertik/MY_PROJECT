using System.Text.Json.Serialization;
namespace Teacher_Dashboard_Backend.Models;
    
public class Assignment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public int GroupId { get; set; }
    [JsonIgnore]
    public Group? Group { get; set; }
}