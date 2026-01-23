namespace Teacher_Dashboard_Backend.DTOs;

public class HistoryDto
{
    public int RecordId { get; set; }
    public int AssignmentId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
}