namespace SimpleMvc.App.BindingModels
{
    using SimpleMvc.Framework.Attributes.Validation;

    public class RegisterUserBindingModel
    {
        public string Username { get; set; }

        [Regex(@"[^\s]{6,20}")]
        public string Password { get; set; }
    }
}
