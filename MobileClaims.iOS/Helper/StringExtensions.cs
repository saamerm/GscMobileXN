using Foundation;

namespace MobileClaims.iOS.Views
{
    public static class Extension
    {
        //returns localization values
        public static string tr(this string translate)
        {
            return NSBundle.MainBundle.LocalizedString(translate, "", "");
        }
    }
}
