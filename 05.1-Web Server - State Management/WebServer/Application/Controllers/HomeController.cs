namespace WebServer.Application.Controllers
{
    using Server.Enums;
    using Server.Http;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System;
    //using Views.Home;
    using Views;

    public class HomeController
    {
        // GET /
        public IHttpResponse Index()
        {
            var response = new ViewResponse(HttpStatusCode.Ok, new IndexView());

            response.Cookies.Add(new HttpCookie("lang", "en"));
            response.Cookies.Add("test", "test");

            return response;
        }

        // GET / testsession
        public IHttpResponse SessionTest(IHttpRequest req)
        {
            const string SessionDateKey = "Saved_Date";

            var session = req.Session;

            if (session.Get(SessionDateKey) == null)
            {
                session.Add(SessionDateKey, DateTime.UtcNow);
            }

            var response = new ViewResponse(
                HttpStatusCode.Ok,
                new SessionTestView(session.Get<DateTime>(SessionDateKey)));

            return response;
        }
    }
}