using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimsHistoryResultsCountFragment_ : BaseFragment, ViewTreeObserver.IOnGlobalLayoutListener
    {
        private ClaimsHistoryResultsCountViewModel _model;
        private MvxListView _searchResultsSummaryList;
        private CustomAdapter _adapter;
        private FrameLayout _layout, _mainNavLayout;
        private View _rootView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            var _view = this.BindingInflate(Resource.Layout.ClaimsHistoryResultsCountView, null);
            _searchResultsSummaryList = _view.FindViewById<MvxListView>(Resource.Id.summary_claims_history);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimsHistoryResultsCountViewModel)ViewModel;
            _model.PropertyChanged -= _model_PropertyChanged;
            _model.PropertyChanged += _model_PropertyChanged;

            _mainNavLayout = Activity.FindViewById<FrameLayout>(Resource.Id.nav_region);
            _mainNavLayout.Visibility = ViewStates.Visible;

            _adapter = new CustomAdapter(Activity, (IMvxAndroidBindingContext)BindingContext, _model.SearchResultsSummary);
            _searchResultsSummaryList.Adapter = _adapter;

            view.Post(() =>
                {
                    if (_searchResultsSummaryList.Count > 0)
                        Utility.setFullListViewHeightCH(_searchResultsSummaryList);
                });
            _rootView = view;
        }


        public void OnGlobalLayout()
        {
            if (_searchResultsSummaryList.Count > 0)
                Utility.setFullListViewHeightCH(_searchResultsSummaryList);

            _rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }


        void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SearchResultsSummary")
            {
                _rootView.Post(() =>
                    {
                        _searchResultsSummaryList.ClearChoices();

                        if (_adapter.Count < _model.SearchResultsSummary.Count)
                        {
                            CustomAdapter _adapter = new CustomAdapter(Activity, (IMvxAndroidBindingContext)BindingContext, _model.SearchResultsSummary);
                            _searchResultsSummaryList.Adapter = _adapter;

                            if (_searchResultsSummaryList.Count > 0 && _searchResultsSummaryList.Adapter.ItemsSource != null)
                                Utility.setFullListViewHeightCH(_searchResultsSummaryList);
                        }
                        else
                            _adapter.NotifyDataSetChanged();

                    });

            }

            else if (e.PropertyName == "IsSearchResultsSummarySelected")
            {
                if (_model.IsSearchResultsSummarySelected && _layout != null)
                    _layout.Visibility = ViewStates.Gone;

            }
        }
    }

    public class CustomAdapter : MvxAdapter
    {
        Context ctx;
        ObservableCollection<ClaimHistorySearchResultSummary> SearchResultsSummary;


        public CustomAdapter(Context context, IMvxAndroidBindingContext bindingContext, ObservableCollection<ClaimHistorySearchResultSummary> data)
            : base(context, bindingContext)
        {
            ctx = context;
            SearchResultsSummary = data;
            ReloadOnAllItemsSourceSets = true;

        }

        public override int Count => SearchResultsSummary.Count;

        protected override View GetBindableView(View convertView, object source, ViewGroup parent, int templateId)
        {
            var item = source as ClaimHistorySearchResultSummary;

            if (item.SearchResultCount > 0)
                templateId = Resource.Layout.item_results_count;

            else if (item.SearchResultCount == 0)
                templateId = Resource.Layout.item_result_count_disabled;

            View vi = base.GetBindableView(convertView, source, parent, templateId);

            return vi;
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override bool IsEnabled(int position)
        {
            if (SearchResultsSummary[position].SearchResultCount <= 0)
            {
                return false;
            }

            return true;
        }
    }
}