using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using System;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region)]
    public class AuditClaimSummaryView : BaseFragment<AuditClaimSummaryViewModel>
    {
        private View _view;
        private RecyclerView _auditClaimsSummaryRecyclerView;
        private RecyclerView.LayoutManager _layoutManager;
        private AuditClaimSummaryAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.AuditClaimSummaryLayout, null);

            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            SetCombinedSizeOfFilesText();

            SetupRecyclerView();

            ViewModel.ClaimsFetched += ViewModelOnClaimsFetched;
        }

        private void SetupRecyclerView()
        {
            _auditClaimsSummaryRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.auditClaimsSummaryRecyclerView);
            _layoutManager = new LinearLayoutManager(Activity);
            _auditClaimsSummaryRecyclerView.SetLayoutManager(_layoutManager);
            _adapter = new AuditClaimSummaryAdapter(ViewModel.Claims);
            _auditClaimsSummaryRecyclerView.SetAdapter(_adapter);
        }

        private void ViewModelOnClaimsFetched(object sender, EventArgs e)
        {
            _adapter.NotifyDataSetChanged();
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
    }
}