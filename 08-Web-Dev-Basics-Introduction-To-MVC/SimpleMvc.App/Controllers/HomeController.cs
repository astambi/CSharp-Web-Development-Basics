namespace SimpleMvc.App.Controllers
{
    using Framework.Attributes.Methods;
    using Framework.Contracts;
    using Framework.Controllers;
    using Models;

    public class HomeController : Controller
    {
        // GET /home/index?id=10
        [HttpGet]
        public IActionResult Index(int id)
        {
            return View();
        }

        // POST /home/index?id=10
        // BODY: Text=Pesho&Number=16
        [HttpPost]
        public IActionResult Index(int id, IndexModel model)
        {
            return View();
        }

              

    }
}
