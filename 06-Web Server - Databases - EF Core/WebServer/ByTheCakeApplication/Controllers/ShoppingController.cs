namespace WebServer.ByTheCakeApplication.Controllers
{
    using Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Services;
    using Services.Contracts;
    using System;
    using System.Linq;
    using ViewModels;

    public class ShoppingController : Controller
    {
        private readonly IProductService productService;
        private readonly IShoppingService shoppingService;
        private readonly IUserService userService;

        public ShoppingController()
        {
            this.productService = new ProductService();
            this.shoppingService = new ShoppingService();
            this.userService = new UserService();
        }

        public IHttpResponse AddToCart(IHttpRequest req)
        {
            var id = int.Parse(req.UrlParameters["id"]);

            // Validate product
            var productExists = this.productService.Exists(id);

            if (!productExists)
            {
                return new NotFoundResponse();
            }

            // Add Product to Shopping cart
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.ProductIds.Add(id);

            // Redirect
            var redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (req.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={req.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest req)
        {
            var shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.ProductIds.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var productIds = shoppingCart.ProductIds;

                var productsInCart = this.productService.FindProductsInCart(productIds);

                var items = productsInCart
                            .Select(p => $"<div>{p.Name} - ${p.Price:F2}</div><br />");

                var totalPrice = productsInCart.Sum(p => p.Price);

                this.ViewData["cartItems"] = string.Join(string.Empty, items);
                this.ViewData["totalCost"] = $"{totalPrice:F2}";
            }

            return this.FileViewResponse(@"shopping\cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest req)
        {
            // Get User
            var username = req.Session
                .Get<string>(SessionStore.CurrentUserKey);

            var userId = this.userService.GetUserId(username);
            if (userId == null)
            {
                throw new InvalidOperationException($"User {username} does not exist");
            }

            // Get Products in Shopping cart
            var shoppingCart = req.Session
                .Get<ShoppingCart>(ShoppingCart.SessionKey);

            var productIds = shoppingCart.ProductIds;
            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            // Save Order in db
            this.shoppingService.CreateOrder(userId.Value, productIds);

            // Clear Shopping Cart
            shoppingCart.ProductIds.Clear();

            // View
            return this.FileViewResponse(@"shopping\finish-order");
        }
    }
}
