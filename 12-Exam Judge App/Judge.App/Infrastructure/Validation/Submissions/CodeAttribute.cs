namespace Judge.App.Infrastructure.Validation.Submissions
{
    using SimpleMvc.Framework.Attributes.Validation;

    public class CodeAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var code = value as string;
            if (code == null)
            {
                return true;
            }

            return code.Length >= 3;
        }
    }
}
