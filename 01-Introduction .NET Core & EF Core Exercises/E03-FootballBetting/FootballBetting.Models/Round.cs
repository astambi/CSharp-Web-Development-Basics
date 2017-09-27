namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Round
    {
        public int Id { get; set; }

        [Required]
        public RoundType Name { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
