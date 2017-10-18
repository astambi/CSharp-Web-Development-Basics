namespace WebServer.GameStoreApplication.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Utilites;

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        [Email]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
