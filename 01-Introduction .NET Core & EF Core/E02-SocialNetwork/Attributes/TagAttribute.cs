namespace SocialNetwork.Attributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    // Task 4
    public class TagAttribute : ValidationAttribute
    {
        public override bool IsValid(object tagValue)
        {
            string tagName = (string)tagValue;

            if (!Regex.IsMatch(tagName, @"^#\S{1,19}$"))
            {
                return false;
            }

            return true;
        }
    }
}
