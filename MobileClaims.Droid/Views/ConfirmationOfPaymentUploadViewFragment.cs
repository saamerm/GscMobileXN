using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
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
    public class ConfirmationOfPaymentUploadViewFragment_ : BaseFragment
    {
        private View _view;
        private RecyclerView _filesRecyclerView;
        private AttachedFilesAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        private ConfirmationOfPaymentUploadViewModel _model;

        private TextView _copViewTitleTextView;
        private Button _addAnotherDocumentButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ConfirmationOfPaymentUploadFragmentLayout, null);

            return _view;
        }

        private void Init()
        {
            var nunitoBookFont = Typeface.CreateFromAsset(Activity.Assets, "fonts/NunitoSansRegular.ttf");

            _copViewTitleTextView = _view.FindViewById<TextView>(Resource.Id.copViewTitleTextView);
            _copViewTitleTextView.SetTextSize(ComplexUnitType.Sp, 24.0f);

            var _combinedSizeOfFilesMustBeText = _view.FindViewById<TextView>(Resource.Id.combinedSizeOfFilesMustBeText);
            _combinedSizeOfFilesMustBeText.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);

            _addAnotherDocumentButton = _view.FindViewById<Button>(Resource.Id.addAnotherDocumentButton);
            _addAnotherDocumentButton.SetTypeface(nunitoBookFont, TypefaceStyle.Normal);
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

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ConfirmationOfPaymentUploadViewModel)ViewModel;
            Init();

            SetupListView();

            SetCombinedSizeOfFilesText();

            _model.AttachmentsAdded += ViewModel_AttachmentsAdded;
        }

        private void ViewModel_AttachmentsAdded(object sender, System.EventArgs e)
        {
            _adapter.NotifyDataSetChanged();
        }

        private void SetupListView()
        {
            _filesRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.filesRecyclerView);
            _layoutManager = new LinearLayoutManager(Context);
            _filesRecyclerView.SetLayoutManager(_layoutManager);

            _adapter = new AttachedFilesAdapter(
                _model.Attachments, 
                pos => _model.DeleteCommand.Execute(pos)
                );
            _filesRecyclerView.SetAdapter(_adapter);
        }
    }
}