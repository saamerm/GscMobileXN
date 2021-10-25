using System;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.SureHealth
{
    [RemoveAfterViewDidDisappear]
    public partial class SureHealthView : GSCBaseViewController, IWebViewSupportedViewController
    {
        private bool _isBusy;
        private string _uri;

        public UIActivityIndicatorView ActivityIndicatorView => this.BusyIndicator;
        public UIButton Back => this.BackButton;
        public UIButton Forward => this.ForwardButton;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                if (_isBusy)
                {
                    BusyIndicator.StartAnimating();
                }
                else
                {
                    BusyIndicator.StopAnimating();
                }
            }
        }

        public string Uri
        {
            get => _uri;
            set
            {
                _uri = value;
                LoadWebView();
            }
        }

        public SureHealthView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            SureHealthWebViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
            NavigationContainerViewTopConstraint.Constant = Constants.IS_OS_VERSION_OR_LATER(11, 0) ? 0 : 20;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = true;
            NavigationItem.SetHidesBackButton(true, false);

            NavigationContainerView.BackgroundColor = Colors.LightGrayColor;

            BackButton.SetImage(UIImage.FromBundle("ArrowBackGray"), UIControlState.Disabled);
            BackButton.SetImage(UIImage.FromBundle("ArrowBack"), UIControlState.Normal);
            BackButton.Enabled = false;
            BackButton.TouchUpInside += BackButton_TouchUpInside;

            ForwardButton.SetImage(UIImage.FromBundle("ArrowForwardGray"), UIControlState.Disabled);
            ForwardButton.SetImage(UIImage.FromBundle("ArrowForward"), UIControlState.Normal);
            ForwardButton.Enabled = false;
            ForwardButton.TouchUpInside += ForwardButton_TouchUpInside;

            SureHealthWebView.ScrollView.ScrollEnabled = true;
            SureHealthWebView.BackgroundColor = Colors.BACKGROUND_COLOR;
            SureHealthWebView.SizeToFit();
            var javaScript = @"
            var meta = document.createElement('meta');
            meta.setAttribute('name', 'viewport');
            meta.setAttribute('content', 'width=device-width');
            document.getElementsByTagName('head')[0].appendChild(meta);
            ";
            var jS = new NSString(javaScript);
            SureHealthWebView.EvaluateJavaScript(jS, (NSObject result, NSError error) => Console.WriteLine("SUCCESSS"));

            SureHealthWebView.NavigationDelegate = new CustomWebViewDelegate<SureHealthView>(this);

            BusyIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            BusyIndicator.HidesWhenStopped = true;

            SetBindings();
        }

        private void SetBindings()
        {
            var setBindings = this.CreateBindingSet<SureHealthView, SureHealthViewModel>();
            setBindings.Bind(this).For(x => x.Uri).To(vm => vm.Uri);
            setBindings.Apply();
        }

        private void LoadWebView()
        {
            var req = NSUrlRequest.FromUrl(new NSUrl(Uri));
            SureHealthWebView.LoadRequest(req);
        }

        private void ForwardButton_TouchUpInside(object sender, EventArgs e)
        {
            if (SureHealthWebView.CanGoForward)
            {
                SureHealthWebView.GoForward();
            }
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            if (SureHealthWebView.CanGoBack)
            {
                SureHealthWebView.GoBack();
            }
        }
    }    
}