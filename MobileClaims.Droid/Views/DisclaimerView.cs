using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class DisclaimerView : BaseFragment
    {
        private View _view;
        private TextView _disclaimerLabel;
        private TextView _disclaimerFirstParagraphLabel;
        private TextView _disclaimerSecondParagraphLabel;
        private TextView _disclaimerThirdParagraphLabel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.DisclaimerLayout, null);

            Init();

            return _view;
        }

        private void Init()
        {
            var nunitoBookFont = Typeface.CreateFromAsset(Activity.Assets, "fonts/NunitoSansRegular.ttf");

            _disclaimerLabel = _view.FindViewById<TextView>(Resource.Id.disclaimerLabel);
            _disclaimerLabel.SetTypeface(nunitoBookFont, TypefaceStyle.Bold);

            _disclaimerFirstParagraphLabel = _view.FindViewById<TextView>(Resource.Id.disclaimerFirstParagraphLabel);
            _disclaimerFirstParagraphLabel.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);

            _disclaimerSecondParagraphLabel = _view.FindViewById<TextView>(Resource.Id.disclaimerSecondParagraphLabel);
            _disclaimerSecondParagraphLabel.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);

            _disclaimerThirdParagraphLabel = _view.FindViewById<TextView>(Resource.Id.disclaimerThirdParagraphLabel);
            _disclaimerThirdParagraphLabel.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);
        }
    }
}