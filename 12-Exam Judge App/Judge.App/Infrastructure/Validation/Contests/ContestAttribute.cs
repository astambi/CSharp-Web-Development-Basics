namespace Judge.App.Infrastructure.Validation.Contests
{
    using SimpleMvc.Framework.Attributes.Validation;

    public class ContestAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var name = value as string;
            if (name == null)
            {
                return true;
            }
            return name.Length >= 3 &&
                   name.Length <= 100 &&
                   char.IsUpper(name[0]);
        }
    }
}
