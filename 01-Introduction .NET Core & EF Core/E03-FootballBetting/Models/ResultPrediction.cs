namespace FootballBetting.Models
{
    using System.Collections.Generic;

    public class ResultPrediction
    {
        public int Id { get; set; }

        public PredictionType Prediction { get; set; }

        public ICollection<BetGame> BetGames { get; set; } = new List<BetGame>();
    }
}
