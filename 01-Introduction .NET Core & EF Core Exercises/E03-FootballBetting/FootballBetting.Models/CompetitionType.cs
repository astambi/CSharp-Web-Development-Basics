namespace FootballBetting.Models
{
    using System.Collections.Generic;

    public class CompetitionType
    {
        public int Id { get; set; }

        public CompetitionTypeEnum Name { get; set; }

        public ICollection<Competition> Competitions { get; set; } = new List<Competition>();
    }
}
