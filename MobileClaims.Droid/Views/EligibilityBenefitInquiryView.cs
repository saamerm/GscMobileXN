using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Cirrious.MvvmCross.Views;
using MobileClaims.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace MobileClaims.Droid
{
	[Region(Resource.Id.right_region)]
	public class EligibilityBenefitInquiryView : BaseFragment, IMvxView
	{
		EligibilityBenefitInquiryViewModel _model;
		bool dialogShown;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.EligibilityBenefitInquiryView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (EligibilityBenefitInquiryViewModel)ViewModel;
			_model.OnInvalidEligibilityCheck += HandleInvalidEligibilityCheck;
			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "InquirySubmitted") {
					if (_model.InquirySubmitted) {
						getSuccessAlert();
					}
				}
			};

//			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
//				TextView serviceProviderSelectTextView = this.Activity.FindViewById<TextView> (Resource.Id.claim_provider_name);
//				serviceProviderSelectTextView.Text = _model.ClaimSubmissionType.ID;
//				_model.NavigateToClaimDetailsCommand.Execute (null);
//			} else {
//				TextView claimPlanParticipantTitle = this.Activity.FindViewById<TextView>(Resource.Id.claim_plan_participant_title);
//				claimPlanParticipantTitle.Text = string.Format(Resources.GetString(Resource.String.claimPlanParticipantTitle), (_model.ClaimSubmissionType.ID).ToLower());
//
//			}

		}

		void HandleInvalidEligibilityCheck (object sender, System.EventArgs e) {
			if (!dialogShown) {
				this.Activity.RunOnUiThread (() => {
					this.dialogShown = true;
					Android.Content.Res.Resources res = this.Resources;
					AlertDialog.Builder builder;
					builder = new AlertDialog.Builder (this.Activity);
					builder.SetTitle (string.Format (res.GetString (Resource.String.error)));
					builder.SetMessage (res.GetString(Resource.String.eligibilityInquiryErrorMessage));
					builder.SetCancelable (false);
					builder.SetPositiveButton ("OK", delegate {
						dialogShown = false;
					});
					builder.Show ();
				});
			}
		}

		private void getSuccessAlert()
		{
			this.Activity.RunOnUiThread (() => {
				this.dialogShown = true;
				Android.Content.Res.Resources res = this.Resources;
				AlertDialog.Builder builder;
				builder = new AlertDialog.Builder (this.Activity);
				builder.SetTitle("Success");
				builder.SetMessage("Enquiry submitted successfully");
				builder.SetCancelable (true);
				builder.SetPositiveButton (string.Format(res.GetString(Resource.String.OK)), delegate {
				});
				builder.Show ();
			}
			);
		}

	}
}

