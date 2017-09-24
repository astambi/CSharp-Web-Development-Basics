namespace FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBookmakerSystemDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Continent> Continents { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<CompetitionType> CompetitionTypes { get; set; }

        public DbSet<BetGame> BetGames { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<ResultPrediction> ResultPredictions { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FootballBettingSystemDb;Integrated Security=True;");

            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Indices & PK
            builder
                .Entity<Country>()
                .HasIndex(c => c.Id)
                .IsUnique();

            builder
                .Entity<Position>()
                .HasIndex(p => p.Id)
                .IsUnique();

            // One-to-Many
            builder
                .Entity<Team>()
                .HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.TownId);

            builder
                .Entity<Team>()
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.TeamPrimaryKits)
                .HasForeignKey(c => c.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Team>()
                .HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.TeamScondaryKits)
                .HasForeignKey(c => c.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Town>()
                .HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(t => t.CountryId);

            builder
                .Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            builder
                .Entity<Player>()
                .HasOne(p => p.Position)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.PositionId);

            builder
                .Entity<Competition>()
                .HasOne(c => c.CompetitionType)
                .WithMany(ct => ct.Competitions)
                .HasForeignKey(c => c.CompetitionTypeId);

            builder
                .Entity<Bet>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bets)
                .HasForeignKey(b => b.UserId);

            builder
                .Entity<Game>()
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Game>()
                .HasOne(g => g.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Game>()
                .HasOne(g => g.Round)
                .WithMany(r => r.Games)
                .HasForeignKey(g => g.RoundId);

            builder
                .Entity<Game>()
                .HasOne(g => g.Competition)
                .WithMany(r => r.Games)
                .HasForeignKey(g => g.CompetitionId);

            //Many-to-Many
            builder
                .Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.GameId, ps.PlayerId });

            builder
                .Entity<PlayerStatistic>()
                .HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId);

            builder
                .Entity<PlayerStatistic>()
                .HasOne(ps => ps.Game)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);

            builder
                .Entity<BetGame>()
                .HasKey(bg => new { bg.GameId, bg.BetId });

            builder
                .Entity<BetGame>()
                .HasOne(bg => bg.Bet)
                .WithMany(b => b.BetGames)
                .HasForeignKey(bg => bg.BetId);

            builder
                .Entity<BetGame>()
                .HasOne(bg => bg.Game)
                .WithMany(g => g.BetGames)
                .HasForeignKey(bg => bg.BetId);

            builder
                .Entity<BetGame>()
                .HasOne(bg => bg.ResultPrediction)
                .WithMany(rp => rp.BetGames)
                .HasForeignKey(bg => bg.BetId);

            builder
                .Entity<CountryContinent>()
                .HasKey(cc => new { cc.CountryId, cc.ContinentId });

            builder
                .Entity<CountryContinent>()
                .HasOne(cc => cc.Country)
                .WithMany(c => c.Continents)
                .HasForeignKey(cc => cc.CountryId);

            builder
                .Entity<CountryContinent>()
                .HasOne(cc => cc.Continent)
                .WithMany(c => c.Countries)
                .HasForeignKey(cc => cc.ContinentId);

            base.OnModelCreating(builder);
        }
    }
}
