using Android.OS;
using Android.Views;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.full_region, true, BackstackTypes.ADD)]
    public class ClaimsHistoryParticipantsView : BaseFragment
    {
        public ClaimsHistoryParticipantsViewModel _model;
        private MvxListView mlistView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.ClaimsHistoryParticipants, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimsHistoryParticipantsViewModel)ViewModel;

            _model.ParticipantSelectionComplete -= HandleParticipantSelectionComplete;
            _model.ParticipantSelectionComplete += HandleParticipantSelectionComplete;

            mlistView = Activity.FindViewById<MvxListView>(Resource.Id.claim_participant);

            if (mlistView.Count > 0)
            {
                Utility.setFullListViewHeight(mlistView);
            }
        }

        private void HandleParticipantSelectionComplete(object sender, EventArgs e)
        {
            Activity.RunOnUiThread(() =>
            {
                Activity.FragmentManager.PopBackStack();
                ((ClaimsHistoryResultsCountView)Activity).CustomFragmentsBackStack.RemoveAt(((ClaimsHistoryResultsCountView)Activity).CustomFragmentsBackStack.Count - 1);
            });
        }
    }
}