namespace Teacher_Dashboard_Backend.DTOs;

public class StudentDashboardItemDto
{
    public int Id { get; set; }
    public string Fio { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public List<HistoryDto> TasksHistory { get; set; } = new();
    public List<TaskDto> AllGroupTasks { get; set; } = new();
}