using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using Android.Views.InputMethods;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Views;

namespace MobileClaims.Droid
{
	[Region(Resource.Id.phone_main_region)]
	public class ClaimTreatmentDetailsListView : BaseFragment, IMvxView
	{
		ClaimTreatmentDetailsListViewModel _model;
		//ClaimSubmitTermsAndConditionsViewModel _model_1;
		SingleSelectMvxListView mlistView;

		bool dialogShown;
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ClaimTreatmentDetailsListView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (ClaimTreatmentDetailsListViewModel)ViewModel;
			//_model_1 = (ClaimSubmitTermsAndConditionsViewModel)this.ViewModel;

			if (_model.TreatmentDetails.Count == 0 && (!_model.HasNavigatedToFirstEntry)) {
				_model.HasNavigatedToFirstEntry = true;
				_model.AddTreatmentDetailCommand.Execute (null);
			} else {
				TextView claimTreatmentDetailsTitle = this.Activity.FindViewById<TextView>(Resource.Id.claim_treatment_details_title);
				claimTreatmentDetailsTitle.Text = string.Format(Resources.GetString(Resource.String.claimTreatmentDetailsTitle), _model.Participant.FullName);

				mlistView = this.Activity.FindViewById<SingleSelectMvxListView> (Resource.Id.claim_treatments);

				if (mlistView.Count > 0)
				{
					Utility.setFullListViewHeight(mlistView);
				}

				_model.OnTriggerRemoveWindowPopup += (object newSender, EventArgs newE) => {
					TriggerRemoveWindowPopup ();
				};
				_model.OnInvalidClaim += (object newSender, EventArgs newE) => {
					ShowInvalidClaim ();
				};
			}

			Button addClaimBtn = this.Activity.FindViewById<Button> (Resource.Id.addClaimBtn);
			addClaimBtn.Click+=(object newSender, EventArgs newE) => {
				_model.AddTreatmentDetailCommand.Execute(null);
			};

            /*if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {

				Button claimSubmitButtonTos = this.Activity.FindViewById<Button> (Resource.Id.claimSubmitClaim);

				claimSubmitButtonTos.Click += (object newSender, EventArgs newE) => {
					ShowClaimTOSTablet ();
				};
			} else {
				_model.SubmitClaimCommand.Execute (null);
			}*/



            InputMethodManager inputManager = 
				(InputMethodManager) Activity.GetSystemService(Context.InputMethodService); 
			inputManager.HideSoftInputFromWindow(  view.WindowToken, HideSoftInputFlags.NotAlways);
        }


//		[Export("removeCommandHandler")] // The value found in android:onClick attribute.
//		public void removeCommandHandler(View v) // Does not need to match value in above attribute.
//		{
//			int position = mlistView.GetPositionForView ((View)v.Parent);
//			_model.RemoveCommand.Execute (position);
//		}


		void ShowInvalidClaim ()
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.claimServiceProviderErrorMsgTitle)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.claimSubmissionMissingTreatmentMsg)));
						builder.SetCancelable(false);
						builder.SetPositiveButton(string.Format(res.GetString(Resource.String.OK)), delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}

		void TriggerRemoveWindowPopup ()
		{
			if (!dialogShown) {
				this.Activity.RunOnUiThread(() =>
				{
					this.dialogShown = true;
					Android.Content.Res.Resources res = this.Resources;
					AlertDialog.Builder builder;
					builder = new AlertDialog.Builder (this.Activity);
						builder.SetTitle(string.Format(res.GetString(Resource.String.claimTreatmentDeleteTitle)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.claimTreatmentDeleteMsg)));
					builder.SetCancelable (true);
						builder.SetPositiveButton (string.Format(res.GetString(Resource.String.OK)), delegate {
						//_model1.AcceptTermsAndConditionsCommand.Execute(null);
						_model.RemoveDroidCommand.Execute(null);
						dialogShown = false;
					});
					builder.SetNegativeButton(string.Format(res.GetString(Resource.String.cancel)), delegate {
						dialogShown = false;
					});
					builder.Create().Show();
				});
			}
		}
	}
}

