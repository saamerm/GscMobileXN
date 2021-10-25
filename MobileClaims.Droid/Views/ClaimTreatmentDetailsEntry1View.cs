using Android.Content;
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
	public class ClaimTreatmentDetailsEntry1View : BaseFragment, Android.App.DatePickerDialog.IOnDateSetListener
	{
		ClaimTreatmentDetailsEntry1ViewModel _model;
		bool dialogShown;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ClaimTreatmentDetailsEntry1View, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (ClaimTreatmentDetailsEntry1ViewModel)ViewModel;
			_model.ClaimTreatmentEntrySuccess += HandleClaimTreatmentEntrySuccess;

			TextView DateOfTreatment = Activity.FindViewById<TextView> (Resource.Id.DateOfTreatment);
			DateOfTreatment.Click += (sender, args) =>
			{
				var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
				frag.Show(FragmentManager, null);

			};

			Button submitEntry = View.FindViewById<Button>(Resource.Id.claimSubmitClaimBtn);
			submitEntry.Click += (newSender, newE) => 
			{
				_model.SubmitEntryCommand.Execute(null);
			};
			Button saveEntry = View.FindViewById<Button>(Resource.Id.claimSaveClaimBtn);
			saveEntry.Click += (newSender, newE) => 
			{
				_model.SaveEntryCommand.Execute(null);
			};

			EditText hideKeyboardButton = View.FindViewById<EditText>(Resource.Id.treatmentTotal);
			hideKeyboardButton.KeyPress += (sender, e) => {
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter) {
					InputMethodManager manager = (InputMethodManager) Activity.GetSystemService(Context.InputMethodService);
					manager.HideSoftInputFromWindow(hideKeyboardButton.WindowToken, 0);
					e.Handled = true;
					//_model.LoginForDroidCommand.Execute(null);
				}
			};

			EditText hideKeyboardButton1 = View.FindViewById<EditText>(Resource.Id.alternateCarrier);
			hideKeyboardButton1.KeyPress += (sender, e) => {
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter) {
					InputMethodManager manager = (InputMethodManager) Activity.GetSystemService(Context.InputMethodService);
					manager.HideSoftInputFromWindow(hideKeyboardButton1.WindowToken, 0);
					e.Handled = true;
					//_model.LoginForDroidCommand.Execute(null);
				}
			};
		}

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
			_model.DateOfTreatment=date;

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