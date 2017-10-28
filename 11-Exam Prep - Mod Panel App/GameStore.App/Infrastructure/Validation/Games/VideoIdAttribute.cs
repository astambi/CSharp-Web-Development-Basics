namespace GameStore.App.Infrastructure.Validation.Games
{
    using SimpleMvc.Framework.Attributes.Validation;

    class VideoIdAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var videoId = value as string;
            if (videoId == null)
            {
                return true;
            }

            return videoId.Length == 11;
        }
    }
}
