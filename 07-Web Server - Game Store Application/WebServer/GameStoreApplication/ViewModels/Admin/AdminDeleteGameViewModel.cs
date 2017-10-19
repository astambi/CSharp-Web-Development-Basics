namespace WebServer.GameStoreApplication.ViewModels.Admin
{
    using System;

    public class AdminDeleteGameViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public double Size { get; set; }

        public string VideoId { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
