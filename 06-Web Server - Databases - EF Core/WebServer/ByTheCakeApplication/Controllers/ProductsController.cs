namespace WebServer.ByTheCakeApplication.Controllers
{
    using Data;
    using Infrastructure;
    using Server.Http.Contracts;
    using Services;
    using Services.Contracts;
    using System;
    using System.Linq;
    using ViewModels;
    using ViewModels.Products;

    public class ProductsController : Controller
    {
        private const string AddView = @"products\add";
        private const string SearchView = @"products\search";

        private readonly IProductService productService;
        private readonly CakesData cakesData;

        public ProductsController()
        {
            this.cakesData = new CakesData();
            this.productService = new ProductService();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(AddView);
        }

        public IHttpResponse Add(IHttpRequest req, AddProductViewModel model)
        {
            // Validate Model
            if (string.IsNullOrWhiteSpace(model.Name) ||
                string.IsNullOrWhiteSpace(model.ImageUrl) ||
                model.Name.Length < 3 || 
                model.Name.Length > 30 ||
                model.ImageUrl.Length < 3 || 
                model.ImageUrl.Length > 2000 ||
                model.Price < 0)
            {
                this.AddError("Invalid product details");
                this.ViewData["showResult"] = "none";

                return this.FileViewResponse(AddView);
            }

            // Create Product in db
            this.productService.Create(model.Name, model.Price, model.ImageUrl);

            // Update View
            this.ViewData["showResult"] = "block";
            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString("F2");
            this.ViewData["imageUrl"] = model.ImageUrl;

            return this.FileViewResponse(AddView);
        }

        public IHttpResponse Search(IHttpRequest req)
        {
            const string searchTermKey = "searchTerm";

            var urlParameters = req.UrlParameters;

            this.ViewData["results"] = string.Empty;
            this.ViewData["searchTerm"] = string.Empty;

            if (urlParameters.ContainsKey(searchTermKey))
            {
                var searchTerm = urlParameters[searchTermKey];

                this.ViewData["searchTerm"] = searchTerm;

                var savedCakesDivs = this.cakesData
                    .All()
                    .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(c => $@"<div>{c.Name} - ${c.Price:F2} <a href=""/shopping/add/{c.Id}?{searchTermKey}={searchTerm}"">Order</a></div>");

                var results = "No cakes found";

                if (savedCakesDivs.Any())
                {
                    results = string.Join(Environment.NewLine, savedCakesDivs);
                }

                this.ViewData["results"] = results;
            }
            else
            {
                this.ViewData["results"] = "Please, enter search term";
            }

            this.ViewData["showCart"] = "none";

            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.Orders.Any())
            {
                var totalProducts = shoppingCart.Orders.Count;
                var totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }

            return this.FileViewResponse(@"cakes\search");
        }
    }
}
