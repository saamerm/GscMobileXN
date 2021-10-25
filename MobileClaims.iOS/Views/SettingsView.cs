using System;
using CoreGraphics;
using Foundation;
using UIKit;
using MobileClaims.Core.ViewModels;
using LocalAuthentication;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS.Views
{
    [Foundation.Register("SettingsView")]
    public class SettingsView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
    {

        protected SettingsViewModel _model;

        public UISwitch toggleSwitch;
        public UILabel settingsName;
        public UILabel biometricsContent1;
        public UILabel biometricsContent2;
        public UILabel biometricsContent3;
        public UIScrollView scrollContainer;
        public LAContext context;

        protected bool prevValue = false;

        public event System.EventHandler VisibilityToggled;
        protected bool isEnabled;
        private float FIELD_WIDTH = Constants.IsPhone() ? (float)1.2 : (float)1.08;


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();         

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.Title = "mysettings".tr();
            base.NavigationController.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.BackBarButtonItem = new UIBarButtonItem(
    " ".tr(), UIBarButtonItemStyle.Plain, null, null);


            NSError error;
            
            _model = (SettingsViewModel)ViewModel;
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            scrollContainer = new UIScrollView();
            scrollContainer.BackgroundColor = Colors.Clear;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            toggleSwitch = new UISwitch();
            toggleSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            toggleSwitch.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
            toggleSwitch.ValueChanged += HandleSwitch;
            scrollContainer.AddSubview(toggleSwitch);


            toggleSwitch.On = _model.UseBiometricsSetting;

            settingsName = new UILabel();
            settingsName.Text = "dontAskTouch".tr();
            settingsName.BackgroundColor = Colors.Clear;
            settingsName.TextColor = Colors.DARK_GREY_COLOR;
            settingsName.TextAlignment = UITextAlignment.Natural;
            settingsName.Lines = 0;
            settingsName.LineBreakMode = UILineBreakMode.WordWrap;
            settingsName.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);

            biometricsContent1 = new UILabel();
            biometricsContent1.Text = "biometricsPara1".tr();
            biometricsContent1.BackgroundColor = Colors.Clear;
            biometricsContent1.TextColor = Colors.DARK_GREY_COLOR;
            biometricsContent1.TextAlignment = UITextAlignment.Natural;
            biometricsContent1.Lines = 0;
            biometricsContent1.LineBreakMode = UILineBreakMode.WordWrap;
            biometricsContent1.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            scrollContainer.AddSubview(biometricsContent1);

            biometricsContent2 = new UILabel();
            biometricsContent2.Text = "biometricsPara2".tr();
            biometricsContent2.BackgroundColor = Colors.Clear;
            biometricsContent2.TextColor = Colors.DARK_GREY_COLOR;
            biometricsContent2.TextAlignment = UITextAlignment.Natural;
            biometricsContent2.Lines = 0;
            biometricsContent2.LineBreakMode = UILineBreakMode.WordWrap;
            biometricsContent2.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            scrollContainer.AddSubview(biometricsContent2);

            biometricsContent3 = new UILabel();
            biometricsContent3.Text = "biometricsPara3".tr();
            biometricsContent3.BackgroundColor = Colors.Clear;
            biometricsContent3.TextColor = Colors.DARK_GREY_COLOR;
            biometricsContent3.TextAlignment = UITextAlignment.Natural;
            biometricsContent3.Lines = 0;
            biometricsContent3.LineBreakMode = UILineBreakMode.WordWrap;
            biometricsContent3.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            scrollContainer.AddSubview(biometricsContent3);

            var set = this.CreateBindingSet<SettingsView, Core.ViewModels.SettingsViewModel>();
            set.Bind(toggleSwitch).To(vm => vm.UseBiometricsSetting);
            set.Apply();

            context = new LAContext();

            if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
            {
                // Do nothing
            }
            else
            {
                toggleSwitch.UserInteractionEnabled = true;
                _model.UseBiometricsSetting = false;
            }
            scrollContainer.AddSubview(settingsName);




        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            //float viewWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            float startY = ViewContentYPositionPadding;
            float yPos = 0;
            float itemPadding = 5;
            float itemPaddingParagraph = 20;

            settingsName.Frame = new CGRect(itemPadding, startY + 10, ViewContainerWidth, (float)settingsName.Frame.Height);
            settingsName.SizeToFit();

            yPos = startY + (float)settingsName.Frame.Height + itemPadding;


            toggleSwitch.Frame = new CGRect(ViewContainerWidth / FIELD_WIDTH, startY, ViewContainerWidth, (float)toggleSwitch.Frame.Height);
            yPos += (float)toggleSwitch.Frame.Height + itemPaddingParagraph;

            biometricsContent1.Frame = new CGRect(itemPadding, yPos, ViewContainerWidth, (float)biometricsContent1.Frame.Height);
            biometricsContent1.SizeToFit();

            yPos += (float)biometricsContent1.Frame.Height + itemPaddingParagraph;

            biometricsContent2.Frame = new CGRect(itemPadding, yPos, ViewContainerWidth, (float)biometricsContent2.Frame.Height);
            biometricsContent2.SizeToFit();

            yPos += (float)biometricsContent2.Frame.Height + itemPaddingParagraph;

            biometricsContent3.Frame = new CGRect(itemPadding, yPos, ViewContainerWidth, (float)biometricsContent3.Frame.Height);
            biometricsContent3.SizeToFit();

            scrollContainer.Frame = new CGRect(0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, yPos + GetBottomPadding());
        }

        void HandleSwitch(object sender, EventArgs e)
        {
            bool shouldOpen = isEnabled ? !toggleSwitch.On : toggleSwitch.On;

            if (shouldOpen)
            {
                _model.UseBiometricsSetting = true;
                isEnabled = false;
            } 
            else
            {
                _model.UseBiometricsSetting = false;
                isEnabled = true;
            }

            context = new LAContext();
            NSError error;

            if (!context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out error))
            {
                toggleSwitch.SetState(false, true);

                if (!toggleSwitch.On && toggleSwitch.On == prevValue){
                    UIAlertView _error = new UIAlertView("", "biometricsDialog".tr(), null, "ok".tr(), null);
                    _error.Show();
                    prevValue = true;
                    return;

                }
                prevValue = false;
            }
        }

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            }
            else
            {
                return (float)base.View.Bounds.Width;
            }
        }

        public float GetViewContainerHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Constants.NAV_HEIGHT;
            }
            else
            {
                return (float)base.View.Bounds.Height - Constants.NAV_HEIGHT;
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return Constants.IsPhone() ? 20 : 85;
            }
            else
            {
                if (Constants.IS_OS_7_OR_LATER())
                {
                    if (Constants.IsPhone())
                    {
                        return 20;
                    }
                    else
                    {
                        return Constants.IOS_7_TOP_PADDING;
                    }
                }
                else
                {
                    return Constants.IOS_6_TOP_PADDING;
                }
            }
        }
    }
}