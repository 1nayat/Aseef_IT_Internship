using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;

namespace EmployeeManagement.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TeacherClassSubject> TeacherClassSubjects { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //////////////////////////////////////
        modelBuilder.Entity<Class>()
      .HasQueryFilter(c => !c.IsDeleted);
        ////////////////////////////////////

        modelBuilder.Entity<TeacherClassSubject>()
            .HasKey(tcs => new { tcs.TeacherId, tcs.ClassId, tcs.SubjectId });

        modelBuilder.Entity<TeacherClassSubject>()
            .HasOne(tcs => tcs.Teacher)
            .WithMany(t => t.TeacherClassSubjects)
            .HasForeignKey(tcs => tcs.TeacherId);

        modelBuilder.Entity<TeacherClassSubject>()
            .HasOne(tcs => tcs.Class)
            .WithMany(c => c.TeacherClassSubjects)
            .HasForeignKey(tcs => tcs.ClassId);

        modelBuilder.Entity<TeacherClassSubject>()
            .HasOne(tcs => tcs.Subject)
            .WithMany(s => s.TeacherClassSubjects)
            .HasForeignKey(tcs => tcs.SubjectId);
        modelBuilder.Entity<TeacherClassSubject>()
    .HasQueryFilter(tcs => !tcs.Class.IsDeleted);

    }
}
