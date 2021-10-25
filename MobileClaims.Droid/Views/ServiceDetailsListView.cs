using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.left_region, BackstackBehaviour = BackstackTypes.ADD)]
    public class ServiceDetailsListView : BaseFragment<ServiceDetailsListViewModel>
    {
        private View _view;
        private RecyclerView _detailsRecyclerView;
        private DetailsListViewAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ServiceDetailsListLayout, null);

            SetupRecycler();

            return _view;
        }

        private void SetupRecycler()
        {
            _detailsRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.detailsRecyclerView);
            _layoutManager = new LinearLayoutManager(Context);
            _detailsRecyclerView.SetLayoutManager(_layoutManager);

            Func<HealthProviderSummaryModel, Task> selectServiceProvider = ViewModel.SelectServiceProvider;

            _adapter = new DetailsListViewAdapter(
                ViewModel.ViewModelParameter, 
                selectServiceProvider,
                ViewModel.ShowDetails, 
                ViewModel.ToggleFavouriteDetails);
            _detailsRecyclerView.SetAdapter(_adapter);
        }

        public override void OnResume()
        {
            base.OnResume();
            ViewModel.FavouriteProviderRefreshed += OnFavouriteProviderRefreshed;
            ViewModel.ServiceProvidersChanged += ViewModelOnServiceProvidersChanged;
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel.FavouriteProviderRefreshed -= OnFavouriteProviderRefreshed;
            ViewModel.ServiceProvidersChanged -= ViewModelOnServiceProvidersChanged;
        }

        private void OnFavouriteProviderRefreshed(object sender, int position)
        {
            _adapter.NotifyItemChanged(position);
        }

        private void ViewModelOnServiceProvidersChanged(object sender, EventArgs e)
        {
            _adapter.UpdateProviders(ViewModel.ViewModelParameter);
            _adapter.NotifyDataSetChanged();
        }
    }
}