using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using DatePickerDialog;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using System;
using System.Linq;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace MobileClaims.Droid.Views
{
    [Region(Resource.Id.phone_main_region)]
	public class ClaimTreatmentDetailsEntryPGView : BaseFragment,Android.App.DatePickerDialog.IOnDateSetListener
	{
		ClaimTreatmentDetailsEntryPGViewModel _model;
		bool dialogShown;
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.ClaimTreatmentDetailsEntryPGView, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (ClaimTreatmentDetailsEntryPGViewModel)ViewModel;
			_model.ClaimTreatmentEntrySuccess += HandleClaimTreatmentEntrySuccess;

			if (!_model.EditMode) {
				if (_model.TypesOfEyewear.First ().ID != 0) {
					ClaimSubmissionBenefit empty = new ClaimSubmissionBenefit ();
					empty.ID = 0;
					empty.Name = "";
					empty.ProcedureCode = "";
					_model.TypesOfEyewear.Insert (0, empty);
				}
			}

			TextView DateOfPurchase = Activity.FindViewById<TextView> (Resource.Id.DateOfPurchase);
			DateOfPurchase.Click += (sender, args) =>
			{
				var frag = new DatePickerDialogFragment(Activity, DateTime.Now, this);
				frag.Show(FragmentManager, null);

			};

			Spinner typeOfEyewear = Activity.FindViewById<Spinner> (Resource.Id.typeofeyewear);
			typeOfEyewear.ItemSelected += spinner_ItemSelected;

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

			EditText hideKeyboardButton = View.FindViewById<EditText>(Resource.Id.feeamount);
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
			Resources res = Resources;
		}

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			var date = new DateTime(year, monthOfYear + 1, dayOfMonth);
			_model.DateOfPurchase=date;

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

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			ClaimSubmissionBenefit resultObj = _model.TypesOfEyewear[e.Position];;
			if (resultObj.ID == 0) {
			}
			else {
				if (_model.TypesOfEyewear.First ().ID == 0) {
					_model.TypesOfEyewear.RemoveAt (0);
				}
				_model.TypeOfEyewear = resultObj;
			}
		}
	
	}
}

