using Microsoft.EntityFrameworkCore;
using Teacher_Dashboard_Backend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CompletedAssignment> CompletedAssignments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    
}