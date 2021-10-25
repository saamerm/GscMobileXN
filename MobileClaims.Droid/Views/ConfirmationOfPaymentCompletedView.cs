using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ConfirmationOfPaymentCompletedView : BaseFragment<ConfirmationOfPaymentCompletedViewModel>
    {
        private View _view;
        private TextView _uploadSuccessCompletedTextView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ConfirmationOfPaymentCompletedLayout, null);

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