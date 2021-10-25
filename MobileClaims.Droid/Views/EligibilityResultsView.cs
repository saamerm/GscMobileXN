using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
	[Region(Resource.Id.phone_main_region)]
	public class EligibilityResultsView : BaseFragment
	{
        private EligibilityResultsViewModel _model;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.EligibilityResultsView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (EligibilityResultsViewModel)ViewModel;
           
            _model.EligibilityResultNotes = Resources.FormatterBrandKeywords(Resource.String.checkeligibilityResultNotes1, new string[] {
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.phonenumber)
            });

#if FPPM
            _model.EligibilityResultNotes = Resources.FormatterBrandKeywords(Resource.String.checkeligibilityResultNotes1FPPM, new string[] {
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.greenshieldcanada),
                Resources.GetString(Resource.String.phonenumberFPPM)
            });
#endif

            NunitoTextView benefitSubmissionGSIDLabel = view.FindViewById<NunitoTextView>(Resource.Id.benefitSubmissionGSIDLabel);
            benefitSubmissionGSIDLabel.Text = Resources.FormatterBrandKeywords(Resource.String.benefitSubmissionGSIDLabel, new string[] { Resources.GetString(Resource.String.greenShield) });


            TextView termsTextView = Activity.FindViewById<TextView>(Resource.Id.notes_text);


            string currLang = Locale.Default.Language;

            //			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
            //				TextView serviceProviderSelectTextView = this.Activity.FindViewById<TextView> (Resource.Id.claim_provider_name);
            //				serviceProviderSelectTextView.Text = _model.ClaimSubmissionType.ID;
            //				_model.NavigateToClaimDetailsCommand.Execute (null);
            //			} else {
            //				TextView claimPlanParticipantTitle = this.Activity.FindViewById<TextView>(Resource.Id.claim_plan_participant_title);
            //				claimPlanParticipantTitle.Text = string.Format(Resources.GetString(Resource.String.claimPlanParticipantTitle), (_model.ClaimSubmissionType.ID).ToLower());
            //
            //			}

            if (_model.EligibilityCheckResults.Result != null)
			{
				String timezone = string.Empty;
				DateTime dtMin = DateTime.MinValue;
				if (_model.EligibilityCheckResults.Result.SubmissionDate != null)
				{
					//TimeZoneInfo tzi = System.TimeZoneInfo.Local.StandardName;
					timezone = TimeZoneInfo.Local.StandardName;//tzi.ToString();

				}
				TextView SubmissionDate = Activity.FindViewById<TextView> (Resource.Id.SubmissionDate);
				SubmissionDate.Text = string.Format (Resources.GetString (Resource.String.eligibilitySubmissionDate), _model.EligibilityCheckResults.Result.SubmissionDate,timezone);

			}

			if (_model.EligibilityCheckType != null)
			{
				TableLayout ChiroPhyMassContent = Activity.FindViewById<TableLayout>(Resource.Id.ChiroPhyMassContent);
				TableLayout EyeContent = Activity.FindViewById<TableLayout>(Resource.Id.EyeContent);
				TableLayout GlassContent = Activity.FindViewById<TableLayout>(Resource.Id.GlassContent);
				TableLayout ContactsContent = Activity.FindViewById<TableLayout>(Resource.Id.ContactsContent);

				LinearLayout plan_limitation_cpm = Activity.FindViewById<LinearLayout>(Resource.Id.plan_limitation_cpm);
				LinearLayout plan_limitation_egc = Activity.FindViewById<LinearLayout>(Resource.Id.plan_limitation_egc);

				plan_limitation_cpm.Visibility = ViewStates.Gone;
				plan_limitation_egc.Visibility = ViewStates.Visible;

				switch (_model.EligibilityCheckType.ID)
				{

				case "CHIRO":
				case "PHYSIO":
				case "MASSAGE":
					ChiroPhyMassContent.Visibility = ViewStates.Visible;
					EyeContent.Visibility = ViewStates.Gone;
					GlassContent.Visibility = ViewStates.Gone;
					ContactsContent.Visibility = ViewStates.Gone;

					plan_limitation_cpm.Visibility = ViewStates.Visible;
					plan_limitation_egc.Visibility = ViewStates.Gone;
					break;
				case "EYEEXAM":
					ChiroPhyMassContent.Visibility = ViewStates.Gone;
					EyeContent.Visibility = ViewStates.Visible;
					GlassContent.Visibility = ViewStates.Gone;
					ContactsContent.Visibility = ViewStates.Gone;
					break;
				case "GLASSES":
					ChiroPhyMassContent.Visibility = ViewStates.Gone;
					EyeContent.Visibility = ViewStates.Gone;
					GlassContent.Visibility = ViewStates.Visible;
					ContactsContent.Visibility = ViewStates.Gone;
					break;
				case "CONTACTS":
					ChiroPhyMassContent.Visibility = ViewStates.Gone;
					EyeContent.Visibility = ViewStates.Gone;
					GlassContent.Visibility = ViewStates.Gone;
					ContactsContent.Visibility = ViewStates.Visible;
					break;
				}
			}

            if (_model.PhoneBusy)
            {
                ProgressDialog progressDialog = null;
                Activity.RunOnUiThread(() =>
                {
                    progressDialog = ProgressDialog.Show(Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
                });

                _model.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "PhoneBusy")
                    {
                        if (!_model.PhoneBusy)
                        {
                            if (!_model.NoPhoneAlteration)
                            {
                                termsTextView.Text = termsTextView.Text.Replace(Resources.GetString(Resource.String.phonenumber), _model.PhoneNumber.Text);
                            }
                            progressDialog.Dismiss();
                        }
                    }
                };
            }
            else
            {
                if (!_model.NoPhoneAlteration)
                {
                    termsTextView.Text = termsTextView.Text.Replace(Resources.GetString(Resource.String.phonenumber), _model.PhoneNumber.Text);
                }
            }
        }
	}
}