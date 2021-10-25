using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Views;

namespace MobileClaims.Droid
{
    [Region(Resource.Id.phone_main_region)]
    public class ClaimSubmissionConfirmationFragment_ : BaseFragment, IMvxView
    {
        private ClaimSubmissionConfirmationViewModel _model;
        private bool dialogShown;
        
        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater,
            Android.Views.ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.ClaimSubmissionConfirmationView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _model = (ClaimSubmissionConfirmationViewModel) ViewModel;
            try
            {
                TextView claimProviderAddress = this.Activity.FindViewById<TextView>(Resource.Id.claimProviderAddress);
                claimProviderAddress.Text = _model.Claim.Provider.DoctorName + ' ' +
                                            _model.Claim.Provider.FormattedAddress + ' ' +
                                            Resources.GetString(Resource.String.phone) + _model.Claim.Provider.Phone;

                NunitoTextView atv =
                    view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_CoverageWithGSCId);
                atv.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_CoverageWithGSC,
                    new string[] {Resources.GetString(Resource.String.gsc)});

                NunitoTextView atvUnpaid =
                    view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_UnpaidGSCPlanId);
                atvUnpaid.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_UnpaidGSCPlan,
                    new string[] {Resources.GetString(Resource.String.gsc)});

                NunitoTextView atvNumber =
                    view.FindViewById<NunitoTextView>(Resource.Id.claimOtherBenefitsView_OtherGSCNumberId);
                atvNumber.Text = Resources.FormatterBrandKeywords(Resource.String.claimOtherBenefitsView_OtherGSCNumber_confirmation,
                    new string[] {Resources.GetString(Resource.String.gsc)});

                ImageButton errorButtonParticipantList =
                    view.FindViewById<ImageButton>(Resource.Id.errorButtonParticipantList);
                errorButtonParticipantList.Tag = Resources.FormatterBrandKeywords(
                    Resource.String.claimOtherGscPlanParticipantListError,
                    new string[] {Resources.GetString(Resource.String.gsc)});

                NunitoTextView claimTermText = view.FindViewById<NunitoTextView>(Resource.Id.claim_term_text);
                claimTermText.Text = Resources.FormatterBrandKeywords(Resource.String.claimSubmissionTOS,
                    new string[]
                    {
                        Resources.GetString(Resource.String.greenshieldcanada),
                        Resources.GetString(Resource.String.greenshieldcanada)
                    });


                _model.OnNoMatchedDependent += NoMatchedDependent;
                _model.OnMultipleMatch += MultipleMatch;
                _model.OnInvalidSecondaryPlanNumber += InvalidSecondaryPlanNumber;
                _model.OnInvalidOnlineClaim += InvalidOnlineClaim;
                _model.OnInvalidGSCNumber += InvalidGSCNumber;
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.StackTrace);
            }
        }

        void NoMatchedDependent(object s, EventArgs e)
        {
            if (!dialogShown)
            {
                this.Activity.RunOnUiThread(() =>
                    {
                        this.dialogShown = true;
                        Android.Content.Res.Resources res = this.Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this.Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.FormatterBrandKeywords(
                            Resource.String.claimNoMatchedDependent,
                            new string[] {Resources.GetString(Resource.String.gsc)})));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void MultipleMatch(object s, EventArgs e)
        {
            if (!dialogShown)
            {
                this.Activity.RunOnUiThread(() =>
                    {
                        this.dialogShown = true;
                        Android.Content.Res.Resources res = this.Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this.Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.GetString(Resource.String.claimMultipleMatch)));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }

        void InvalidSecondaryPlanNumber(object s, EventArgs e)
        {
            if (!dialogShown)
            {
                this.Activity.RunOnUiThread(() =>
                    {
                        this.dialogShown = true;
                        Android.Content.Res.Resources res = this.Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this.Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(
                            string.Format(res.GetString(Resource.String.claimInvalidSecondaryPlanNumber)));
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
                this.Activity.RunOnUiThread(() =>
                    {
                        this.dialogShown = true;
                        Android.Content.Res.Resources res = this.Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this.Activity);
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
                this.Activity.RunOnUiThread(() =>
                    {
                        this.dialogShown = true;
                        Android.Content.Res.Resources res = this.Resources;
                        AlertDialog.Builder builder;
                        builder = new AlertDialog.Builder(this.Activity);
                        builder.SetTitle(string.Format(res.GetString(Resource.String.error)));
                        builder.SetMessage(string.Format(res.FormatterBrandKeywords(
                            Resource.String.claimInvalidGSCNumber,
                            new string[] {Resources.GetString(Resource.String.gsc)})));
                        builder.SetCancelable(false);
                        builder.SetPositiveButton(Resource.String.OK, delegate { dialogShown = false; });
                        builder.Show();
                    }
                );
            }
        }
    }
}