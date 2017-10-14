namespace WebServer.ByTheCakeApplication.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Products;

    public class ProductService : IProductService
    {
        public void Create(string name, decimal price, string imageUrl)
        {
            using (var context = new ByTheCakeDbContext())
            {
                var product = new Product
                {
                    Name = name,
                    Price = price,
                    ImageUrl = imageUrl
                };

                context.Add(product);
                context.SaveChanges();
            }
        }

        public IEnumerable<ProductListingViewModel> All(string searchTerm = null)
        {
            using (var context = new ByTheCakeDbContext())
            {
                var resultsQuery = context.Products.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    resultsQuery = resultsQuery
                        .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                return resultsQuery
                       .Select(p => new ProductListingViewModel
                       {
                           Id = p.Id,
                           Name = p.Name,
                           Price = p.Price
                       })
                       .ToList();
            }
        }
    }
}
