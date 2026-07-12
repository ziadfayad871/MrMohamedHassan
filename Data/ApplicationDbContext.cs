using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<StudentGroup> StudentGroups => Set<StudentGroup>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<ExpenseCategory> ExpenseCategories => Set<ExpenseCategory>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<ExamResult> ExamResults => Set<ExamResult>();
    public DbSet<Homework> Homeworks => Set<Homework>();
    public DbSet<HomeworkSubmission> HomeworkSubmissions => Set<HomeworkSubmission>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<StudentGroup>()
            .HasIndex(sg => new { sg.StudentId, sg.GroupId })
            .IsUnique();

        builder.Entity<Attendance>()
            .HasIndex(a => new { a.StudentId, a.GroupId, a.Date })
            .IsUnique();

        builder.Entity<ExamResult>()
            .HasIndex(er => new { er.ExamId, er.StudentId })
            .IsUnique();

        builder.Entity<HomeworkSubmission>()
            .HasIndex(hs => new { hs.HomeworkId, hs.StudentId })
            .IsUnique();

        builder.Entity<Student>()
            .HasQueryFilter(s => !s.IsDeleted);

        builder.Entity<Group>()
            .HasQueryFilter(g => !g.IsDeleted);

        builder.Entity<Teacher>()
            .HasQueryFilter(t => !t.IsDeleted);
    }
}
