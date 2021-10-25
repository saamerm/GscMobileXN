using System;
using System.Globalization;
using System.IO;
using Foundation;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Services.FacadeEntities;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.WebClaimSubmissionAgreement
{
    [RemoveAfterViewDidDisappear]
    public partial class WebAgreementView : GSCBaseViewController
    {
        private bool _isLoadingDynamicText;
        private TextAlterationWithDate _dynamicText;
       
        private string currentContentString;
       
        private NSUrl baseURL;
        private string _filePath = "WebClaimSubmissionAgreementResources/webagreement_en.html";

        protected WebAgreementViewModel _model;
        
        public bool IsLoadingDynamicText
        {
            get
            {
                return _isLoadingDynamicText;
            }
            set
            {
                _isLoadingDynamicText = value;
            }
        }

        public TextAlterationWithDate DynamicText
        {
            get
            {
                return _dynamicText;
            }
            set
            {
                _dynamicText = value;
                CreateHTMLTextForTexAlteration(value);
                var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

                WCSAWebView.LoadHtmlString(headerString + currentContentString, baseURL);
            }
        }

        public WebAgreementView()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            if (Constants.Bottom == 0)
            {
                ButtonBottomConstraint.Constant = 10;
            }
            else
            {
                ButtonBottomConstraint.Constant = Constants.Bottom;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationController.NavigationItem.SetHidesBackButton(false, false);
            base.NavigationItem.SetHidesBackButton(false, false);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = (WebAgreementViewModel)ViewModel;

            SetResourcePath();

            base.NavigationController.NavigationBarHidden = true;
            base.NavigationController.NavigationItem.SetHidesBackButton(true, false);
            base.NavigationItem.SetHidesBackButton(true, false);

            AcceptButton.SetTitle("acceptToTerms".tr(), UIControlState.Normal);
            CancelButton.SetTitle("cancelToTerms".tr(), UIControlState.Normal);
            WCSAWebView.BackgroundColor = UIColor.White;
            ButtonContainer.Layer.BorderColor = Colors.LightGrayColor.CGColor;
            ButtonContainer.Layer.BorderWidth = 1f;

            var set = this.CreateBindingSet<WebAgreementView, WebAgreementViewModel>();
            set.Bind(AcceptButton).To(vm => vm.AcceptTermsAndConditionsCommand);
            set.Bind(AcceptButton).For(x => x.Hidden).To(vm => vm.IsAccepted);
            set.Bind(CancelButton).To(vm => vm.CancelTermsAndConditionsCommand);            
            set.Bind(CancelButton).For(x => x.Hidden).To(vm => vm.IsAccepted);

            set.Bind(this).For(v => v.IsLoadingDynamicText).To(vm => vm.IsLoadingDynamicText);
            set.Bind(this).For(v => v.DynamicText).To(vm => vm.TextAlteration);
            set.Apply();
        }

        private void SetResourcePath()
        {
            if (!CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en"))
            {
                _filePath = "WebClaimSubmissionAgreementResources/webagreement_fr.html";
            }
        }

        private void CreateHTMLTextForTexAlteration(TextAlterationWithDate value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.TextAlteration)
                                              && !string.IsNullOrWhiteSpace(value.TextAlterationDate)
                                              && !string.Equals(value.TextAlteration, StringConstants.TextAlterationNotAvailable, StringComparison.OrdinalIgnoreCase)
                                              && !string.Equals(value.TextAlterationDate, StringConstants.TextAlterationDateNotAvailable, StringComparison.OrdinalIgnoreCase))
            {
                ContentString(value.TextAlteration, value.TextAlterationDate);
                currentContentString = HtmlFormatString(currentContentString);
            }
            else
            {
                if (IsLoadingDynamicText)
                {
                    // Brand specific Web Claim Submission Agreement file for CCQ and ARTA
#if CCQ || ARTA || WWL
                    using (var sr = new StreamReader(_filePath))
                    {
                        var htmlContents = sr.ReadToEnd();
                        currentContentString = htmlContents.Replace("{GENERAL_FONT}", Constants.HTML_FONT)
                              .Replace("{GENERAL_FONT_SIZE}", Constants.TERMS_TEXT_FONT_SIZE.ToString() + "px")
                              .Replace("{GENERAL_PADDING}", Constants.TERMS_SIDE_PADDING.ToString() + "px")
                              .Replace("{GENERAL_TEXT_COLOR}", "#" + Colors.DARK_GREY_COLOR_HEX)
                              .Replace("{H3_TEXT_COLOR}", "#" + Colors.HIGHLIGHT_COLOR_HEX)
                              .Replace("{H3_FONT}", Constants.HTML_HEADING_FONT);
                    }
#else
                    ContentString();
                    currentContentString = HtmlFormatString(currentContentString);
#endif
                }
            }
        }

        private string HtmlFormatString(string content)
        {
            return "<html> \n <head> \n <style type=\"text/css\"> \n body {font-family: \"" + Constants.HTML_FONT + "\"; font-size:" + Constants.TERMS_TEXT_FONT_SIZE + "; padding-left:" + Constants.TERMS_SIDE_PADDING + "px; padding-right:" + Constants.TERMS_SIDE_PADDING + "px; color:#" + Colors.DARK_GREY_COLOR_HEX + " ;}\n p{clear:both;} \n h3{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ; font-family: \"" + Constants.HTML_HEADING_FONT + "\";}\n h2{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ; font-family: \"" + Constants.HTML_HEADING_FONT + "\";} \n a{color:#" + Colors.HIGHLIGHT_COLOR_HEX + " ;} \n </style> \n </head> \n <body>" + content + "</body> \n </html>";
        }

        private void ContentString(string dText = null, string dDate = null)
        {
            if (dText != null && dDate != null)
            {
                currentContentString = "webAgreementTitle".tr()
                    + "webContent1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada) + string.Format("newTermsDateDynamic".tr(), dDate)
                    + string.Format("pmosHeading".tr().Replace("{HIGHLIGHT_COLOR_HEX}", "#" + Colors.HIGHLIGHT_COLOR_HEX),
                        LocalizableBrand.GreenShieldCanada)
                    + "webContent2".tr() + dText + "webContent16".tr();
            }
            else
            {
                currentContentString = "webAgreementTitle".tr()
                    + "webContent1".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada) + "newTermsDate".tr()
                    + string.Format("pmosHeading".tr().Replace("{HIGHLIGHT_COLOR_HEX}", "#" + Colors.HIGHLIGHT_COLOR_HEX),
                        LocalizableBrand.GreenShieldCanada)
                    + "webContent2".tr() + "webContent3".tr() + "webContent4".tr() + "webContent5".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                    + "webContent6".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                    + "webContent7".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada) + "webContent8".tr()
                    + "webContent9".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                    + "webContent10".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                    + "webContent11".tr()
                    + "webContent12".FormatWithBrandKeywords(LocalizableBrand.GreenShieldCanada)
                    + "webContent13".tr()
                    + "webContent14".tr() + "webContent15".tr() + "webContent16".tr();
            }
        }

        private void OnWebAgreementViewDismissed(object sender, EventArgs e)
        {
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationController.PopViewController(false);
        }

        private void OnWebAgreementViewDismissedWhenCanceled(object sender, EventArgs e)
        {
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationController.PopViewController(false);
        }
    }
}