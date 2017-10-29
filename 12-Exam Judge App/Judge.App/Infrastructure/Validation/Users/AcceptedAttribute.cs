namespace Judge.App.Infrastructure.Validation.Users
{
    using SimpleMvc.Framework.Attributes.Validation;

    public class AcceptedAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var hasAccepted = value as string;
            if (hasAccepted == null)
            {
                return true;
            }

            return hasAccepted == "true";
        }
    }
}
