namespace Judge.App.Infrastructure
{
    using Models.Contests;
    using Models.Submissions;

    public static class HtmlHelpers
    {
        public static string ToHtml(this ContestListingModel model, bool isAdmin)
        {
            var html = $@"
                <tr>
                    <td scope=""row"">{model.Name}</td>
                    <td>{model.SubmissionsCount}</td>";

            if (isAdmin)
            {
                html += $@"
                    <td>
                        <a href=""/contests/edit?id={model.Id}"" class=""btn btn-sm btn-warning"">Edit</a>
                        <a href=""/contests/delete?id={model.Id}"" class=""btn btn-sm btn-danger"">Delete</a>
                    </td>
                </tr>";
            }
            else
            {
                html += "<td></td></tr>";
            }

            return html;
        }
        public static string ToHtml(this SubmissionListingModel model)
        {
            return $@"
                <a class=""list-group-item list-group-item-action list-group-item-dark"" data-toggle=""list"" href=""#{model.ContestId}"" role=""tab"">{model.Contest}</a>";
        }

        public static string ToHtml(this ContestDropDownModel model)
        {
            return $@"<option value=""{model.Id}"">{model.Name}</option>";
        }
    }
}
