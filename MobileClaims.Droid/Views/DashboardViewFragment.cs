using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class DashboardViewFragment_ : BaseFragment
    {
        private View _view;
        private DashboardViewModel _model;
        private RecyclerView _dashboardRecentClaimsRecyclerView;
        private RecyclerView.LayoutManager _layoutManager;
        private DashboardRecentClaimsAdapter _adapter;

        private Button _dashboardViewAllButton;
        private TextView _dashboardDentalTextview;
        private TextView _dashboardChiropractorTextview;
        private TextView _dashboardMassageTextview;
        private TextView _dashboardDrugsTextview;
        private TextView _dashboardHcsaTitleTextview;
        private TextView _dashboardPsaTitleTextview;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.DashboardLayout, null);

            Init();

            return _view;
        }

        private void Init()
        {
            var leagueFont = Typeface.CreateFromAsset(Context.Assets, "fonts/LeagueGothic.ttf");

            _dashboardViewAllButton = _view.FindViewById<Button>(Resource.Id.dashboardViewAllButton);
            _dashboardViewAllButton.SetTypeface(leagueFont, TypefaceStyle.Normal);

            _dashboardDentalTextview = _view.FindViewById<TextView>(Resource.Id.dashboard_dental_textview);
            _dashboardDentalTextview.SetTypeface(leagueFont, TypefaceStyle.Normal);

            _dashboardChiropractorTextview = _view.FindViewById<TextView>(Resource.Id.dashboard_chiropractor_textview);
            _dashboardChiropractorTextview.SetTypeface(leagueFont, TypefaceStyle.Normal);

            _dashboardMassageTextview = _view.FindViewById<TextView>(Resource.Id.dashboard_massage_textview);
            _dashboardMassageTextview.SetTypeface(leagueFont, TypefaceStyle.Normal);

            _dashboardDrugsTextview = _view.FindViewById<TextView>(Resource.Id.dashboard_drugs_textview);
            _dashboardDrugsTextview.SetTypeface(leagueFont, TypefaceStyle.Normal);

            _dashboardHcsaTitleTextview = _view.FindViewById<TextView>(Resource.Id.dashboard_hcsa_title_textview);
            _dashboardHcsaTitleTextview.SetTypeface(leagueFont, TypefaceStyle.Normal);

            _dashboardPsaTitleTextview = _view.FindViewById<TextView>(Resource.Id.dashboard_psa_title_textview);
            _dashboardPsaTitleTextview.SetTypeface(leagueFont, TypefaceStyle.Normal);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (DashboardViewModel)ViewModel;

            SetupRecycler();

            _model.ClaimsFetched += ClaimsFetched;
        }

        public override void OnResume()
        {
            base.OnResume();
            _model.RefreshShouldRibbonBeDisplayed();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            _model.ClaimsFetched -= ClaimsFetched;
        }

        private void ClaimsFetched(object sender, EventArgs e)
        {
            _adapter.NotifyDataSetChanged();
        }

        private void SetupRecycler()
        {
            _dashboardRecentClaimsRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.dashboardRecentClaimsRecyclerView);

            _layoutManager = Resources.GetBoolean(Resource.Boolean.isTablet) 
                ? new GridLayoutManager(Activity, 2)
                : new LinearLayoutManager(Activity);
            
            _dashboardRecentClaimsRecyclerView.SetLayoutManager(_layoutManager);

            _adapter = new DashboardRecentClaimsAdapter(_model);
            _dashboardRecentClaimsRecyclerView.SetAdapter(_adapter);

            _dashboardRecentClaimsRecyclerView.NestedScrollingEnabled = false;
        }
    }
}