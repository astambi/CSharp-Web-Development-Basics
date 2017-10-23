namespace SimpleMvc.Framework.Helpers
{
    public static class ControllerHelpers
    {
        public static string GetControllerName(object controller)
            => controller 
                .GetType()
                .Name
                .Replace(MvcContext.Get.ControllersSuffix, string.Empty);

        // Assembly.ViewsFolder.Controller.Action, Assembly NB!
        // SimpleMvc.App.Views.Home.Index, SimpleMvc.App
        public static string GetFullQualifiedName(string controller, string action)
            => string.Format(
                            "{0}.{1}.{2}.{3}, {0}",
                            MvcContext.Get.AssemblyName,
                            MvcContext.Get.ViewsFolder,
                            controller,
                            action);
    }
}
