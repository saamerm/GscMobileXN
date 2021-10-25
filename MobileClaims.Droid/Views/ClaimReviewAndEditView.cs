using System;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels.HCSA;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.right_region, false, BackstackTypes.ADD)]
    public class ClaimReviewAndEditView : BaseFragment, ViewTreeObserver.IOnGlobalLayoutListener
    {
        ClaimReviewAndEditViewModel _model;
        SingleSelectMvxListView mlistView;
        View rootView;
        bool dialogShown;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.ClaimReviewAndEditView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimReviewAndEditViewModel)ViewModel;

            _model.OnInvalidClaim -= Model_OnInvalidClaim;
            _model.OnInvalidClaim += Model_OnInvalidClaim;

            _model.CloseReviewAndEditViewModel -= _model_CloseReviewAndEditViewModel;
            _model.CloseReviewAndEditViewModel += _model_CloseReviewAndEditViewModel;

            mlistView = Activity.FindViewById<SingleSelectMvxListView>(Resource.Id.claim_review_hcsa);
            TextView Claimparticipant = Activity.FindViewById<TextView>(Resource.Id.claim_treatment_details_title);
            Claimparticipant.Text = string.Format(Resources.GetString(Resource.String.claimDetailsNameTitle), _model.Participant.FullName);

            mlistView.Post(() =>
                {
                    if (mlistView.Count > 0)
                    {

                        Utility.setFullListViewHeightCH(mlistView);
                    }
                });


            Button addClaimBtn = Activity.FindViewById<Button>(Resource.Id.addClaimBtn);
            addClaimBtn.Click += (newSender, newE) =>
            {
                _model.AddCommand.Execute(null);
            };

            rootView = view;
        }

        private void _model_CloseReviewAndEditViewModel(object sender, EventArgs e)
        {
            Activity.RunOnUiThread(() =>
                {
                    Activity.FragmentManager.PopBackStack();
                });
        }

        private void Model_OnInvalidClaim(object sender, EventArgs e)
        {
            dialogShown = true;
            Resources res = Resources;
            AlertDialog.Builder builder;
            builder = new AlertDialog.Builder(Activity);
            builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
            builder.SetMessage(string.Format(res.GetString(Resource.String.claimSubmissionMissingTreatmentMsg)));
            builder.SetCancelable(true);
            builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate
            {
                dialogShown = false;
            });
            builder.Show();
        }

        public void OnGlobalLayout()
        {
            if (mlistView.Count > 0)
                Utility.setFullListViewHeightCH(mlistView);
            rootView.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            rootView.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }
    }
}