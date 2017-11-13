using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(n => n.Name).HasColumnType("nvarchar(100)");

                entity.Property(p => p.PhoneNumber).IsRequired(false).HasColumnType("char(10)");

                entity.Property(b => b.Birthday).IsRequired(false);

                entity
                    .HasMany(s => s.HomeworkSubmissions)
                    .WithOne(h => h.Student)
                    .HasForeignKey(s => s.StudentId);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(n => n.Name).HasColumnType("nvarchar(80)");

                entity.Property(d => d.Description).IsRequired(false).HasColumnType("nvarchar(max)");

                entity
                    .HasMany(c => c.HomeworkSubmissions)
                    .WithOne(h => h.Course)
                    .HasForeignKey(c => c.CourseId);

                entity
                    .HasMany(c => c.Resources)
                    .WithOne(r => r.Course)
                    .HasForeignKey(c => c.CourseId);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(n => n.Name).HasColumnType("nvarchar(50)");

                entity.Property(u => u.Url).HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.Property(c => c.Content).HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new
                {
                    sc.StudentId,
                    sc.CourseId
                });
        }
    }
}
