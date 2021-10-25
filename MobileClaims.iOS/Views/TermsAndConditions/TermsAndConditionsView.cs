using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;
using WebKit;

namespace MobileClaims.iOS.Views.TermsAndConditions
{
    public partial class TermsAndConditionsView : GSCBaseViewController
    {
        private static string _imagePath = Constants.MISC_PATH_IPad + "Logo.png";
        private static string _emptyHtmlFilePath = "TermsAndConditionsResources/Empty.html";

        public int _selectedButtonIndex;
        private NSUrl _baseURL;
        public TermsAndConditionsViewModel _model;
        public string[] _filePaths =
        {
            "TermsAndConditionsResources/TermsAndConditions_en.html",
            "TermsAndConditionsResources/Privacy_en.html",
            "TermsAndConditionsResources/Legal_en.html",
            "TermsAndConditionsResources/Security_en.html",
        };

        public SFViewController sfView;
        public SFViewControllerDelegate controllerDelegate;

        public TermsAndConditionsView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _model = (TermsAndConditionsViewModel)ViewModel;
            _model.PropertyChanged += Model_PropertyChanged;
            View.BackgroundColor = Colors.BACKGROUND_COLOR;

            SetResourcePath();

            SetButtonState(0);
            LoadHtmlFile(_filePaths[0], !_model.NoPhoneAlteration, !_model.NoClaimAlteration, !_model.AcceptedTC);

            TermsAndConditionsWebView.BackgroundColor = Colors.Clear;
            TermsAndConditionsWebView.NavigationDelegate = new CustomWebViewDelegate(TermsAndConditionsWebView, this);
            TermsAndConditionsWebView.SizeToFit();
            var javaScript = @"
            var meta = document.createElement('meta');
            meta.setAttribute('name', 'viewport');
            meta.setAttribute('content', 'width=device-width');
            document.getElementsByTagName('head')[0].appendChild(meta);
            ";
            var jS = new NSString(javaScript);
            TermsAndConditionsWebView.EvaluateJavaScript(jS, (NSObject result, NSError error) => Console.WriteLine("SUCCESSS"));

            GeneralButton.TouchUpInside += HandleGeneralButtonTouch;
            SecurityButton.TouchUpInside += HandleSecurityButtonTouch;
            PrivacyButton.TouchUpInside += HandlePrivacyButtonTouch;
            LegalButton.TouchUpInside += HandleLegalButtonTouch;

            CheckBoxButton.SetBackgroundImage(UIImage.FromBundle("CheckboxNormal"), UIControlState.Normal);
            CheckBoxButton.SetBackgroundImage(UIImage.FromBundle("CheckboxSelected"), UIControlState.Selected);
            CheckBoxButton.AdjustsImageWhenHighlighted = true;

            CheckBoxLabel.TextColor = Colors.Black;
            CheckBoxLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.TCCheckBoxFontSize);

            CheckBoxContainer.Layer.BorderColor = Colors.LightGrayColor.CGColor;
            CheckBoxContainer.Layer.BorderWidth = 1f;
            SetBindings();

            var locationManager = new CLLocationManager();
            locationManager.RequestWhenInUseAuthorization();

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(true, false);

            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            if (_model.AcceptedTC)
            {
                WebViewBottomConstraint.Constant = (Constants.Bottom / 2) + bnbHeight;
            }
            else
            {
                var bottomOffset = Constants.Bottom / 2;
                bnbHeight += bottomOffset;
                CheckBoxContainerHeightConstraint.Constant = bnbHeight;
                CheckBoxButtonYCenterConstraint.Constant = -bottomOffset;
                CheckBoxLabelYCenterConstraint.Constant = -bottomOffset;
            }

            if (!Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                TCButtonContainerTopConstraint.Constant = 20;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            CheckBoxButton.TouchUpInside += CheckBoxButton_TouchUpInside;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            CheckBoxButton.TouchUpInside -= CheckBoxButton_TouchUpInside;
        }

        private void SetBindings()
        {
            var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter(true);

            var set = this.CreateBindingSet<TermsAndConditionsView, TermsAndConditionsViewModel>();
            set.Bind(GeneralButton).For("Title").To(vm => vm.General);
            set.Bind(SecurityButton).For("Title").To(vm => vm.Security);
            set.Bind(PrivacyButton).For("Title").To(vm => vm.Privacy);
            set.Bind(LegalButton).For("Title").To(vm => vm.Legal);

            set.Bind(CheckBoxButton).To(vm => vm.AcceptTermsAndConditions);
            set.Bind(CheckBoxButton).For(x => x.Selected).To(vm => vm.AcceptedTC);
            set.Bind(CheckBoxContainer).For(x => x.Hidden).To(vm => vm.AcceptedTC);
            set.Bind(CheckBoxContainerHeightConstraint).For(x => x.Constant).To(vm => vm.AcceptedTC).WithConversion(boolToDefaultFloatValueConverter, CheckBoxContainerHeightConstraint.Constant);

            set.Bind(CheckBoxLabel).To(vm => vm.IAgreeToTC);
            set.Apply();
        }

        private void CheckBoxButton_TouchUpInside(object sender, EventArgs e)
        {
            var cb = sender as UIButton;
            cb.Selected = !cb.Selected;
        }

        private void HandleGeneralButtonTouch(object sender, EventArgs e)
        {
            SetButtonState(0);
            LoadHtmlFile(_filePaths[0], !_model.NoPhoneAlteration, !_model.NoClaimAlteration, !_model.AcceptedTC);
        }

        private void HandlePrivacyButtonTouch(object sender, EventArgs e)
        {
            SetButtonState(1);
            LoadHtmlFile(_filePaths[1], !_model.NoPhoneAlteration, false, true);
        }

        private void HandleLegalButtonTouch(object sender, EventArgs e)
        {
            SetButtonState(2);
            LoadHtmlFile(_filePaths[2], false, false, true);
        }

        private void HandleSecurityButtonTouch(object sender, EventArgs e)
        {
            SetButtonState(3);
            LoadHtmlFile(_filePaths[3], false, false, true);
        }

        public void LoadHtmlFile(string path, bool updatePhoneNumber, bool updateClaimText, bool phoneNumberOnly)
        {
            using (var sr = new StreamReader(path))
            {
                var htmlContents = sr.ReadToEnd();
                htmlContents = htmlContents.Replace("{GENERAL_FONT}", Constants.HTML_FONT)
                                .Replace("{GENERAL_FONT_SIZE}", Constants.TERMS_TEXT_FONT_SIZE.ToString() + "px")
                                .Replace("{GENERAL_PADDING}", Constants.TERMS_SIDE_PADDING.ToString() + "px")
                                .Replace("{GENERAL_TEXT_COLOR}", "#" + Colors.DARK_GREY_COLOR_HEX)
                                .Replace("{H3_TEXT_COLOR}", "#" + Colors.HIGHLIGHT_COLOR_HEX)
                                .Replace("{H3_FONT}", Constants.HTML_HEADING_FONT)
                                .Replace("{LOGO_PATH}", _imagePath)
                                .Replace("{Logo_Size}", Constants.IsPhone() ? Constants.TCLogoHeightOniPhone : Constants.TCLogoHeightOniPad);

                StringBuilder sb = new StringBuilder();
                if (!phoneNumberOnly)
                {
                    sb.Append("<li>");
                    if (updateClaimText && _model.ClaimAgreement != null)
                    {
                        sb.Append(string.Join("</br></br>", _model.ClaimAgreement.Select(x => x.Text)));
                    }
                    else
                    {
                        sb.Append(_model.TermsAndConditionsContent1);
                        sb.Append("</br></br>");
                        sb.Append(_model.TermsAndConditionsContent2);
                        sb.Append("</br></br>");
                        sb.Append(_model.TermsAndConditionsContent3);
                        sb.Append("</br></br>");
                        sb.Append(_model.TermsAndConditionsContent4);
                        sb.Append("</br></br>");
                        sb.Append(_model.TermsAndConditionsContent5);
                        sb.Append("</br></br>");
                        sb.Append(_model.TermsAndConditionsContent6);
                        sb.Append("</br></br>");
                        sb.Append(_model.TermsAndConditionsContent7);
                    }
                    sb.Append("</li>");
                }
                htmlContents = htmlContents.Replace("{CLAIM_AGREEMENT}", sb.ToString());

                var phoneSB = new StringBuilder();
                if (updatePhoneNumber && _model.PhoneNumber != null)
                {
                    phoneSB.AppendFormat($"<a href=\"tel:{_model.PhoneNumber.Text}\" style='{Colors.HIGHLIGHT_COLOR}'>{_model.PhoneNumber}</a>");
                }
                else
                {
                    phoneSB.AppendFormat($"<a href=\"tel:{_model.TermsAndConditionsContent_Phone.Replace("-", string.Empty)}\" style='{Colors.HIGHLIGHT_COLOR}'>{_model.TermsAndConditionsContent_Phone}</a>");
                }
                htmlContents = htmlContents.Replace("{PHONE_NUMBER}", phoneSB.ToString());
                var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

                TermsAndConditionsWebView.LoadHtmlString(headerString + htmlContents, _baseURL);
            }
        }

        public void SetButtonState(int index)
        {
            _selectedButtonIndex = index;
            GeneralButton.Selected = index == 0;
            PrivacyButton.Selected = index == 1;
            LegalButton.Selected = index == 2;
            SecurityButton.Selected = index == 3;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PhoneBusy" || e.PropertyName == "ClaimBusy")
            {
                if (_model.ClaimError || _model.PhoneError)
                {
                    HandleLoginConnectionIssues();
                }
                if (!_model.PhoneBusy && !_model.ClaimBusy)
                {
                    SetButtonState(_selectedButtonIndex);
                    LoadHtmlFile(_filePaths[0], !_model.NoPhoneAlteration, !_model.NoClaimAlteration, false);
                }
            }
        }

        private void HandleLoginConnectionIssues()
        {
            GeneralButton.Hidden = true;
            PrivacyButton.Hidden = true;
            LegalButton.Hidden = true;
            SecurityButton.Hidden = true;

            var connectionError = "connectionError".tr();
            using (var sr = new StreamReader(_emptyHtmlFilePath))
            {
                var htmlContents = sr.ReadToEnd();
                htmlContents = htmlContents.Replace("{GENERAL_FONT}", Constants.HTML_FONT)
                               .Replace("{GENERAL_FONT_SIZE}", Constants.TERMS_TEXT_FONT_SIZE.ToString() + "px")
                               .Replace("{GENERAL_PADDING}", Constants.TERMS_SIDE_PADDING.ToString() + "px")
                               .Replace("{GENERAL_TEXT_COLOR}", "#" + Colors.DARK_GREY_COLOR_HEX)
                               .Replace("{H3_TEXT_COLOR}", "#" + Colors.HIGHLIGHT_COLOR_HEX)
                               .Replace("{H3_FONT}", Constants.HTML_HEADING_FONT)
                               .Replace("{LOGO_PATH}", _imagePath);
                htmlContents = htmlContents.Replace("{CUSTOM_HTML_CONTENTS", connectionError);
                var headerString = "<head><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no'></head>";

                TermsAndConditionsWebView.LoadHtmlString(headerString + htmlContents, _baseURL);
            }
        }

        private void SetResourcePath()
        {
            var basePath = NSBundle.MainBundle.BundlePath;
            _baseURL = new NSUrl(basePath, true);

            if (!CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en"))
            {
                _imagePath = Constants.MISC_PATH_IPad + "Logo-fr.png";

                _filePaths = new string[]
                {
                    "TermsAndConditionsResources/TermsAndConditions_fr.html",
                    "TermsAndConditionsResources/Privacy_fr.html",
                    "TermsAndConditionsResources/Legal_fr.html",
                    "TermsAndConditionsResources/Security_fr.html"
                };
            }
        }        
    }
    public class CustomWebViewDelegate : WKNavigationDelegate
    {
        private WKWebView _view;
        TermsAndConditionsView _parentView;
        public CustomWebViewDelegate(WKWebView view, TermsAndConditionsView parentView)
        {
            _view = view;
            _parentView = parentView;
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            string urlDtr = navigationAction.Request.Url.ToString();
            if (urlDtr.Contains("tel:"))
            {
                UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
                decisionHandler(WKNavigationActionPolicy.Cancel);
            }
            // If an internal link tag is tapped
            else if (urlDtr.Contains(@"file:///") || urlDtr.Contains(@"Containers/Bundle") || urlDtr.Contains(@"containers/Bundle"))
            {
                UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
            else if (urlDtr.Contains("x-apple-data-detectors"))
            {
                UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
                decisionHandler(WKNavigationActionPolicy.Cancel);
            }
            else
            {
                try
                {
                    _parentView.sfView = new SFViewController(navigationAction.Request.Url);
                    _parentView.PresentViewControllerAsync(_parentView.sfView, true);
                    _parentView.controllerDelegate = (SFViewControllerDelegate)_parentView.sfView.Delegate;
                    _parentView.controllerDelegate.RefreshEvent += DoRefresh;
                    decisionHandler(WKNavigationActionPolicy.Cancel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (UIApplication.SharedApplication.CanOpenUrl(navigationAction.Request.Url))
                    {
                        UIApplication.SharedApplication.OpenUrl(navigationAction.Request.Url);
                        decisionHandler(WKNavigationActionPolicy.Cancel);
                    }
                    else
                        decisionHandler(WKNavigationActionPolicy.Cancel);
                }
            }
        }
        private void DoRefresh(object sender, EventArgs args)
        {
            _parentView.SetButtonState(_parentView._selectedButtonIndex);
            _parentView.LoadHtmlFile(_parentView._filePaths[_parentView._selectedButtonIndex], !_parentView._model.NoPhoneAlteration, !_parentView._model.NoClaimAlteration, false);

            _parentView.View.SetNeedsLayout();
            _parentView.controllerDelegate.RefreshEvent -= DoRefresh;
        }
    }
}