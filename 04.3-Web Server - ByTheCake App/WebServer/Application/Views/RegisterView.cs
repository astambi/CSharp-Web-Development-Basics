namespace WebServer.Application.Views
{
    using Server.Contracts;

    public class RegisterView : IView
    {
        public string View()
        {
            return
                "<body>" +
                "   <form method=\"POST\">" +
                "      Name</br>" +
                "      <input type=\"text\" name=\"name\" placeholder=\"Enter name [a-z]\" /><br/>" +
                "      <input type=\"submit\" />" +
                "   </form>" +
                "</body>";
        }
    }
}
