using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MobileClaims.Droid.Interfaces;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimSubmissionConfirmationView : BaseFragment
    {
        private ClaimSubmissionConfirmationViewModel _model;
        bool dialogShown;

        private View _view;
        private RecyclerView _filesSubmitRecyclerView;
        private AttachedFilesSubmitAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            _view = this.BindingInflate(Resource.Layout.ClaimSubmissionConfirmationView, null);
            return _view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimSubmissionConfirmationViewModel)ViewModel;

            NunitoTextView atv = view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_CoverageWithGSCId);
            atv.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_CoverageWithGSC, new string[] { Resources.GetString(Resource.String.gsc) });

            NunitoTextView atvUnpaid = view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_UnpaidGSCPlanId);
            atvUnpaid.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_UnpaidGSCPlan, new string[] { Resources.GetString(Resource.String.gsc) });

            NunitoTextView atvNumber = view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_OtherGSCNumberId);
            atvNumber.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_OtherGSCNumber_confirmation, new string[] { Resources.GetString(Resource.String.gsc) });
            
            ImageButton errorButtonParticipantList = view.FindViewById<ImageButton>(Resource.Id.errorButtonParticipantList);
            errorButtonParticipantList.Tag = Resources.FormatterBrandKeywords(Resource.String.claimOtherGscPlanParticipantListError, new string[] { Resources.GetString(Resource.String.gsc) });

            NunitoTextView claimTermText = view.FindViewById<NunitoTextView>(Resource.Id.claim_term_text);
            claimTermText.Text = Resources.FormatterBrandKeywords(Resource.String.claimSubmissionTOS, new string[] { Resources.GetString(Resource.String.greenshieldcanada), Resources.GetString(Resource.String.greenshieldcanada) });

            NunitoTextView claimSubmissionGSIDLabelPhone = view.FindViewById<NunitoTextView>(Resource.Id.claimSubmissionGSIDLabelPhone);
            claimSubmissionGSIDLabelPhone.Text = Resources.FormatterBrandKeywords(Resource.String.claimSubmissionGSIDLabelPhone, new string[] { Resources.GetString(Resource.String.greenShield) });

            

            ProgressDialog progressDialog = null;
            if (_model.Busy)
            {
                Activity.RunOnUiThread(() =>
                {
                    progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                });
            }

            _model.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Busy")
                {
                    if (_model.Busy)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            if (progressDialog == null)
                            {
                                progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                            }
                            else
                            {
                                if (!progressDialog.IsShowing)
                                {
                                    progressDialog.Show();
                                }
                            }
                        });
                    }
                    else
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            if (progressDialog != null && progressDialog.IsShowing)
                                progressDialog.Dismiss();
                        });
                    }
                }
            };

            if (_model.Claim != null && _model.Claim.Provider != null)
            {
                TextView claimProviderAddress = Activity.FindViewById<TextView>(Resource.Id.claimProviderAddress);
                claimProviderAddress.Text = _model.Claim.Provider.DoctorName + ' ' + _model.Claim.Provider.FormattedAddress + ' ' + Resources.GetString(Resource.String.phone) + _model.Claim.Provider.Phone;
            }

            _model.OnNoMatchedDependent += NoMatchedDependent;
            _model.OnMultipleMatch += MultipleMatch;
            _model.OnInvalidSecondaryPlanNumber += InvalidSecondaryPlanNumber;
            _model.OnInvalidOnlineClaim += InvalidOnlineClaim;
            _model.OnInvalidGSCNumber += InvalidGSCNumber;
            _model.OnInvalidClaim += InvalidClaim;

            SetupRecycler();
            SetUpClaimCounter();
        }

        private void SetUpClaimCounter()
        {
            Resources res = Resources;
            MvxLinearLayout ItemTreatmentLinearLayout = View.FindViewById<MvxLinearLayout>(Resource.Id.linearLayoutItemTreatment2);
            int TotalTreatments = ItemTreatmentLinearLayout.ChildCount;
            for (int n = 0; n < TotalTreatments; n++)
            {                
                LinearLayout ItemLinearLayout = (LinearLayout)ItemTreatmentLinearLayout.GetChildAt(n);
                TableLayout ItemTableLayout = (TableLayout)ItemLinearLayout.GetChildAt(0);
                TableRow row = (TableRow)ItemTableLayout.GetChildAt(0);
                
                if (row != null)
                {
                    NunitoTextView rowCount = (NunitoTextView)row.GetChildAt(0);
                    int tempn = n + 1;
                    if (rowCount != null)
                    {
                        rowCount.Text = string.Format(res.GetString(Resource.String.claimCountOf), tempn, TotalTreatments);
                    }
                }
            }
        }

        void NoMatchedDependent(object s, EventArgs e)
        {


            if (!dialogShown)
            {

                Activity.RunOnUiThread(() =>
                    {
                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.FormatterBrandKeywords(Resource.String.claimNoMatchedDependent, new string[] { Resources.GetString(Resource.String.gsc) })));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void InvalidClaim(object s, EventArgs e)
        {
            //TODO 
        }
        void MultipleMatch(object s, EventArgs e)
        {
            ParticipantGSC emptyParticipant = new ParticipantGSC();
            emptyParticipant.ParticipantNumber = "";
            emptyParticipant.FirstName = "";
            emptyParticipant.LastName = "";

            List<ParticipantGSC> participantList = new List<ParticipantGSC>();
            List<ParticipantGSC> updatedParticipantList = new List<ParticipantGSC>();

            participantList = _model.Participants;

            updatedParticipantList.Add(emptyParticipant);
            foreach (ParticipantGSC tempParticipant in participantList)
            {
                if (tempParticipant.Equals(emptyParticipant))
                {
                }
                else
                {
                    updatedParticipantList.Add(tempParticipant);
                }
            }
            _model.Participants = updatedParticipantList;
        }

        void InvalidSecondaryPlanNumber(object s, EventArgs e)
        {
            if (!dialogShown)
            {

                Activity.RunOnUiThread(() =>
                    {

                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.GetString(Resource.String.claimInvalidSecondaryPlanNumber)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void InvalidOnlineClaim(object s, EventArgs e)
        {
            if (!dialogShown)
            {

                Activity.RunOnUiThread(() =>
                    {

                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.GetString(Resource.String.claimInvalidOnlineClaim)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void InvalidGSCNumber(object s, EventArgs e)
        {
            if (!dialogShown)
            {

                Activity.RunOnUiThread(() =>
                    {

                        dialogShown = true;
                        Resources res = Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.FormatterBrandKeywords(Resource.String.claimInvalidGSCNumber, new string[] { Resources.GetString(Resource.String.gsc) })));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        private void SetupRecycler()
        {
            _filesSubmitRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.filesSubmitRecyclerView);
            _layoutManager = new LinearLayoutManager(Context);

            _filesSubmitRecyclerView.SetLayoutManager(_layoutManager);
            _filesSubmitRecyclerView.NestedScrollingEnabled = false;

            _adapter = new AttachedFilesSubmitAdapter(_model.Attachments);
            _filesSubmitRecyclerView.SetAdapter(_adapter);
        }

    }
}