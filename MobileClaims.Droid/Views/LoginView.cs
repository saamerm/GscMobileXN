using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Text.Method;
using Android.Views;
using Android.Widget;

using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using System;
using System.Threading;
using MvvmCross;
using Uri = Android.Net.Uri;

namespace MobileClaims.Droid.Views
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "", Icon = null, Theme = "@style/AppTheme",
        ConfigurationChanges = ConfigChanges.ScreenLayout | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden,
        WindowSoftInputMode = SoftInput.StateAlwaysHidden | SoftInput.AdjustResize)]
    public class LoginView : ActivityBase
    {
        private string _packageName;
        private LoginViewModel _model;
        private bool dialogShown;
        CancellationTokenSource token;

        public override void OnBackPressed()
        {
            var loginService = Mvx.IoCProvider.Resolve<ILoginService>();
            loginService.ShouldExit = false;
            // shouldn't be able to go back on login view
            Intent startMain = new Intent(Intent.ActionMain);
            startMain.AddCategory(Intent.CategoryHome);
            startMain.SetFlags(ActivityFlags.NewTask);
            StartActivity(startMain);
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            SetContentView(Resource.Layout.LoginView);

            _model = (LoginViewModel)ViewModel;
            
            SetPackageName();

            _model.UpdateRequired += OnUpdateRequired;
            _model.ShowBiometricLoginEvent += ShowBiometricEventHanlder;

            TextView tv = FindViewById<TextView>(Resource.Id.web_instructions_textview);
            tv.TextFormatted = GetTextFormatted(Resource.String.login_instruction);
            tv.MovementMethod = new LinkMovementMethod();

            CustomEditText userName = FindViewById<CustomEditText>(Resource.Id.user_name_edit_text);
            userName.Hint = _model.UserNamePlaceholderText;

            CustomEditText password = FindViewById<CustomEditText>(Resource.Id.editText2);
            password.Hint = _model.PasswordPlaceholderText;

            SetMessages();
        }

        private void SetMessages()
        {
            _model.LoginFailedMessage = _model.LoginErrorText;
            if (_model.UsingBiometricLogin)
            {
                _model.LoginFailedMessage = _model.PasswordChangedErrorFingerprint;
            }
        }

        private void ShowBiometricEventHanlder(object sender, EventArgs e)
        {
            if (!_model.ShouldExit && _model.BiometricLoginCommand.CanExecute() && _model.ShowTouchLogin)
            {
                _model.BiometricLoginCommand.Execute(token.Token);
            }
        }

        private void OnUpdateRequired(object sender, EventArgs e)
        {
            // Redirect to Google play store for the app.
            _model.ShowLoginFields = false;
            try
            {
                StartActivity(new Intent(Intent.ActionView, Uri.Parse($"market://details?id={_packageName}")));
            }
            catch (Exception ex)
            {
                StartActivity(new Intent(Intent.ActionView, Uri.Parse($"http://play.google.com/store/apps/details?id={_packageName}")));
            }
        }

        private void SetPackageName()
        {
            var appContext = Android.App.Application.Context.ApplicationContext;
            var packageInfo = appContext.PackageManager.GetPackageInfo(appContext.PackageName, 0);
            _packageName = packageInfo.PackageName;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.SetSoftInputMode(SoftInput.AdjustPan);

            // This is a temporary solution for CCQ which is compatible with rest of brands. Resolution problems should be solved by placing a vector logo format for each brand (not available now).
#if CCQ
            ImageView brandLogo = FindViewById<ImageView>(Resource.Id.logo_image);
            brandLogo.SetImageResource(Resource.Drawable.branded_logo);

            ViewGroup.MarginLayoutParams marginParams = (ViewGroup.MarginLayoutParams) brandLogo.LayoutParameters;
            marginParams.SetMargins(
                (int)(Resources.DisplayMetrics.Density * 64),
                (int)(Resources.DisplayMetrics.Density * 40),
                (int)(Resources.DisplayMetrics.Density * 64),
                0);
#endif
#if ENCON
            ImageView brandLogo = FindViewById<ImageView>(Resource.Id.logo_image);
            brandLogo.SetImageResource(Resource.Drawable.branded_logo);

            ViewGroup.MarginLayoutParams marginParams = (ViewGroup.MarginLayoutParams)brandLogo.LayoutParameters;
            marginParams.SetMargins(
                (int)(Resources.DisplayMetrics.Density * 96),
                (int)(Resources.DisplayMetrics.Density * 40),
                (int)(Resources.DisplayMetrics.Density * 96),
                0);
#endif
        }

        protected override void OnResume()
        {
            base.OnResume();
            token?.Dispose();
            token = new CancellationTokenSource();

            _model.isInBackground = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (token != null)
            {
                token.Cancel();
                token.Dispose();
                token = null;
            }

            _model.isInBackground = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Hide dialogs that may have been shown.
            if((_model.Dialogs != null))
            {
                _model.Dialogs.HideLoading();
            }
		}
    }
}