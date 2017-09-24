namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Color
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Team> TeamPrimaryKits { get; set; } = new List<Team>();

        public ICollection<Team> TeamScondaryKits { get; set; } = new List<Team>();
    }
}
