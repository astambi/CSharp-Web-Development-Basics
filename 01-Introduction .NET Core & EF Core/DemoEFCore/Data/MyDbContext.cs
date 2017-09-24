namespace DemoEFCore.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MyDbContext : DbContext
    {
        //public DbSet<Person> People { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TestDb;Integrated Security=True");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmeentId);

            base.OnModelCreating(builder);
        }
    }
}
