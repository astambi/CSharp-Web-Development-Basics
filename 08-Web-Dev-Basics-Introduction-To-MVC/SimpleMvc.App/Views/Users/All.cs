namespace SimpleMvc.App.Views.Users
{
    using SimpleMvc.Framework.Contracts.Generic;
    using System.Text;
    using ViewModels;

    public class All : IRenderable<AllUsernamesViewModel>
    {
        public AllUsernamesViewModel Model { get; set; }

        public string Render()
        {
            var builder = new StringBuilder();
            builder
                .AppendLine("<a href=\"/home/index\">Home</a>")
                .AppendLine("<h3>All users</h3>")
                .AppendLine("<ul>");

            foreach (var user in this.Model.UsernamesWithIds)
            {
                builder
                    .AppendLine($"<li><a href=\"/users/profile?id={user.UserId}\">{user.Username}</a></li>");
            }

            builder.AppendLine("</ul>");

            return builder.ToString();
        }
    }
}
