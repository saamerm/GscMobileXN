using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using MobileClaims.Core.ViewModels;
using Uri = Android.Net.Uri;
using Path = System.IO.Path;
using File = Java.IO.File;
using Environment = Android.OS.Environment;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using MobileClaims.Core.Messages;
using MobileClaims.Droid.Helpers;
using MvvmCross;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Views;

namespace MobileClaims.Droid
{
	[Region(Resource.Id.phone_main_region)]
	public class DrugLookupResultsView : BaseFragment, IMvxView
	{
		DrugLookupResultsViewModel _model;
		bool dialogShown;

        readonly string[] PermissionsStorage =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        };
        const int RequestStorageID = 888999;
        private IMvxMessenger _messenger;
        protected MvxSubscriptionToken _permissionsStorageGrantedToken;

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            return this.BindingInflate(Resource.Layout.DrugLookupResultsFragment, null);
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
			_model = (DrugLookupResultsViewModel)ViewModel;

            SubtitleTextView stv = view.FindViewById<SubtitleTextView>(Resource.Id.drugsNotesDescButton2Id);
            stv.Text = Resources.FormatterBrandKeywords(Resource.String.DrugsNotesDescButton2, new string[] { Resources.GetString(Resource.String.gsc) });


            ProgressDialog progressDialog = null;
			if (_model.Busy) {
				this.Activity.RunOnUiThread (() => {
					progressDialog = ProgressDialog.Show (this.Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
				});
			}

			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "Busy") {
					if (_model.Busy) {
						this.Activity.RunOnUiThread (() => {
							if (progressDialog == null) {
								progressDialog = ProgressDialog.Show (this.Activity, "", Resources.GetString(Resource.String.loadingIndicator), true);
							}
							else {
								if (!progressDialog.IsShowing) {
									progressDialog.Show();
								}
							}
						});
					}
					else {
						this.Activity.RunOnUiThread (() => {
							if (progressDialog != null && progressDialog.IsShowing)
								progressDialog.Dismiss ();
						});
					}
				}
				else if (e.PropertyName == "Drug") {
					_model.Drug = ((DrugLookupResultsViewModel)sender).Drug;
					this.View.Invalidate();
				}
				else if(e.PropertyName == "SendSpecialAuthRequested"){
					if (_model.SendSpecialAuthRequested == true) {
						getEmailAlert();
					}
				}
//				else if(e.PropertyName == "SpecialAuthError"){
//					specialAuthError();
//				}
			};

			_model.FetchSpecialAuthComplete += HandleFetchSpecialAuthComplete;
			_model.ErrorFetchingSpecialAuth += HandleErrorFetchingSpecialAuth;
			_model.OnEmailError += HandleOnEmailError;
			_model.OnEmailComplete += HandleonEmailComplete;

		}

//	[Activity (Label = "Drug Results")]			
//	public class DrugLookupResultsView : MvxFragmentActivity
//	{
//
//		DrugLookupResultsViewModel _model;
//		bool dialogShown;
//
//		protected override void OnCreate(Bundle bundle)
//		{
//			base.OnCreate(bundle);
//			SetContentView(Resource.Layout.DrugLookupResultsFragment);
//		}
//
//		protected override void OnViewModelSet ()
//		{
//			base.OnViewModelSet ();
//			_model = (DrugLookupResultsViewModel)ViewModel;
//			_model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
//				if(e.PropertyName == "SendSpecialAuthRequested"){
//					getEmailAlert();
//				}else if(e.PropertyName == "SpecialAuthError"){
//					specialAuthError();
//				}
//			};
//
//		}

		void HandleonEmailComplete(object s, EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
				                            {
					this.dialogShown = true;
					Android.Content.Res.Resources res = this.Resources;
					AlertDialog.Builder builder;
					builder = new AlertDialog.Builder(this.Activity);
					builder.SetTitle( string.Format(res.GetString(Resource.String.detailsProviderEmailCompleteTitle)));
					builder.SetMessage(string.Format(res.GetString(Resource.String.detailsProviderEmailCompleteDesc)));
					builder.SetCancelable(false);
					builder.SetPositiveButton("OK", delegate { dialogShown = false; });
					builder.Show();
				}
				);
			}
		}

		void HandleErrorFetchingSpecialAuth (object sender, System.EventArgs e) {
			if (!dialogShown) {
				this.Activity.RunOnUiThread (() => {
					this.dialogShown = true;
					Android.Content.Res.Resources res = this.Resources;
					AlertDialog.Builder builder;
					builder = new AlertDialog.Builder (this.Activity);
					builder.SetTitle (string.Format (res.GetString (Resource.String.error)));
					builder.SetMessage (string.Format(res.GetString(Resource.String.drugsDownloadError)));
					builder.SetCancelable (false);
					builder.SetPositiveButton ("OK", delegate {
						dialogShown = false;
					});
					builder.Show ();
				});
			}
		}

		void HandleFetchSpecialAuthComplete (object sender, System.EventArgs e)
		{
			this.Activity.RunOnUiThread(() =>
				{
                    if ((int)Build.VERSION.SdkInt < 23)
                    {
                        SavePDFAndDisplay();
                        return;
                    }

                    GetStoragePermissions();
				}
			);
		}

        private void SavePDFAndDisplay()
        {
            string filepath = _model.PathToSpecialAuthForm;
            string dirPath = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryDownloads).AbsolutePath;
            string sdFilePath = Path.Combine(dirPath, _model.Drug.SpecialAuthFormName);
            System.IO.File.Copy(filepath, sdFilePath, true);

            File file = new File(sdFilePath);
            Uri fileUri = Uri.FromFile(file);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                fileUri = FileProvider.GetUriForFile(Application.Context, Application.Context.PackageName + ".fileprovider", file);
            }
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(fileUri, "application/pdf");
            intent.SetFlags(ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission | ActivityFlags.NoHistory);
            StartActivity(intent);
        }

        private void GetStoragePermissions()
        {
            const string writePermission = Manifest.Permission.WriteExternalStorage;
            if (this.Activity.CheckSelfPermission(writePermission) == (int)Permission.Granted)
            {
                SavePDFAndDisplay();
                return;
            }

            _permissionsStorageGrantedToken = _messenger.Subscribe<PermissionsStorageGrantedMessage>((message) =>
            {
                _messenger.Unsubscribe<PermissionsStorageGrantedMessage>(_permissionsStorageGrantedToken);
                SavePDFAndDisplay();
            });
            this.Activity.RequestPermissions(PermissionsStorage, RequestStorageID);
        }

        private void getEmailAlert()
		{
			this.Activity.RunOnUiThread (() => {
					this.dialogShown = true;
					Android.Content.Res.Resources res = this.Resources;
					AlertDialog.Builder builder;
					builder = new AlertDialog.Builder (this.Activity);
					builder.SetTitle( string.Format(res.GetString(Resource.String.sendAuthForm)));
					EditText emailInput = new EditText(this.Activity);
					emailInput.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationEmailAddress;
					emailInput.SetSingleLine(true);
					builder.SetView(emailInput);
					builder.SetCancelable (true);
					builder.SetPositiveButton (string.Format(res.GetString(Resource.String.send)), delegate {
						_model.SpecialAuthEMail = emailInput.Text;
						_model.ExecuteSendSpecialAuthorizationCommand.Execute(null);
						dialogShown = false;
					});
					builder.SetNegativeButton(string.Format(res.GetString(Resource.String.cancel)), delegate {
						_model.SendSpecialAuthorizationDroidCommandCancel.Execute(null);
						dialogShown = false;
					});
					builder.Show ();
				}
			);
		}

		private void HandleOnEmailError(object sender, System.EventArgs e)
		{
			if (!dialogShown) {

				this.Activity.RunOnUiThread(() =>
					{
						this.dialogShown = true;
						Android.Content.Res.Resources res = this.Resources;
						AlertDialog.Builder builder;
						builder = new AlertDialog.Builder(this.Activity);
						builder.SetTitle(string.Format (res.GetString (Resource.String.error)));
						builder.SetMessage(string.Format(res.GetString (Resource.String.detailsProviderEmailErrorDesc)));
						builder.SetCancelable(false);
						builder.SetPositiveButton("OK", delegate { dialogShown = false; });
						builder.Show();
					}
				);
			}
		}
	}
}
