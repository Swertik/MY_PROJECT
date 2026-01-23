namespace Teacher_Dashboard_Backend.Models;

public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
    }