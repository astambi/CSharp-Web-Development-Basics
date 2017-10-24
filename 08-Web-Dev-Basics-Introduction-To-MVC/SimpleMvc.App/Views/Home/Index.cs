namespace SimpleMvc.App.Views.Home
{
    using Framework.Contracts;
    using System.Text;

    public class Index : IRenderable
    {
        public string Render()
        {
            var builder = new StringBuilder();

            builder
                .AppendLine("<h3>NotesApp</h3>")
                .AppendLine("<a href=\"/users/all\">All Users</a>")
                .AppendLine("<a href=\"/users/register\">Register Users</a>");

            return builder.ToString();
        }
    }
}
