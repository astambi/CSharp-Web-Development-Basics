namespace WebServer.Infrastructure
{
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    public abstract class Controller
    {
        private const string DefaultPath = @"{0}\Resources\{1}.html";
        private const string ContentPlaceholder = "{{{content}}}";

        protected Controller()
        {
            this.ViewData = new Dictionary<string, string>
            {
                ["anonymousDisplay"] = "none",  // guest users
                ["authDisplay"] = "flex",       // show logout button // previous block
                ["showError"] = "none"          // hide form errors
            };
        }

        protected abstract string ApplicationDirectory { get; }

        protected IDictionary<string, string> ViewData { get; private set; }

        protected IHttpResponse FileViewResponse(string fileName)
        {
            var resultHtml = ProcessFileHtml(fileName);

            if (this.ViewData.Any())
            {
                foreach (var value in this.ViewData)
                {
                    resultHtml = resultHtml.Replace($"{{{{{{{value.Key}}}}}}}", value.Value);
                }
            }

            return new ViewResponse(HttpStatusCode.Ok, new FileView(resultHtml));
        }

        protected IHttpResponse RedirectResponse(string route)
        {
            return new RedirectResponse(route);
        }

        protected void AddError(string errorMessage)
        {
            this.ViewData["showError"] = "block";
            this.ViewData["error"] = errorMessage;
        }

        protected bool ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(model, context, results, true) == false)
            {
                foreach (var result in results)
                {
                    if (result != ValidationResult.Success)
                    {
                        this.AddError(result.ErrorMessage);

                        return false;
                    }
                }
            }

            return true;
        }

        private string ProcessFileHtml(string fileName)
        {
            var layoutHtml = File.ReadAllText(string.Format(
                            DefaultPath, this.ApplicationDirectory, "layout"));

            var fileHtml = File.ReadAllText(string.Format(
                            DefaultPath, this.ApplicationDirectory, fileName));

            return layoutHtml.Replace(ContentPlaceholder, fileHtml);
        }
    }
}
