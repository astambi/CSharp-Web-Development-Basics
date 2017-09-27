namespace SocialNetwork.Utils
{
    using System;
    using System.Text.RegularExpressions;

    public static class TagTransformer
    {
        public static string Transform(string tag)
        {
            string validTag = new Regex(@"\s+").Replace(tag.Trim(), String.Empty);

            if (!validTag.StartsWith("#"))
            {
                validTag = "#" + validTag;
            }

            if (validTag.Length > 20)
            {
                validTag = validTag.Substring(0, 20);
            }

            return validTag;
        }
    }
}
