using System.ComponentModel.DataAnnotations.Schema;

namespace Teacher_Dashboard_Backend.Models;

[Table("groups")]
public class Group
    {

        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
    }