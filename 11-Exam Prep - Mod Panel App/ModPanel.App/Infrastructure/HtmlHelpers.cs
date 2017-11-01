namespace ModPanel.App.Infrastructure
{
    using Models.Admin;
    using Models.Log;
    using Models.Posts;

    public static class HtmlHelpers
    {
        public static string ToHtml(this AdminUserListingModel user)
            => $@"
                <tr>
                    <td>{user.Id}</td>
                    <td>{user.Email}</td>
                    <td>{user.Position.ToFriendlyNamePosition()}</td>
                    <td class=""text-right"">{user.Posts}</td>
                    <td>
                        {(user.IsApproved
                        ? string.Empty
                        : $@"<a class=""btn btn-primary"" href=""/admin/approve?id={user.Id}"">Approve</a>")}                            
                    </td>
                </tr>";

        public static string ToHtml(this LogListingModel log)
           => $@"
                <div class=""card border-{log.Type.ToViewClassName()} mb-1"">
                    <div class=""card-body"">
                        <p class=""card-text"">{log}</p>
                    </div>
                </div>";

        public static string ToHtml(this PostListingModel post)
            => $@"
                <tr>
                    <td>{post.Id}</td>
                    <td class=""text-capitalize"">{post.Title}</td>
                    <td>
                        <a href=""/admin/edit?id={post.Id}"" class=""btn btn-sm btn-warning"">Edit</a>
                        <a href=""/admin/delete?id={post.Id}"" class=""btn btn-sm btn-danger"">Delete</a>
                    </td>
                </tr>";
    }
}
