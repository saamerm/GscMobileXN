using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Interfaces;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimSummaryView : BaseFragment<ClaimSummaryViewModel>
    {
        private ClaimSummaryViewModel _model;

        private View _view;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);
            _view = this.BindingInflate(Resource.Layout.ClaimSummaryLayout, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimSummaryViewModel)ViewModel;
            SetCombinedSizeOfFilesText();
        }

        private void SetCombinedSizeOfFilesText()
        {
            var combinedSizeOfFilesMustBeText = _view.FindViewById<TextView>(Resource.Id.combinedSizeOfFilesMustBeText);
            combinedSizeOfFilesMustBeText.Text = ViewModel.CombinedSizeOfFilesMustBe;
            var photoFile4MbLabelTextView = new SpannableString(ViewModel.TwentyFourMb);

            photoFile4MbLabelTextView.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(Context, Resource.Color.dark_red))), 0, photoFile4MbLabelTextView.Length(), 0);

            combinedSizeOfFilesMustBeText.Append(" ");
            combinedSizeOfFilesMustBeText.Append(photoFile4MbLabelTextView);
        }

        public bool BackPressHandled { get; set; }
        public void OnBackPressed()
        {
            BackPressHandled = true;
            _model.GoBackCommand.Execute(null);
        }
    }
}