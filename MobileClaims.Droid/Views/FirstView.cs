using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Platforms.Android.Views;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "View for FirstViewModel")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);

        }
    }
}