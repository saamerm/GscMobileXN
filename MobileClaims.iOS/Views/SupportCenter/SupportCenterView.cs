using System;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.SupportCenter
{
    [RemoveAfterViewDidDisappear]
    public partial class SupportCenterView : GSCBaseViewController, IWebViewSupportedViewController
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

        public SupportCenterView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            SupportCenterWebViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
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

            SupportCenterWebView.ScrollView.ScrollEnabled = true;
            SupportCenterWebView.BackgroundColor = Colors.BACKGROUND_COLOR;
            SupportCenterWebView.SizeToFit();
            var javaScript = @"
            var meta = document.createElement('meta');
            meta.setAttribute('name', 'viewport');
            meta.setAttribute('content', 'width=device-width');
            document.getElementsByTagName('head')[0].appendChild(meta);
            ";
            var jS = new NSString(javaScript);
            SupportCenterWebView.EvaluateJavaScript(jS, (NSObject result, NSError error) => Console.WriteLine("SUCCESSS"));

            SupportCenterWebView.NavigationDelegate = new CustomWebViewDelegate<SupportCenterView>(this);

            BusyIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            BusyIndicator.HidesWhenStopped = true;

            SetBindings();
        }

        private void SetBindings()
        {
            var setBindings = this.CreateBindingSet<SupportCenterView, SupportCenterViewModel>();
            setBindings.Bind(this).For(x => x.Uri).To(vm => vm.Uri);
            setBindings.Apply();
        }

        private void LoadWebView()
        {
#if GSC
            if (string.IsNullOrEmpty(Uri))
                Uri = BrandResource.SupportCenterLink;
#endif
            var req = NSUrlRequest.FromUrl(new NSUrl(Uri));
            SupportCenterWebView.LoadRequest(req);
        }

        private void ForwardButton_TouchUpInside(object sender, EventArgs e)
        {
            if (SupportCenterWebView.CanGoForward)
            {
                SupportCenterWebView.GoForward();
            }
        }

        private void BackButton_TouchUpInside(object sender, EventArgs e)
        {
            if (SupportCenterWebView.CanGoBack)
            {
                SupportCenterWebView.GoBack();
            }
        }
    }
}