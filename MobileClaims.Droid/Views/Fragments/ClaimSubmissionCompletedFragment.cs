using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class ClaimSubmissionCompletedFragment_ : BaseFragment
    {
        private View _view;
        private TextView _uploadSuccessCompletedTextView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ClaimSubmissionCompletedLayout, null);

            Init();

            return _view;
        }

        private void Init()
        {
            var nunitoBookFont = Typeface.CreateFromAsset(Activity.Assets, "fonts/NunitoSansRegular.ttf");

            _uploadSuccessCompletedTextView = _view.FindViewById<TextView>(Resource.Id.uploadSuccessCompletedTextView);
            _uploadSuccessCompletedTextView.SetTypeface(nunitoBookFont, TypefaceStyle.Bold);
        }
    }
}