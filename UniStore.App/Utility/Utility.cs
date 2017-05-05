namespace UniStore.App.Utility
{
    using System.IO;
    using Models;

    public static class Utility
    {
        public static string ImageAbsolutePath(string imageFileName)
        {
            return
                string.IsNullOrWhiteSpace(imageFileName)
                    ? Path.Combine("~", Constants.DefaultImage)
                    : Path.Combine("~", imageFileName);
        }
    }
}