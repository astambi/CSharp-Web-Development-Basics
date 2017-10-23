namespace SimpleMvc.Framework.Controllers
{
    using Contracts;
    using Contracts.Generic;
    using Helpers;
    using System.Runtime.CompilerServices;
    using ViewEngine;
    using ViewEngine.Generic;

    public abstract class Controller
    {
        protected IActionResult View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerHelpers.GetControllerName(this); // child controller
            var viewFullQualifiedName = ControllerHelpers.GetFullQualifiedName(controllerName, caller);

            return new ActionResult(viewFullQualifiedName);
        }

        protected IActionResult View(string controller, string action)
        {
            var viewFullQualifiedName = ControllerHelpers.GetFullQualifiedName(controller, action);

            return new ActionResult(viewFullQualifiedName);
        }

        protected IActionResult<TModel> View<TModel>(TModel model, [CallerMemberName] string caller = "")
        {
            var controllerName = ControllerHelpers.GetControllerName(this);
            var viewFullQualifiedName = ControllerHelpers.GetFullQualifiedName(controllerName, caller);

            return new ActionResult<TModel>(viewFullQualifiedName, model);
        }

        protected IActionResult<TModel> View<TModel>(TModel model, string controller, string action)
        {
            var viewFullQualifiedName = ControllerHelpers.GetFullQualifiedName(controller, action);

            return new ActionResult<TModel>(viewFullQualifiedName, model);
        }
    }
}
