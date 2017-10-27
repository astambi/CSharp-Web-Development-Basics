namespace SimpleMvc.Data
{
    using Domain;
    using Microsoft.EntityFrameworkCore;

    public class NotesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=NotesDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasMany(u => u.Notes)
                .WithOne(n => n.Owner)
                .HasForeignKey(n => n.OwnerId);
        }
    }
}
