namespace WebServer.GameStoreApplication.ViewModels.Admin
{
    using Common;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AdminEditGameViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.Game.TitleMinLength,
           ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Game.TitleMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "YouTube Video URL")]
        [MinLength(
            ValidationConstants.Game.VideoLength,
           ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        [MaxLength(
            ValidationConstants.Game.VideoLength,
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        public string VideoId { get; set; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        // in GB
        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(
            ValidationConstants.Game.DescriptionMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }
    }
}
