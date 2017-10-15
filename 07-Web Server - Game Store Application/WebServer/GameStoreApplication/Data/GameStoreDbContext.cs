namespace WebServer.GameStoreApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GameStoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                 .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=GameStoreDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Indices
            builder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Many-to-Many
            builder
                .Entity<UserGame>()
                .HasKey(ug => new { ug.UserId, ug.GameId });

            builder
                .Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(ug => ug.User)
                .HasForeignKey(ug => ug.UserId);

            builder
                .Entity<Game>()
                .HasMany(g => g.Users)
                .WithOne(ug => ug.Game)
                .HasForeignKey(ug => ug.GameId);
        }
    }
}
