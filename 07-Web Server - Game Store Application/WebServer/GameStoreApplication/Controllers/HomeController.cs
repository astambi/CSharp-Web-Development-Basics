namespace WebServer.GameStoreApplication.Controllers
{
    using Server.Http.Contracts;

    public class HomeController : BaseController
    {
        public HomeController(IHttpRequest request) 
            : base(request)
        {
        }

        public IHttpResponse Index() => this.FileViewResponse(@"home\index");





    }
}
