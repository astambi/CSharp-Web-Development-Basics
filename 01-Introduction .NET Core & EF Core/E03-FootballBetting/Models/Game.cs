namespace FootballBetting.Models
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public int Id { get; set; }

        public int HomeTeamId { get; set; }

        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }

        public Team AwayTeam { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public DateTime GameDateTime { get; set; }

        public decimal HomeTeamWinBetRate { get; set; }

        public decimal AwayTeamWinBetRate { get; set; }

        public decimal DrawGameBetRate { get; set; }

        public int RoundId { get; set; }

        public Round Round { get; set; }

        public int CompetitionId { get; set; }

        public Competition Competition { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new LinkedList<PlayerStatistic>();

        public ICollection<BetGame> BetGames { get; set; } = new List<BetGame>();
    }
}