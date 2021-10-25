using Android.OS;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using Android.App;
using System;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views
{
	public class DrugLookupResultsFragment_ : BaseFragment
    {

		DrugLookupResultsViewModel _model;
		bool dialogShown;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			return this.BindingInflate(Resource.Layout.DrugLookupResultsFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);


			_model = (DrugLookupResultsViewModel)ViewModel;
			try{
			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName == "SendSpecialAuthRequested"){
					getEmailAlert();
				}else if(e.PropertyName == "SpecialAuthError"){
					specialAuthError();
				}
			};
			}catch(Exception ex){
				System.Console.Write (ex.StackTrace);
			}

		}

		private void getEmailAlert()
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread (() => {
					this.dialogShown = true;
					Android.Content.Res.Resources res = this.Resources;
					AlertDialog.Builder builder;
					builder = new AlertDialog.Builder (this.Activity);
					builder.SetTitle( string.Format(res.GetString(Resource.String.sendAuthForm)));
					EditText emailInput = new EditText(this.Activity);
					emailInput.SetSingleLine(true);
					builder.SetView(emailInput);
					builder.SetCancelable (true);

					builder.SetPositiveButton (string.Format(res.GetString(Resource.String.send)), delegate {
						_model.SpecialAuthEMail = emailInput.Text;
						_model.ExecuteSendSpecialAuthorizationCommand.Execute(null);
					});
					builder.SetNegativeButton(string.Format(res.GetString(Resource.String.cancel)), delegate {
						dialogShown = false;
					});
					builder.Show ();

				}
				);

			}
		}

		private void specialAuthError()
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle( string.Format("Missing text constant"));
						builder.SetMessage(string.Format(_model.SpecialAuthError));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}

		private void downloadSpecialAuth()
		{

		}
	}
}

