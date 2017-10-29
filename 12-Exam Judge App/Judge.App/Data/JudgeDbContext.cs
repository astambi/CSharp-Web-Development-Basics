namespace Judge.App.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class JudgeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Contest> Contests { get; set; }

        public DbSet<Submission> Submissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=JudgeDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Unique User Email index
            builder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // One-to-Many
            builder
                .Entity<User>()
                .HasMany(u => u.Contests)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .Entity<Contest>()
               .HasMany(u => u.Submissions)
               .WithOne(s => s.Contest)
               .HasForeignKey(s => s.ContestId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Submission>()
                .HasOne(s => s.User)
                .WithMany(u => u.Submissions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
