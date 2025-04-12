// Models/CourseRegistrationDbContext.cs
using Microsoft.EntityFrameworkCore;
using CourseRegistration.Models.Entities;

namespace CourseRegistration.Models
{
    public class CourseRegistrationDbContext : DbContext
    {
        public CourseRegistrationDbContext(DbContextOptions<CourseRegistrationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Section> Sections { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Department entity
            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            modelBuilder.Entity<Department>()
                .Property(d => d.Code)
                .IsRequired()
                .HasMaxLength(10);
            
            // Configure Course entity
            modelBuilder.Entity<Course>()
                .Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(20);
                
            modelBuilder.Entity<Course>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);
                
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure Section entity
            modelBuilder.Entity<Section>()
                .Property(s => s.SectionNumber)
                .IsRequired()
                .HasMaxLength(10);
                
            modelBuilder.Entity<Section>()
                .Property(s => s.Semester)
                .IsRequired()
                .HasMaxLength(20);
                
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Sections)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure Student entity
            modelBuilder.Entity<Student>()
                .Property(s => s.StudentId)
                .IsRequired()
                .HasMaxLength(20);
                
            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            modelBuilder.Entity<Student>()
                .Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);
                
            modelBuilder.Entity<Student>()
                .Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(20);
                
            // Configure Registration entity
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Section)
                .WithMany(s => s.Registrations)
                .HasForeignKey(r => r.SectionId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Registrations)
                .HasForeignKey(r => r.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}