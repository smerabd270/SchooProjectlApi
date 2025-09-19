using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<Grade> Grades => Set<Grade>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for many-to-many
            modelBuilder.Entity<Enrollment>().HasKey(e => new { e.UserId, e.CourseId });

            // Unique username
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            // ------------------ Relationships ------------------

            // Enrollment → User
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enrollment → Course
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment → Course
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade → Assignment
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Assignment)
                .WithMany(a => a.Grades)
                .HasForeignKey(g => g.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade → User (Student)
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
.               WithMany(u => u.Grades)
                .HasForeignKey(g => g.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ------------------ Seed Data ------------------

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), FullName = "Admin", Role = "Admin" },
                new User { Id = 2, Username = "teacher1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("teacher123"), FullName = "Teacher One", Role = "Teacher" },
                new User { Id = 3, Username = "student1", PasswordHash = BCrypt.Net.BCrypt.HashPassword("student123"), FullName = "Student One", Role = "Student" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "Math 101", Description = "Intro to Math", TeacherId = 2 }
            );

            modelBuilder.Entity<Assignment>().HasData(
                new Assignment { Id = 1, Title = "Algebra HW", Description = "Solve problems", DueDate = DateTime.UtcNow.AddDays(7), CourseId = 1 }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { UserId = 3, CourseId = 1, EnrolledAt = DateTime.UtcNow }
            );

            modelBuilder.Entity<Grade>().HasData(
                new Grade { Id = 1, AssignmentId = 1, StudentId = 3, Submitted = true, Score = 88, SubmittedAt = DateTime.UtcNow.AddDays(-1), Feedback = "Good job" }
            );
        }
    }
}
