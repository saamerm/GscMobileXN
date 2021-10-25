using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region)]
    public class ChooseClaimOrHistoryFragment_ : BaseFragment
    {
        private View _view;
        private ChooseClaimOrHistoryViewModel _model;
        private RecyclerView _activeClaimsRecyclerView;
        private ActiveClaimsAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.EnsureBindingContextIsSet(inflater);

            _view = this.BindingInflate(Resource.Layout.ChooseClaimOrHistoryFragment, null);

            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ChooseClaimOrHistoryViewModel)ViewModel;

            _model.ClaimsCollectionIsSet += ClaimsCollectionIsSet;
        }

        public override void OnResume()
        {
            base.OnResume();

            SetupRecycler();
        }

        private void ClaimsCollectionIsSet(object sender, EventArgs e)
        {
            SetupRecycler();
        }

        private void SetupRecycler()
        {
            _activeClaimsRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.activeClaimsRecyclerView);
            _layoutManager = new LinearLayoutManager(Context);
            _activeClaimsRecyclerView.SetLayoutManager(_layoutManager);

            _adapter = new ActiveClaimsAdapter(_model.COPClaims, _model);
            _activeClaimsRecyclerView.SetAdapter(_adapter);
        }
    }
}