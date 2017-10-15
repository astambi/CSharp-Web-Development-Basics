namespace WebServer.Application.Views
{
    using Server.Contracts;

    public class IndexView : IView
    {
        public string View() => "<h1>Welcome!</h1>";
    }
}