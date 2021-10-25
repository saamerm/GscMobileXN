using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class LandingPageView : BaseFragment<LandingPageViewModel>
    {
        private View _view;
	    private RecyclerView _menuItemsRecyclerView;
	    private LinearLayoutManager _layoutManager;
	    private MenuItemsAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);
            
            return this.BindingInflate(Resource.Layout.LandingPageFragment, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _view = view;
            SetupRecycler();
        }
        
        private void SetupRecycler()
	    {
	        _menuItemsRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.menuItemsRecyclerView);
	        _layoutManager = new LinearLayoutManager(Activity);
	        _menuItemsRecyclerView.SetLayoutManager(_layoutManager);

	        _adapter = new MenuItemsAdapter(ViewModel);
	        _menuItemsRecyclerView.SetAdapter(_adapter);

	        _menuItemsRecyclerView.NestedScrollingEnabled = false;
        }
    }
}