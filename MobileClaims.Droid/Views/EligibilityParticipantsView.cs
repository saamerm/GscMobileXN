using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MobileClaims.Droid.Interfaces;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class EligibilityParticipantsView : BaseFragment
    {
        private EligibilityParticipantsViewModel _model;
        private bool dialogShown;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.EligibilityParticipantsView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (EligibilityParticipantsViewModel)ViewModel;

            if (Resources.GetBoolean(Resource.Boolean.isTablet))
            {
                _model.ChangeParticipantRequest += HandleChangeParticipantRequest;
            }
        }

        private void HandleChangeParticipantRequest(object sender, EventArgs e)
        {
            if (dialogShown)
            {
                return;
            }

            Activity.RunOnUiThread(() =>
                {
                    dialogShown = true;
                    Resources res = Resources;
                    AlertDialog.Builder builder;
                    builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle(string.Format(res.GetString(Resource.String.claimPlanParticipantChangedTitle)));
                    builder.SetMessage(string.Format(res.GetString(Resource.String.benefitPlanParticipantChangedContent), _model.RequestedParticipant.FullName));
                    builder.SetCancelable(false);
                    builder.SetNegativeButton(string.Format(res.GetString(Resource.String.noCancelBtn)), delegate
                    {
                        dialogShown = false;
                        int index = _model.ParticipantsViewModel.Participants.IndexOf(_model.SelectedParticipant);
                        if (index >= 0)
                        {
                            ListView participantList = Activity.FindViewById<ListView>(Resource.Id.claim_participant);
                            participantList.SetItemChecked(index, true);
                        }

                        if (!Resources.GetBoolean(Resource.Boolean.isTablet))
                        {
                            _model.ChangeParticipantCommand.Execute(_model.SelectedParticipant);
                        }
                    });
                    builder.SetPositiveButton(string.Format(res.GetString(Resource.String.yesContinueBtn)), delegate
                    {
                        dialogShown = false;
                        _model.ChangeParticipantCommand.Execute(_model.RequestedParticipant);
                    });
                    builder.Show();
                }
            );
        }

        public bool BackPressHandled { get; set; }
        public void OnBackPressed()
        {
            BackPressHandled = true;
            _model.BackBtnClickCommandDroid.Execute();
        }
    }
}