using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;
using MobileClaims.Core.ViewModels;
using MobileClaims.Droid.Helpers;
using MobileClaims.Droid.Interfaces;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
	public class EligibilityCheckCFOView : BaseFragment
	{
        private EligibilityCheckCFOViewModel _model;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.EligibilityCheckCFOView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (EligibilityCheckCFOViewModel)ViewModel;

#if FPPM
            _model.ChangeFPPMNumberBenefitLabel = Resources.FormatterBrandKeywords(Resource.String.unablebenefitEligibilityLabelFPPM, new string[] { Resources.GetString(Resource.String.phonenumberFPPM) });
            _model.ChangeFPPMNumberResults = Resources.FormatterBrandKeywords(Resource.String.eligibilityResultsNotesFPPM, new string[] { Resources.GetString(Resource.String.greenshieldcanada), Resources.GetString(Resource.String.phonenumberFPPM) });
#else
            _model.ChangeFPPMNumberBenefitLabel = Resources.FormatterBrandKeywords(Resource.String.unablebenefitEligibilityLabel, new string[] { Resources.GetString(Resource.String.phonenumber) });
            _model.ChangeFPPMNumberResults = Resources.FormatterBrandKeywords(Resource.String.eligibilityResultsNotes, new string[] { Resources.GetString(Resource.String.greenshieldcanada), Resources.GetString(Resource.String.phonenumber) });
#endif

            TextView notesView = Activity.FindViewById<TextView> (Resource.Id.eligibilityResultsDRENotes);
			notesView.Visibility = ViewStates.Gone;

            NunitoTextView eligibilityResultsOrthoticNotes = view.FindViewById<NunitoTextView>(Resource.Id.eligibilityResultsOrthoticNotes);
            eligibilityResultsOrthoticNotes.Text = Resources.FormatterBrandKeywords(Resource.String.eligibilityResultsOrthoticNotes, new string[] { Resources.GetString(Resource.String.greenshieldcanada) });

            NunitoTextView eligibilityResultsDRENotes = view.FindViewById<NunitoTextView>(Resource.Id.eligibilityResultsDRENotes);
            eligibilityResultsDRENotes.Text = Resources.FormatterBrandKeywords(Resource.String.eligibilityResultsDRENotes, new string[] { Resources.GetString(Resource.String.greenshieldcanada) });


            TextView termsTextView = Activity.FindViewById<TextView>(Resource.Id.notes_text);

            if (_model.EligibilityCheckResults.Result != null)
			{
				String timezone = string.Empty;
				DateTime dtMin = DateTime.MinValue;
				if (_model.EligibilityCheckResults.Result.SubmissionDate != null)
				{
					DateTime submitDate = DateTime.Parse(_model.EligibilityCheckResults.Result.SubmissionDate.ToString());
					if (submitDate.Year == dtMin.Year && submitDate.Month == dtMin.Month && submitDate.Day == dtMin.Day)
					{
						_model.EligibilityCheckResults.Result.SubmissionDate = DateTime.Now;
						TimeZoneInfo tzi = TimeZoneInfo.Utc;
						timezone = tzi.ToString();
					}
				}
				else
				{
					_model.EligibilityCheckResults.Result.SubmissionDate = DateTime.Now;
					TimeZoneInfo tzi = TimeZoneInfo.Utc;
					timezone = tzi.ToString();
				}
				TextView inquiryDate = Activity.FindViewById<TextView> (Resource.Id.inquiryDate);
				inquiryDate.Text = string.Format (Resources.GetString (Resource.String.eligibilityInquiryDate), _model.EligibilityCheckResults.Result.SubmissionDate,timezone);

            }
            string currLang = Locale.Default.Language;

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

        public bool BackPressHandled { get; set; }
        public void OnBackPressed()
        {
            BackPressHandled = true;
            _model.BackBtnClickCommandDroid.Execute();
        }
    }
}