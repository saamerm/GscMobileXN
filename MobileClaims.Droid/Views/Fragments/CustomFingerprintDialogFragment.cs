using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.Fingerprint.Dialog;

namespace MobileClaims.Droid.Views.Fragments
{
    public class CustomFingerprintDialogFragment : FingerprintDialogFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var fallback = view.FindViewById<Button>(Resource.Id.fingerprint_btnFallback);
            fallback.Visibility = ViewStates.Gone;
            return view;
        }
    }
}
