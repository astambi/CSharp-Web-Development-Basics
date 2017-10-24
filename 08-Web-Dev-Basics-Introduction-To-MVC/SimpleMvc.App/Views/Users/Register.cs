namespace SimpleMvc.App.Views.Users
{
    using SimpleMvc.Framework.Contracts;
    using System.Text;

    public class Register : IRenderable
    {
        public string Render()
        {
            var builder = new StringBuilder();
            builder
                .AppendLine("<a href=\"/home/index\">Home</a>")
                .AppendLine("<h3>Register new user</h3>")
                .AppendLine("<form action=\"register\" method=\"post\">")
                    .AppendLine("Username: <input type=\"text\" name=\"Username\"/>")
                    .AppendLine("</br>")
                    .AppendLine("Password: <input type=\"password\" name=\"Password\"/>")
                    .AppendLine("</br>")
                    .AppendLine("<input type=\"submit\" value=\"Register\"/>")
                .AppendLine("</form>");

            return builder.ToString();
        }
    }
}
