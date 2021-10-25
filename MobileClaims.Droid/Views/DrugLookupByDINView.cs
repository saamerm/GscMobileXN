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
using MvvmCross.Platforms.Android.Binding.Views;
using MvvmCross.Views;


namespace MobileClaims.Droid
{
	[Region(Resource.Id.phone_main_region)]
	public class DrugLookupByDINView : BaseFragment, IMvxView
	{
		DrugLookupByDINViewModel _model;
		bool dialogShown;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			return this.BindingInflate(Resource.Layout.DrugLookupByDINFragment, null);
		}


		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			_model = (DrugLookupByDINViewModel)ViewModel;

			var list = Activity.FindViewById(Resource.Id.participant_list) as MvxListView;
			list.Adapter = new SingleSelectionMvxAdapter (this.Activity, (IMvxAndroidBindingContext)BindingContext);

			if (list.Count > 0) {
				Utility.setListViewHeightBasedOnChildren (list);
				list.SetItemChecked(0,true);
				_model.SelectedParticipant = _model.Participants [0];
			}

			var item = this.View.FindViewById(Resource.Id.button_search_din);
			if(item != null)
			{
				item.Click+=(object newSender, EventArgs newE) => 
				{
					_model.SearchAndNavigateCommand.Execute(null);
				};
			}
			ProgressDialog progressDialog = null;
			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "Searching") {
					if (_model.Searching) {
						this.Activity.RunOnUiThread (() => {
							progressDialog = ProgressDialog.Show (this.Activity, "", this.Activity.Resources.GetString(Resource.String.searchingIndicator), true);
						});
					} else {
						this.Activity.RunOnUiThread (() => {
							if (progressDialog != null && progressDialog.IsShowing)
								progressDialog.Dismiss ();
						});
					}
				}
				else if(e.PropertyName=="DIN" || e.PropertyName == "SelectedParticipant")
				{
                    var theButton = this.View.FindViewById(Resource.Id.button_search_din);
					if(_model.SelectedParticipant!=null && !string.IsNullOrEmpty(_model.DIN))
					{
						theButton.Enabled=true;
					}else{
						theButton.Enabled=false;
					}
                }else if(e.PropertyName == "ErrorInSearch" && _model.ErrorInSearch){
					ShowSearchError();
				}

			};
			_model.OnInvalidDIN += ShowInvalidDIN;
			_model.OnMissingDIN += ShowMissingDIN;


			EditText hideKeyboardButton = this.View.FindViewById<EditText>(Resource.Id.edit_text_drug_din);
			hideKeyboardButton.KeyPress += (object sender, View.KeyEventArgs e) => {
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter) {
					InputMethodManager manager = (InputMethodManager) this.Activity.GetSystemService(Context.InputMethodService);
					manager.HideSoftInputFromWindow(hideKeyboardButton.WindowToken, 0);
					e.Handled = true;
					//_model.LoginForDroidCommand.Execute(null);
				}
			};


			Button button_search_din = this.View.FindViewById<Button> (Resource.Id.button_search_din);
			button_search_din.Click+= (object sender, EventArgs e) => {
					InputMethodManager manager = (InputMethodManager) this.Activity.GetSystemService(Context.InputMethodService);
					manager.HideSoftInputFromWindow(hideKeyboardButton.WindowToken, 0);
			
			};

		}

		void ShowSearchError ()
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.noresults)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.noresultsdetails)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}
		void ShowInvalidDIN (object s, EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.noresults)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.dinFormatError)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}
		void ShowMissingDIN (object s, EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format(res.GetString(Resource.String.noresults)));
						builder.SetMessage(string.Format(res.GetString(Resource.String.dinError)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}

	}
}