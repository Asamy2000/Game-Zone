using static System.Net.Mime.MediaTypeNames;

namespace GameZone.Settings
{
    public static class FileSettings
    {
        public const string imgesPath = "/assets/images/games";
        public const string AllowedExtentions = ".jpg,.jpeg,.png";
        public const int MaxFileSizeInMB = 1;
        public const int MaxFileSizeInByte = MaxFileSizeInMB * 1024 * 1024;

    }
}
