using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ActiveClaimDetailViewFragment_ : BaseFragment
    {
        private View _view;
        private TextView _uploadDocumentsLabelActiveClaimTextView;
        private ActiveClaimDetailViewModel _model;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ActiveClaimDetailLayout, null);

            return _view;
        }

        private void Init()
        {
            _uploadDocumentsLabelActiveClaimTextView =
                _view.FindViewById<TextView>(Resource.Id.uploadDocumentsLabelActiveClaimTextView);

            _uploadDocumentsLabelActiveClaimTextView.SetTextSize(ComplexUnitType.Sp, 24.0f);

            SetCombinedSizeOfFilesText();
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ActiveClaimDetailViewModel)ViewModel;
            Init();
        }

        private void SetCombinedSizeOfFilesText()
        {
            TextView _combinedSizeOfFilesMustBeText = _view.FindViewById<TextView>(Resource.Id.combinedSizeOfFilesMustBeText);
            _combinedSizeOfFilesMustBeText.Text = _model.CombinedSizeOfFilesMustBe;
            var _photoFile4MBLabelTextView = new SpannableString(_model.TwentyFourMb);

            _photoFile4MBLabelTextView.SetSpan(new ForegroundColorSpan(new Color(ContextCompat.GetColor(Context, Resource.Color.dark_red))), 0, _photoFile4MBLabelTextView.Length(), 0);

            _combinedSizeOfFilesMustBeText.Append(" ");
            _combinedSizeOfFilesMustBeText.Append(_photoFile4MBLabelTextView);
        }
    }
}