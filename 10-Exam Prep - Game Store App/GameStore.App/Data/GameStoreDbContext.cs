namespace GameStore.App.Data
{
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class GameStoreDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=GameStoreDb;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Unique index
            builder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Many-to-Many
            builder
                .Entity<Order>()
                .HasKey(o => new { o.UserId, o.GameId });

            builder
                .Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Games)
                .HasForeignKey(o => o.UserId);

            builder
                .Entity<Order>()
                .HasOne(o => o.Game)
                .WithMany(g => g.Users)
                .HasForeignKey(o => o.GameId);
        }
    }
}
