using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Lecture> Lectures => Set<Lecture>();
    
    public virtual DbSet<Student> Students => Set<Student>();
    
    public virtual DbSet<Teacher> Teachers => Set<Teacher>();

    public virtual DbSet<StudentLecture> StudentLectures => Set<StudentLecture>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}