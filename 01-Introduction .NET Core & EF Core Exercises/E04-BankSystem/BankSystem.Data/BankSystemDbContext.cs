namespace BankSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BankSystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<SavingsAccount> SavingsAccounts { get; set; }

        public DbSet<CheckingAccount> CheckingAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=BankSystemDb;Integrated Security=True;");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<SavingsAccount>()
                .HasOne(a => a.User)
                .WithMany(u => u.SavingsAccounts)
                .HasForeignKey(a => a.UserId);

            builder
                .Entity<CheckingAccount>()
                .HasOne(a => a.User)
                .WithMany(u => u.CheckingAccounts)
                .HasForeignKey(a => a.UserId);

            base.OnModelCreating(builder);
        }
    }
}
