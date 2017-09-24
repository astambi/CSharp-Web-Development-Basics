namespace SocialNetwork.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class SocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        // Task 3
        public DbSet<Album> Albums { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        // Task 4
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=SocialNetworkDb;Integrated Security=True;");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Many-to-Many

            // UserFriends (Task 2)
            builder
                .Entity<UserFriend>()
                .HasKey(uf => new { uf.UserId, uf.FriendId });

            builder
                .Entity<UserFriend>()
                .HasOne(uf => uf.User)
                .WithMany(f => f.FriendshipsMade)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserFriend>()
                .HasOne(uf => uf.Friend)
                .WithMany(f => f.FriendshipsAccepted)
                .HasForeignKey(uf => uf.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            // PictureAlbums (Task 3)
            builder
                .Entity<PictureAlbum>()
                .HasKey(pa => new { pa.PictureId, pa.AlbumId });

            builder
                .Entity<Picture>()
                .HasMany(p => p.Albums)
                .WithOne(pa => pa.Picture)
                .HasForeignKey(pa => pa.PictureId);

            builder
                .Entity<Album>()
                .HasMany(a => a.Pictures)
                .WithOne(pa => pa.Album)
                .HasForeignKey(pa => pa.AlbumId);

            // AlbumsTags (Task 4)
            builder
                .Entity<AlbumTag>()
                .HasKey(at => new { at.AlbumId, at.TagId });

            builder
                .Entity<Album>()
                .HasMany(a => a.Tags)
                .WithOne(at => at.Album)
                .HasForeignKey(at => at.AlbumId);

            builder
                .Entity<Tag>()
                .HasMany(t => t.Albums)
                .WithOne(at => at.Tag)
                .HasForeignKey(at => at.TagId);

            // UserAlbum (Task 5)
            builder
                .Entity<UserAlbum>()
                .HasKey(pa => new { pa.UserId, pa.AlbumId });

            builder
                .Entity<User>()
                .HasMany(u => u.SharedAlbums)
                .WithOne(ua => ua.User)
                .HasForeignKey(ua => ua.UserId);

            builder
                .Entity<Album>()
                .HasMany(a => a.SharedAlbums)
                .WithOne(ua => ua.Album)
                .HasForeignKey(ua => ua.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many

            // Albums (Task 3)
            // Replaced by UserAlbum (Task 5)
            // User navigation properties in Albums are not deleted on migration to SharesAlbums (UserAlbums)
            // Existing data in Albums is transfered to SharedAlbums
            builder
                .Entity<Album>()
                .HasOne(a => a.User)
                .WithMany(u => u.Albums)
                .HasForeignKey(a => a.UserId);

            base.OnModelCreating(builder);
        }
    }
}
