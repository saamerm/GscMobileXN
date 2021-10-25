using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.ViewModels;
using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
	public class ClaimTreatmentDetailsEntryMIView : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener
	{
		ClaimTreatmentDetailsEntryMIViewModel _model;
		int btntag;
		bool firsttimeDateReferal;
		bool dialogShown;
		bool dateReferalVisible;
		private View _view;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);
			_view =  this.BindingInflate(Resource.Layout.ClaimTreatmentDetailsEntryMIView, null);
			return _view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (ClaimTreatmentDetailsEntryMIViewModel)ViewModel;
			_model.ClaimTreatmentEntrySuccess += HandleClaimTreatmentEntrySuccess;

			Button submitEntry = _view.FindViewById<Button>(Resource.Id.claimSubmitClaimBtn);
			submitEntry.Click += (newSender, newE) => 
			{
				_model.SubmitEntryCommand.Execute(null);
			};
			Button saveEntry = _view.FindViewById<Button>(Resource.Id.claimSaveClaimBtn);
			saveEntry.Click += (newSender, newE) => 
			{
				_model.SaveEntryCommand.Execute(null);
			};

			TextView PickupDate = _view.FindViewById<TextView> (Resource.Id.PickupDate);
			PickupDate.Click += (sender, args) =>
			{
				btntag=0;
				var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
				frag.Show(FragmentManager, null);

			};


			Button claimreferraldate = _view.FindViewById<Button> (Resource.Id.claimreferraldate);
			claimreferraldate.Click += (sender, args) =>
			{
				btntag=1;
				var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
				frag.Show(FragmentManager, null);

			};

			//dateReferalVisible
			Button claimreferraldateclear = _view.FindViewById<Button> (Resource.Id.claimreferraldateclear);
			claimreferraldateclear.Click += (sender, args) =>
			{
				_model.DateOfReferral=DateTime.MinValue;
				claimreferraldate.SetTextColor (Resources.GetColor (Resource.Color.off_white));
				claimreferraldate.SetCompoundDrawablesWithIntrinsicBounds (0, 0, Android.Resource.Drawable.IcMenuToday, 0);
				dateReferalVisible = false;
				firsttimeDateReferal=false;
				claimreferraldateclear.Visibility = ViewStates.Gone;
			};

			if (_model.EditMode) {
				if (_model.TypeOfMedicalProfessional != null && _model.IsDateOfReferralSetByUser) {
					dateReferalVisible = true;
					firsttimeDateReferal = true;
				} else {
					dateReferalVisible = false;
					firsttimeDateReferal = false;
				}
			}
			if (dateReferalVisible) {
				claimreferraldate.SetCompoundDrawablesWithIntrinsicBounds (0, 0, 0, 0);
				claimreferraldateclear.Visibility = ViewStates.Visible;
			} else {
				claimreferraldateclear.Visibility = ViewStates.Gone;
			}

			if (!firsttimeDateReferal) {
				claimreferraldate.SetTextColor (Resources.GetColor (Resource.Color.off_white));

			}

			EditText hideKeyboardButton = _view.FindViewById<EditText>(Resource.Id.totalCharged);
			hideKeyboardButton.KeyPress += (sender, e) => {
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter) {
					InputMethodManager manager = (InputMethodManager) Activity.GetSystemService(Context.InputMethodService);
					manager.HideSoftInputFromWindow(hideKeyboardButton.WindowToken, 0);
					e.Handled = true;
					//_model.LoginForDroidCommand.Execute(null);
				}
			};

			EditText hideKeyboardButton1 = _view.FindViewById<EditText>(Resource.Id.alternateCarrier);
			hideKeyboardButton1.KeyPress += (sender, e) => {
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter) {
					InputMethodManager manager = (InputMethodManager) Activity.GetSystemService(Context.InputMethodService);
					manager.HideSoftInputFromWindow(hideKeyboardButton1.WindowToken, 0);
					e.Handled = true;
					//_model.LoginForDroidCommand.Execute(null);
				}
			};

			Resources res = Resources;

		}

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
			switch (btntag) {
			case 0:
				{
					_model.PickupDate=date;break;
				}
			case 1:
				{
				
					_model.DateOfReferral=date;
					if (!firsttimeDateReferal) {
						Button claimreferraldate = _view.FindViewById<Button> (Resource.Id.claimreferraldate);
						Button claimreferraldateclear = _view.FindViewById<Button> (Resource.Id.claimreferraldateclear);
						//_model.DateOfReferral=DateTime.Today;
						firsttimeDateReferal=true;
						claimreferraldate.SetTextColor(Resources.GetColor (Resource.Color.dark_grey));
						claimreferraldate.SetCompoundDrawablesWithIntrinsicBounds (0, 0, 0, 0);
						dateReferalVisible = true;
						claimreferraldateclear.Visibility = ViewStates.Visible;
					}
					break;
				}
			}



		}

		void HandleClaimTreatmentEntrySuccess (object sender, EventArgs e)
		{
			Activity.RunOnUiThread(() =>
				{
					Activity.FragmentManager.PopBackStack();
				    ((ActivityBase)Activity).CustomFragmentsBackStack.RemoveAt(((ActivityBase)Activity).CustomFragmentsBackStack.Count - 1);
                }
			);
		}
			
	}
}

