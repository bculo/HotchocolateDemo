using HotchocolateEndpoint.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotchocolateEndpoint.Services;

public class SchoolDbContext : DbContext
{
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Instructor> Instructors { get; set; }
    
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) {}
}