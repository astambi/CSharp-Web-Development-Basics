namespace WebServer.ByTheCakeApplication
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get(
                "/",
                req => new HomeController().Index());

            appRouteConfig.Get(
                "/about",
                req => new HomeController().About());

            appRouteConfig.Get(
                "/add",
                req => new CakesController().Add());

            appRouteConfig.Post(
                "/add",
                req => new CakesController().Add(
                    req.FormData["name"],
                    req.FormData["price"]));

            appRouteConfig.Get(
                "/search",
                req => new CakesController().Search(req));

            appRouteConfig.Get(
                "/calculator",
                req => new CalculatorController().Calculate());

            appRouteConfig.Post(
                "calculator",
                req => new CalculatorController().Calculate(
                    req.FormData["number1"],
                    req.FormData["number2"],
                    req.FormData["mathOperator"]));

            appRouteConfig.Get(
                "/login",
                req => new AccountController().Login());

            appRouteConfig.Post(
                "/login",
                req => new AccountController().Login(req));

            appRouteConfig.Post(
                "/logout",
                req => new AccountController().Logout(req));

            appRouteConfig.Get(
                "/shopping/add/{(?<id>[0-9]+)}",
                req => new ShoppingController().AddToCart(req));

            appRouteConfig.Get(
                "/cart",
                req => new ShoppingController().ShowCart(req));

            appRouteConfig.Post(
                "/shopping/finish-order",
                req => new ShoppingController().FinishOrder(req));
        }
    }
}
