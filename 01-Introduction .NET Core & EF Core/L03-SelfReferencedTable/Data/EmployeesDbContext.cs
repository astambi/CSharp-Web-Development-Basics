namespace SelfReferencedTable.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class EmployeesDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=EmployeesDb;Integrated Security=True;");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // One-to-Many
            builder
                 .Entity<Employee>()
                 .HasOne(e => e.Department)
                 .WithMany(d => d.Employees)
                 .HasForeignKey(e => e.DepartmentId);
            
            // Self-reference
            builder
                .Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
