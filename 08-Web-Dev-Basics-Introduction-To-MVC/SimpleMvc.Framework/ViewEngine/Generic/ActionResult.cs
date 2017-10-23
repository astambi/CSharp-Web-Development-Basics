namespace SimpleMvc.Framework.ViewEngine.Generic
{
    using Contracts.Generic;
    using System;

    public class ActionResult<TModel> : IActionResult<TModel>
    {
        public ActionResult(string viewFullQualifiedName, TModel model)
        {
            // Best practice
            //this.Action = Activator
            //             .CreateInstance(Type.GetType(viewFullQualifiedName))
            //             as IRenderable<TModel>;

            //if (this.Action == null)
            //{
            //    throw new InvalidOperationException("The given view does not implement IRenderable<TModel>.");
            //}

            this.Action = (IRenderable<TModel>)Activator
                          .CreateInstance(Type.GetType(viewFullQualifiedName));

            this.Action.Model = model;
        }

        public IRenderable<TModel> Action { get; set; }

        public string Invoke() => this.Action.Render();
    }
}
