using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using System;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
	public class AuditListViewFragment_ : BaseFragment
    {
        private AuditListViewModel _model;
	    private View _view;
	    private RecyclerView _auditListRecyclerView;
	    private AuditListAdapter _auditListAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
		    base.OnCreateView(inflater, container, savedInstanceState);
		    this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.AuditListView, null);

		    return _view;
		}

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var message = view.FindViewById(Resource.Id.noAuditMessage);
            message.Visibility = ViewStates.Gone;

            _model = (AuditListViewModel)ViewModel;

            SetupRecycler();
        }

	    private void SetupRecycler()
	    {
	        _auditListRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.auditListRecyclerView);
            _auditListRecyclerView.SetLayoutManager(Resources.GetBoolean(Resource.Boolean.isTablet)
                ? new GridLayoutManager(Activity, 2)
                : new LinearLayoutManager(Activity));
	        _auditListAdapter = new AuditListAdapter(_model);
            _auditListRecyclerView.SetAdapter(_auditListAdapter);
            _model.ClaimsFetched += OnAuditClaimsFetched;
	    }

	    private void OnAuditClaimsFetched(object sender, EventArgs e)
	    {
	        _auditListAdapter.NotifyDataSetChanged();
        }
	}
}