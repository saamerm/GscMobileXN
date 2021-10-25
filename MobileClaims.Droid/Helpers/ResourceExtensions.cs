using Android.Content.Res;

namespace MobileClaims.Droid.Helpers
{
    public static class ResourceExtensions
    {
        public static string FormatterBrandKeywords(this Resources resources, int resourceId, string[] table)
        {
            string result = resources.GetString(resourceId);
            return string.Format(result, table);
        }
    }
}