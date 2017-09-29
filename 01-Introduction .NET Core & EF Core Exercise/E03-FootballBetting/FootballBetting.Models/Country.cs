namespace FootballBetting.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        [Key]
        [MinLength(3), MaxLength(3)]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Town> Towns { get; set; } = new List<Town>();

        public ICollection<CountryContinent> Continents { get; set; } = new List<CountryContinent>();
    }
}