using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;
using WebKit;

namespace MobileClaims.iOS
{
    [RemoveAfterViewDidDisappear]
    public class ChangeForLifeTermsAndConditionsView : GSCBaseViewController
    {

        protected UIScrollView scrollableContainer;

        protected ChangeForLifeTermsAndConditionsViewModel _model;
        protected GreyNavButton generalButton;
        protected GreyNavButton privacyButton;
        protected GreyNavButton legalButton;
        protected GreyNavButton securityButton;

        protected WKWebView textArea;

        protected GSButton CheckboxAccept;
        protected GSButton CheckboxCancel;


        private float TOP_PADDING = Constants.IsPhone() ? 20.0f : 60.0f;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 50.0f : 100.0f;
        private float BUTTON_PADDING = 2.0f;
        private float CONTENT_PADDING = Constants.IsPhone() ? 10.0f : 20.0f;
        private float INNER_PADDING = 5.0f;

        private string currentContentString;
        private bool isGeneral;

        private NSUrl baseURL;
        private string imagePath;
        private NSUrl fileURL;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _model = (ChangeForLifeTermsAndConditionsViewModel)ViewModel;

            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            if (base.NavigationController != null)
            {
                base.NavigationController.NavigationBarHidden = true;
            }

            string path = NSBundle.MainBundle.BundlePath;
            baseURL = new NSUrl(path, true);

            imagePath = Constants.MISC_PATH + ("locale".tr() == "en" ? "gscLogo.png" : "gscLogo_fr.png");

            fileURL = NSUrl.FromFilename(imagePath);

            scrollableContainer = new UIScrollView();
            scrollableContainer.BackgroundColor = Colors.BACKGROUND_COLOR;
            scrollableContainer.Layer.CornerRadius = Constants.CORNER_RADIUS;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollableContainer);

            generalButton = new GreyNavButton();
            generalButton.SetTitle("C4L".tr(), UIControlState.Normal);
            generalButton.TouchUpInside += HandleGeneralButtonTouch;
            ((GSCBaseView)View).baseContainer.AddSubview(generalButton);

            privacyButton = new GreyNavButton();
            privacyButton.SetTitle("privacy".tr(), UIControlState.Normal);
            privacyButton.TouchUpInside += HandlePrivacyButtonTouch;
            ((GSCBaseView)View).baseContainer.AddSubview(privacyButton);

            legalButton = new GreyNavButton();
            legalButton.SetTitle("legal".tr(), UIControlState.Normal);
            legalButton.TouchUpInside += HandleLegalButtonTouch;
            ((GSCBaseView)View).baseContainer.AddSubview(legalButton);

            securityButton = new GreyNavButton();
            securityButton.SetTitle("security".tr(), UIControlState.Normal);
            securityButton.TouchUpInside += HandleSecurityButtonTouch;
            ((GSCBaseView)View).baseContainer.AddSubview(securityButton);

            textArea = new WKWebView(View.Bounds, new WKWebViewConfiguration());
            textArea.NavigationDelegate = new CustomWebViewDelegate(textArea, this);
            textArea.BackgroundColor = Colors.Clear;

            currentContentString = "C4LAgreement".tr()
                + "C4LTCContent1".FormatWithBrandKeywords(LocalizableBrand.GSC)
                + "C4LTCContent2".tr()
                + "C4LTCContent3".FormatWithBrandKeywords(LocalizableBrand.GSC)
                + "C4LTCContent4".FormatWithBrandKeywords(LocalizableBrand.GSC)
                + "C4LTCContent5".FormatWithBrandKeywords(LocalizableBrand.GSC);

            var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

            textArea.LoadHtmlString(headerString + htmlFormatString(currentContentString), baseURL);
            ((GSCBaseView)View).baseContainer.AddSubview(textArea);

            if (!_model.IsAccepted)
            {

                isGeneral = true;

                CheckboxAccept = new GSButton();
                CheckboxAccept.SetTitle("acceptToTerms".tr(), UIControlState.Normal);
                View.AddSubview(CheckboxAccept);

                CheckboxCancel = new GSButton();
                CheckboxCancel.SetTitle("cancelToTerms".tr(), UIControlState.Normal);
                View.AddSubview(CheckboxCancel);

            }

            generalButton.Selected = true;
            //_model.AcceptedTC = true;
            var set = this.CreateBindingSet<ChangeForLifeTermsAndConditionsView, Core.ViewModels.ChangeForLifeTermsAndConditionsViewModel>();
            set.Bind(this.CheckboxAccept).To(vm => vm.AcceptTermsAndConditionsCommand);
            set.Bind(this.CheckboxCancel).To(vm => vm.CancelTermsAndConditionsCommand);
            set.Bind(CheckboxAccept).For(v => v.Enabled).To(vm => vm.IsNotBusy);
            //set.Bind (this.CheckboxCancel).To (vm => vm.IsAccepted);
            set.Apply();

        }

        void HandlePrivacyButtonTouch(object sender, EventArgs e)
        {
            isGeneral = false;

            privacyButton.Selected = true;
            legalButton.Selected = false;
            securityButton.Selected = false;
            generalButton.Selected = false;
            currentContentString = "privacyContent1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent2".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent3".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent4".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent5".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent6".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent7".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "privacyContent8".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

            textArea.LoadHtmlString(headerString + htmlFormatString(currentContentString), baseURL);

            View.SetNeedsLayout();
        }

        void HandleLegalButtonTouch(object sender, EventArgs e)
        {
            isGeneral = false;

            privacyButton.Selected = false;
            legalButton.Selected = true;
            securityButton.Selected = false;
            generalButton.Selected = false;
            currentContentString = "legalContent1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "legalContent2".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada, LocalizableBrand.GreenShield)
                + "legalContent3".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

            textArea.LoadHtmlString(headerString + htmlFormatString(currentContentString), baseURL);

            View.SetNeedsLayout();
        }

        void HandleSecurityButtonTouch(object sender, EventArgs e)
        {
            isGeneral = false;

            privacyButton.Selected = false;
            legalButton.Selected = false;
            securityButton.Selected = true;
            generalButton.Selected = false;
            currentContentString = "securityContent1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "securityContent2".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                + "securityContent3".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada);
            var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

            textArea.LoadHtmlString(headerString + htmlFormatString(currentContentString), baseURL);

            View.SetNeedsLayout();
        }

        void HandleGeneralButtonTouch(object sender, EventArgs e)
        {
            isGeneral = true;

            privacyButton.Selected = false;
            legalButton.Selected = false;
            securityButton.Selected = false;
            generalButton.Selected = true;
            currentContentString = "C4LAgreement".tr()
                + "C4LTCContent1".FormatWithBrandKeywords(LocalizableBrand.GSC)
                + "C4LTCContent2".tr()
                + "C4LTCContent3".FormatWithBrandKeywords(LocalizableBrand.GSC)
                + "C4LTCContent4".FormatWithBrandKeywords(LocalizableBrand.GSC)
                + "C4LTCContent5".FormatWithBrandKeywords(LocalizableBrand.GSC);
            var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

            textArea.LoadHtmlString(headerString + htmlFormatString(currentContentString), baseURL);

            View.SetNeedsLayout();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float viewwidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height;
            float statusBarHeight = 20; //Status bar height acting peculiar in landscape. TODO: Figure better way to get status bar height. Tried: UIApplication.SharedApplication.StatusBarFrame.Size.Height
            float contentWidth = viewwidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
            float buttonWidth = (viewwidth - (BUTTON_PADDING * 3)) / 4;

            float yPOS = TOP_PADDING;
            float startY = Constants.IsPhone() ? 5 : 40;

            float navOffset = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            float BUTTON_WIDTH = Constants.IsPhone() ? 120 : 250;
            float BUTTON_HEIGHT = Constants.IsPhone() ? 30 : 45;
            float BUTTON_HEIGHT1 = Constants.IsPhone() ? 40 : 50;
            float extraHeight;


            if (UIScreen.MainScreen.NativeBounds.Height == 2436)
            {
                extraHeight = Helpers.IsInPortraitMode() ? 10 : -10;

            }
            else
            {
                extraHeight = -5;
            }

            navOffset = navOffset + BUTTON_HEIGHT1 + BUTTON_PADDING;
            scrollableContainer.Frame = new CGRect(0, Constants.DRUG_LOOKUP_LABEL_HEIGHT, viewwidth, viewHeight);

            generalButton.Frame = new CGRect(0, startY, buttonWidth, BUTTON_HEIGHT);
            privacyButton.Frame = new CGRect(BUTTON_PADDING + buttonWidth, startY, buttonWidth, BUTTON_HEIGHT);
            legalButton.Frame = new CGRect(BUTTON_PADDING * 2 + buttonWidth * 2, startY, buttonWidth, BUTTON_HEIGHT);
            securityButton.Frame = new CGRect(buttonWidth * 3 + BUTTON_PADDING * 3, startY, buttonWidth, BUTTON_HEIGHT);

            yPOS += (float)generalButton.Frame.Height;

            textArea.Frame = new CGRect(0, yPOS, viewwidth, viewHeight - (CONTENT_PADDING * (Helpers.IsInPortraitMode() ? 3.5 : 2.5)) - yPOS - ((isGeneral ? navOffset : navOffset / 2)) - extraHeight);
            var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

            textArea.LoadHtmlString(headerString + htmlFormatString(currentContentString), baseURL);

            yPOS += (float)textArea.Frame.Height + CONTENT_PADDING / 2;

            if (CheckboxAccept != null)
            {

                float checkboxX = ((viewwidth / 2) - BUTTON_WIDTH) / 2;

                if (isGeneral)
                {
                    CheckboxAccept.Frame = new CGRect(viewwidth / 2 - (checkboxX + BUTTON_WIDTH), viewHeight - navOffset + extraHeight, BUTTON_WIDTH, BUTTON_HEIGHT1);
                    CheckboxAccept.Hidden = false;

                    CheckboxCancel.Frame = new CGRect(viewwidth / 2 + checkboxX, viewHeight - navOffset + extraHeight, BUTTON_WIDTH, BUTTON_HEIGHT1);
                    CheckboxCancel.Hidden = false;

                }
                else
                {
                    CheckboxCancel.Hidden = true;
                    CheckboxAccept.Hidden = true;
                }

                /*
				float checkboxWidth = (float)CheckboxAccept.BackgroundImageForState(UIControlState.Normal).Size.Width;
				float checkboxHeight = (float)CheckboxAccept.BackgroundImageForState(UIControlState.Normal).Size.Height;

				float checkboxX = viewwidth / 2 - (float)acceptLabel.Frame.Width / 2;  
				 
				CheckboxAccept.Frame = new CGRect (checkboxX, yPOS + (float)acceptLabel.Frame.Height/2 - checkboxHeight/2, checkboxWidth, checkboxHeight);
				acceptLabel.Frame = new CGRect ((float)CheckboxAccept.Frame.X + checkboxWidth + INNER_PADDING, yPOS, (float)acceptLabel.Frame.Width, (float)acceptLabel.Frame.Height);

				CheckboxCancel.Frame = new CGRect ((float)CheckboxAccept.Frame.X + checkboxWidth*2 + INNER_PADDING, yPOS + (float)acceptLabel.Frame.Height/2 - checkboxHeight/2, checkboxWidth, checkboxHeight);
				cancelLabel.Frame = new CGRect ((float)CheckboxAccept.Frame.X + checkboxWidth*3 + INNER_PADDING, yPOS, (float)acceptLabel.Frame.Width, (float)acceptLabel.Frame.Height);
				CheckboxAccept.Hidden = false;
				acceptLabel.Hidden = false; 
				CheckboxCancel.Hidden = false;
				cancelLabel.Hidden = false; 
				*/
            }

            scrollableContainer.ContentSize = new CGSize(viewwidth, Constants.LIST_BUTTON_HEIGHT);
        }

        private string htmlFormatString(string content)
        {
            if (isGeneral)
                return "<html> \n <head> \n <style type=\"text/css\"> \n body {font-family: \"" + Constants.HTML_FONT + "\"; font-size:" + Constants.TERMS_TEXT_FONT_SIZE + "; padding-left:" + Constants.TERMS_SIDE_PADDING + "px; padding-right:" + Constants.TERMS_SIDE_PADDING + "px; color:#" + Colors.DARK_GREY_COLOR_HEX + " ;}\n p{clear:both;} \n h3{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ; font-family: \"" + Constants.HTML_HEADING_FONT + "\";}\n h2{color:#" + Colors.DARK_GREY_COLOR_HEX + " ; font-family: \"" + Constants.HTML_HEADING_FONT + "\";} \n a{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ;} \n </style> \n </head> \n <body>" + content + "</body> \n </html>";
            else
                return "<html> \n <head> \n <style type=\"text/css\"> \n body {font-family: \"" + Constants.HTML_FONT + "\"; font-size:" + Constants.TERMS_TEXT_FONT_SIZE + "; padding-left:" + Constants.TERMS_SIDE_PADDING + "px; padding-right:" + Constants.TERMS_SIDE_PADDING + "px; color:#" + Colors.DARK_GREY_COLOR_HEX + " ;}\n p{clear:both;} \n h3{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ; font-family: \"" + Constants.HTML_HEADING_FONT + "\";}\n h2{color:#" + Colors.DARK_GREY_COLOR_HEX + " ; font-family: \"" + Constants.HTML_HEADING_FONT + "\";} \n a{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ;} \n </style> \n </head> \n <body> <img src=\"" + fileURL.AbsoluteString + "\" style=\"float:right;margin:0 5px 0 0;\"  <p></p> <br/>" + content + "</body> \n </html>";

        }

        public class CustomWebViewDelegate : WKNavigationDelegate
        {
            private WKWebView _view;
            ChangeForLifeTermsAndConditionsView _parentView;
            public CustomWebViewDelegate(WKWebView view, ChangeForLifeTermsAndConditionsView parentView)
            {
                _view = view;
                _parentView = parentView;
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                var url = navigationAction.Request.Url;
                if (url.AbsoluteString.Contains("priv.gc.ca"))
                {
                    UIApplication.SharedApplication.OpenUrl(url);
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                }
                else if (url.Equals("http://security.com/"))
                {
                    // redirect to security tab
                    _parentView.HandleSecurityButtonTouch(null, null);
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                }
                else if (url.Equals("http://legal.com/"))
                {
                    // redirect to legal tab
                    _parentView.HandleLegalButtonTouch(null, null);
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                }
                else if (url.Equals("http://privacy.com/"))
                {
                    // redirect to privacy tab
                    _parentView.HandlePrivacyButtonTouch(null, null);
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(url);
                    decisionHandler(WKNavigationActionPolicy.Allow);
                }
            }

            public bool ShouldStartLoad(WKWebView webView,
                Foundation.NSUrlRequest request,
                UIWebViewNavigationType navigationType)
            {
                System.Console.WriteLine("Starting load");

                if (request.Url.AbsoluteString.Equals("http://security.com/"))
                {
                    // redirect to security tab
                    _parentView.HandleSecurityButtonTouch(null, null);
                    return false;
                }
                else if (request.Url.AbsoluteString.Equals("http://legal.com/"))
                {
                    // redirect to legal tab
                    _parentView.HandleLegalButtonTouch(null, null);
                    return false;
                }
                else if (request.Url.AbsoluteString.Equals("http://privacy.com/"))
                {
                    // redirect to privacy tab
                    _parentView.HandlePrivacyButtonTouch(null, null);
                    return false;
                }

                if (navigationType == UIWebViewNavigationType.LinkClicked)
                {
                    UIApplication.SharedApplication.OpenUrl(request.Url);
                    return false;
                }

                return true;
            }

        }

    }
}

