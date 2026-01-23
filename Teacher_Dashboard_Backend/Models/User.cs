using System.ComponentModel.DataAnnotations.Schema;

namespace Teacher_Dashboard_Backend.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }
    [Column("username")]
    public string Username { get; set; } = string.Empty;
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;
    [Column("role")]
    public string Role { get; set; } = "Teacher";
}