namespace ManyToManyRelation.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StudentsCoursesDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        //public DbSet<StudentCourse> StudentsCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=StudentsDb;Integrated Security=True;");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Many-to-Many
            builder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            builder
                 .Entity<Student>()
                 .HasMany(s => s.StudentsCourses)
                 .WithOne(sc => sc.Student)
                 .HasForeignKey(sc => sc.StudentId);

            builder
                .Entity<Course>()
                .HasMany(c => c.StudentsCourses)
                .WithOne(sc => sc.Course)
                .HasForeignKey(sc => sc.CourseId);

            //builder
            //    .Entity<StudentCourse>()
            //    .HasOne(sc => sc.Student)
            //    .WithMany(s => s.StudentsCourses)
            //    .HasForeignKey(sc => sc.StudentId);

            //builder
            //    .Entity<StudentCourse>()
            //    .HasOne(sc => sc.Course)
            //    .WithMany(c => c.StudentsCourses)
            //    .HasForeignKey(sc => sc.CourseId);

            base.OnModelCreating(builder);
        }
    }
}
