using System;
using System.Diagnostics;
using System.Globalization;
using CoreText;
using Foundation;
using LocalAuthentication;
using MobileClaims.Core;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Login
{
    public partial class LoginView : GSCBaseViewController
    {
        private string _webInstructionsLinkStr;
        private string _webInstructionsStr;
        private LoginViewModel _viewModel;
        private SFViewControllerDelegate _controllerDelegate;

        public LoginView()
            : base()
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            // Mandatory to have this here, as GSCBaseView is not used at this point to se the BNB height.
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                Constants.SetSafe((float)View.SafeAreaInsets.Bottom);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(true, false);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = (LoginViewModel)ViewModel;
            _viewModel.UpdateRequired += OnUpdateRequired;
            _viewModel.ShowBiometricLoginEvent += ShowBiometricEventHanlder;
            _viewModel.isInBackground = false;

            NavigationController.SetNavigationBarHidden(true, false);

            var isCurrentCultureEng = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en", StringComparison.OrdinalIgnoreCase);
            BrandLogoImageView.Image = isCurrentCultureEng ?
                UIImage.FromBundle("AppLogo-En") :
                UIImage.FromBundle("AppLogo-Fr");
#if CLAC
            if (Constants.IsPhone())
            {
                BrandLogoHeightConstraint.Constant = 60;
            }
#elif ARTA
            if (Constants.IsPhone())
            {
                BrandLogoHeightConstraint.Constant = 130;
            }
            else
            {
                BrandLogoHeightConstraint.Constant = 150;
            }
#endif

            PasswordTextField.SecureTextEntry = true;
            DetectBiometricCapability();

            BiometricLoginButton.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
            BiometricLoginButton.BackgroundColor = Colors.Clear;
            BiometricLoginButton.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.DETAILS_FONT_SIZE);

            CheckboxButton.SetBackgroundImage(UIImage.FromBundle("CheckboxNormal"), UIControlState.Normal);
            CheckboxButton.SetBackgroundImage(UIImage.FromBundle("CheckboxSelected"), UIControlState.Selected);
            CheckboxButton.AdjustsImageWhenHighlighted = true;
            CheckboxButton.TouchUpInside += CheckboxSelected;

            CheckboxLabel.Text = BrandResource.RememberMe;
            CheckboxLabel.TextColor = Colors.DARK_GREY_COLOR;
            CheckboxLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.SMALL_DETAILS_FONT_SIZE);

            UpdateAvailableLabel.TextColor = Colors.DARK_GREY_COLOR;
            UpdateAvailableLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.FONT_SIZE_13_15);

            ForceUpdateLoginLabel.TextColor = Colors.DARK_GREY_COLOR;
            ForceUpdateLoginLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.FONT_SIZE_13_15);

            InstructionLabel.Text = string.Format("{0} {1}.", BrandResource.WebInstructions, BrandResource.GeneralContent8.ToUpper());
            InstructionLabel.TextColor = Colors.DARK_GREY_COLOR;
            InstructionLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.TERMS_TEXT_FONT_SIZE);
            var myAttributedString = InstructionLabel.AttributedText;
            var myMutableString = new NSMutableAttributedString(myAttributedString);
            UIStringAttributes atSelection = new UIStringAttributes { ForegroundColor = Colors.HIGHLIGHT_COLOR };           
            myMutableString.AddAttributes(atSelection.Dictionary, new NSRange(BrandResource.WebInstructions.Length, BrandResource.GeneralContent8.Length + 2));            
            InstructionLabel.AttributedText = myMutableString;
            InstructionLabel.UserInteractionEnabled = true;
            var options = new UIApplicationOpenUrlOptions();
            options.OpenInPlace = true;
            var url = new Uri(BrandResource.WebInstructionsLink);
            InstructionLabel.AddGestureRecognizer(new UITapGestureRecognizer(() =>
                OpenPlanMemberOnlineServices(url)));
            MyIdCardLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            SetMyIdCardImageDimensions(isCurrentCultureEng);

            GreenDiagonalImageView.Image = UIImage.FromBundle("GreenDiagonal");
            SetBindings();
            SetMessages();
        }

        private void ShowBiometricEventHanlder(object sender, EventArgs e)
        {
            _viewModel.ShowBiometricLoginEvent -= ShowBiometricEventHanlder;
            if (_viewModel.BiometricLoginCommand.CanExecute() && !_viewModel.ShouldExit && _viewModel.LoggedInBefore)
            {
                _viewModel.BiometricLoginCommand.Execute();
            }
        }

        private void SetMessages()
        {
            _viewModel.LoginFailedMessage = _viewModel.LoginErrorText;
            if (_viewModel.UsingBiometricLogin)
            {
                var context = new LAContext();
                Version.TryParse(UIDevice.CurrentDevice.SystemVersion, out Version deviceVersionNumber);
                Version version = new Version(11, 0);

                if (Constants.IS_OS_VERSION_OR_LATER(11, 0) && context.BiometryType == LABiometryType.FaceId)
                {
                    _viewModel.LoginFailedMessage = _viewModel.PasswordChangedErrorFaceId;
                }
                else
                {
                    _viewModel.LoginFailedMessage = _viewModel.PasswordChangedErrorTouchId;
                }
            }
        }

        private void OnUpdateRequired(object sender, EventArgs e)
        {
            NSUrl appStoreLink = new NSUrl(Constants.AppStoreLink);
            UIApplication.SharedApplication.OpenUrl(appStoreLink, new NSDictionary { }, null);
            Process.GetCurrentProcess().Close();
        }

        private void SetBindings()
        {
            var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter();
            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<LoginView, LoginViewModel>();
            set.Bind(UpdateAvailableLabel).To(vm => vm.UpdateAvailableMessage2);
            set.Bind(ForceUpdateLoginLabel).To(vm => vm.ConnectionError);
            set.Bind(UserNameTextField).To(vm => vm.UserName);
            set.Bind(UserNameTextField).For(x => x.Placeholder).To(vm => vm.UserNamePlaceholderText);
            set.Bind(PasswordTextField).To(vm => vm.Password);
            set.Bind(PasswordTextField).For(x => x.Placeholder).To(vm => vm.PasswordPlaceholderText);
            set.Bind(LoginButton).To(vm => vm.LoginCommand);
            set.Bind(LoginButton).For("Title").To(vm => vm.LoginText);
            set.Bind(BiometricLoginButton).To(vm => vm.BiometricLoginCommand);
            set.Bind(CheckboxButton).For(x => x.Selected).To(vm => vm.RememberMe);
            set.Bind(MyIdCardLabel).For(x => x.Text).To(vm => vm.MyIdCardText);
            set.Bind(MyIdCardButton).To(vm => vm.ShowIDCard);

            set.Bind(UserNameTextField).For(x => x.Hidden).To(vm => vm.ShowLoginFields).WithConversion(boolOppositeValueConverter);
            set.Bind(PasswordTextField).For(x => x.Hidden).To(vm => vm.ShowLoginFields).WithConversion(boolOppositeValueConverter);
            set.Bind(LoginButton).For(x => x.Hidden).To(vm => vm.ShowLoginFields).WithConversion(boolOppositeValueConverter);
            set.Bind(BiometricLoginButton).For(x => x.Hidden).To(vm => vm.ShowTouchLogin).WithConversion(boolOppositeValueConverter);
            set.Bind(RememberMeContainer).For(x => x.Hidden).To(vm => vm.ShowLoginFields).WithConversion(boolOppositeValueConverter);
            set.Bind(UpdateAvailableLabel).For(x => x.Hidden).To(vm => vm.ShowForceUpgradeDialog).WithConversion(boolOppositeValueConverter);
            set.Bind(ForceUpdateLoginLabel).For(x => x.Hidden).To(vm => vm.ShowGenericErrorMessage).WithConversion(boolOppositeValueConverter);
            set.Bind(InstructionLabel).For(x => x.Hidden).To(vm => vm.ShowLoginFields).WithConversion(boolOppositeValueConverter);
            set.Bind(MyIdCardContainer).For(x => x.Hidden).To(vm => vm.CanShowCard).WithConversion(boolOppositeValueConverter);
            set.Bind(BiometricLoginButtonHeightConstraint).For(x => x.Constant).To(vm => vm.ShowTouchLogin).WithConversion(boolToDefaultFloatValueConverter, BiometricLoginButtonHeightConstraint.Constant);
            set.Apply();
        }

        private void OpenPlanMemberOnlineServices(Uri url)
        {
            var sfView = new SFViewController(url);
            PresentViewControllerAsync(sfView, true);
            _controllerDelegate = (SFViewControllerDelegate)sfView.Delegate;
            _controllerDelegate.RefreshEvent += DoRefresh;
        }

        private void DoRefresh(object sender, EventArgs args)
        {
            ViewDidLayoutSubviews();
            _controllerDelegate.RefreshEvent -= DoRefresh;
        }

        private void CheckboxSelected(object sender, EventArgs e)
        {
            var checkBox = sender as UIButton;
            checkBox.Selected = !checkBox.Selected;
            _viewModel.RememberMe = checkBox.Selected;
        }

        private void DetectBiometricCapability()
        {
            var context = new LAContext();
            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out NSError error))
            {
                var strSystemVersion = UIDevice.CurrentDevice.SystemVersion.Split('.')[0];
                double systemVersion = Convert.ToDouble(strSystemVersion);

                if (systemVersion >= 11.0 && context.BiometryType == LABiometryType.FaceId)
                {
                    BiometricLoginButton.SetTitle("loginWithFace".tr(), UIControlState.Normal);
                }
                else
                {
                    BiometricLoginButton.SetTitle("loginWithTouch".tr(), UIControlState.Normal);
                }
            }
        }

        private void SetMyIdCardImageDimensions(bool isCurrentCultureEng)
        {
            if (isCurrentCultureEng)
            {
#if CCQ
                if (Constants.IsPhone())
                {
                    MyIdCardLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 14f);
                    MyIdCardImageWidthConstraint.Constant = 35.0f;
                    MyIdCardImageHeightConstraint.Constant = 31.0f;
                    MyIdCardContainerHeightConstraint.Constant = 35.0f;
                    MyIdCardContainerWidthConstraint.Constant = 140.0f;
                }
                else
                {
                    MyIdCardLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 17f);
                    MyIdCardImageWidthConstraint.Constant = 50.0f;
                    MyIdCardImageHeightConstraint.Constant = 50.0f;
                    MyIdCardContainerHeightConstraint.Constant = 50.0f;
                    MyIdCardContainerWidthConstraint.Constant = 170.0f;
                }
#else
                if (Constants.IsPhone())
                {
                    MyIdCardLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 17f);
                    MyIdCardImageWidthConstraint.Constant = 25.0f;
                    MyIdCardImageHeightConstraint.Constant = 21.0f;
                    MyIdCardContainerHeightConstraint.Constant = 25.0f;                    
                }
                else
                {
                    MyIdCardLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 20f);
                    MyIdCardImageWidthConstraint.Constant = 35.0f;
                    MyIdCardImageHeightConstraint.Constant = 31.0f;
                    MyIdCardContainerHeightConstraint.Constant = 35.0f;
                }
#endif
            }
            else
            {
                if (Constants.IsPhone())
                {
                    MyIdCardLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 14f);
                    MyIdCardImageWidthConstraint.Constant = 35.0f;
                    MyIdCardImageHeightConstraint.Constant = 31.0f;
                    MyIdCardContainerHeightConstraint.Constant = 35.0f;
                    MyIdCardContainerWidthConstraint.Constant = 140.0f;
                }
                else
                {
                    MyIdCardLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 17f);
                    MyIdCardImageWidthConstraint.Constant = 50.0f;
                    MyIdCardImageHeightConstraint.Constant = 50.0f;
                    MyIdCardContainerHeightConstraint.Constant = 50.0f;
                    MyIdCardContainerWidthConstraint.Constant = 170.0f;
                }
            }
        }
    }
}