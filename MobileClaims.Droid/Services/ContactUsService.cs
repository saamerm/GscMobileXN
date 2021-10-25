using Android.Content;


using MobileClaims.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Services
{
    public class ContactUsService : IContactUsService
    {
        public void DisplayContactUsPage()
        {
            var activity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            var uri = Android.Net.Uri.Parse("https://sel.ccq.org/GestionSel/zjweb020/aide.aspx");
            var intent = new Intent(Intent.ActionView, uri);
            activity.StartActivity(intent);
        }
    }
}