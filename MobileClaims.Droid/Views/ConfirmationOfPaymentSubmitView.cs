using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ConfirmationOfPaymentSubmitView : BaseFragment<ConfirmationOfPaymentSubmitViewModel>
    {
        private View _view;
        private RecyclerView _filesSubmitRecyclerView;
        private AttachedFilesSubmitAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        private CheckBox _disclaimerCheckBox;

        private TextView _copSubmitViewTitleTextView;
        private TextView _additionalInformationSubmitTextView;
        private TextView _haveReadAndAcceptTextView;
        private TextView _disclaimerSubmitTextView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ConfirmationOfPaymentSubmitLayout, null);

            Activity.Window.SetSoftInputMode(SoftInput.AdjustResize);

            Init();

            return _view;
        }

        private void Init()
        {
            _disclaimerCheckBox = _view.FindViewById<CheckBox>(Resource.Id.disclaimerCheckBox);
            _disclaimerCheckBox.CheckedChange += DisclaimerCheckBoxOnCheckedChange;

            var nunitoBookFont = Typeface.CreateFromAsset(Activity.Assets, "fonts/NunitoSansRegular.ttf");

            _copSubmitViewTitleTextView = _view.FindViewById<TextView>(Resource.Id.copSubmitViewTitleTextView);
            _copSubmitViewTitleTextView.SetTextSize(ComplexUnitType.Sp, 24.0f);

            _additionalInformationSubmitTextView = _view.FindViewById<TextView>(Resource.Id.additionalInformationSubmitTextView);
            _additionalInformationSubmitTextView.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);

            _haveReadAndAcceptTextView = _view.FindViewById<TextView>(Resource.Id.haveReadAndAcceptTextView);
            _haveReadAndAcceptTextView.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);

            _disclaimerSubmitTextView = _view.FindViewById<TextView>(Resource.Id.disclaimerSubmitTextView);
            _disclaimerSubmitTextView.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);

            SetupRecycler();
        }

        private void DisclaimerCheckBoxOnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.IsDisclaimerChecked = e.IsChecked;
        }

        private void SetupRecycler()
        {
            _filesSubmitRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.filesSubmitRecyclerView);
            _layoutManager = new LinearLayoutManager(Context);
            _filesSubmitRecyclerView.SetLayoutManager(_layoutManager);

            _adapter = new AttachedFilesSubmitAdapter(ViewModel.Attachments);
            _filesSubmitRecyclerView.SetAdapter(_adapter);
        }
    }
}