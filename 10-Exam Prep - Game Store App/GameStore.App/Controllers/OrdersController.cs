namespace GameStore.App.Controllers
{
    using Infrastructure;
    using Models.Orders;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;

    public class OrdersController : BaseController
    {
        private const string ShoppingCartSessionKey = "%^^Shopping_Cart%";
        private const string ShoppingCartPath = "/orders/cart";

        private readonly IGameService gameService;
        private readonly IOrderService orderService;

        public OrdersController(IGameService gameService,
                                IOrderService orderService)
        {
            this.gameService = gameService;
            this.orderService = orderService;
        }

        public IActionResult Buy(int id)
        {
            // Validate game
            if (!this.gameService.Exists(id))
            {
                this.NotFound();
            }

            this.Request.Session
                .GetShoppingCart()
                .AddGame(id);

            //this.ShowSuccess("You successfully added a game to the shopping cart");

            return this.Redirect(ShoppingCartPath);
        }

        public IActionResult Cart()
        {
            // Get orders from cart
            var shoppingCart = this.Request.Session.GetShoppingCart();

            var gameIds = shoppingCart.AllGames();

            var gamesToBuy = this.gameService
                .ByIds<GameListingOrdersModel>(gameIds);

            var gamesHtml = gamesToBuy
                .Select(g => g.ToHtml())
                .ToList();

            this.ViewModel["orders"] = string.Join(string.Empty, gamesHtml);
            this.ViewModel["total-price"] = gamesToBuy.Sum(g => g.Price)
                                            .ToString("F2");

            return this.View();
        }

        public IActionResult Remove(int id)
        {
            var shoppingCart = this.Request.Session.GetShoppingCart();
            shoppingCart.RemoveGame(id);

            return this.Redirect(ShoppingCartPath);
        }

        [HttpPost]
        public IActionResult Finish()
        {
            // Authorized users
            if (!this.User.IsAuthenticated)
            {
                return this.RedirectToLogin;
            }

            var shoppingCart = this.Request.Session.GetShoppingCart();
            var gameIds = shoppingCart.AllGames();

            this.orderService.Create(gameIds, this.Profile.Id);
            shoppingCart.Empty();

            return this.RedirectToHome;
        }
    }
}
