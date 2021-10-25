using System;
using Foundation;
using UIKit;
using MobileClaims.Core.ViewModels;
using CoreGraphics;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Platform;
using WebKit;

namespace MobileClaims.iOS.Views
{
    [RemoveAfterViewDidDisappear]
    public class ChangeForLifeView : GSCBaseViewController
    {
        protected ChangeForLifeViewModel _model;
        protected UIScrollView scrollContainer;
        protected UIButton buttBack;
        protected UIButton buttForward;
        protected WKWebView webView;
        UIActivityIndicatorView busy;
        UIView toolbar;
        NSMutableUrlRequest req;
        bool _busy;

        public bool Busy
        {
            get
            {
                return _busy;
            }

            set
            {
                if (_busy != value)
                {
                    if (_busy)
                    {
                        busy.StartAnimating();
                    }
                    else
                    {
                        busy.StopAnimating();
                    }
                    _busy = value;
                }
            }
        }

        string _rsUri;
        public string RsUri
        {
            get { return _rsUri; }
            set
            {
                _rsUri = value;
                LoadWebView();
            }
        }

        float scale = (float)0.0;

        bool PreviousMode = false;
        CGSize previousWebviewSize;

        public ChangeForLifeView()
        {

        }

        #region Create Controls
        #region CreateBackButton
        private void CreateBackButton()
        {
            buttBack = new UIButton();
            buttBack.SetImage(UIImage.FromBundle("ArrowBackGray"), UIControlState.Disabled);
            buttBack.SetImage(UIImage.FromBundle("ArrowBack"), UIControlState.Normal);
            buttBack.TintColor = Colors.HIGHLIGHT_COLOR;
            buttBack.Enabled = false;
            buttBack.TouchUpInside += (sender, e) =>
            {
                if (webView.CanGoBack)
                    webView.GoBack();
            };
        }
        #endregion

        #region CreateToolbar
        private void CreateToolbar()
        {
            CreateBackButton();
            CreateForwardButton();
            toolbar = new UIView();
            toolbar.BackgroundColor = Colors.LightGrayColor;
            toolbar.AddSubview(buttBack);
            toolbar.AddSubview(buttForward);
        }
        #endregion

        #region CreateForwardButton
        private void CreateForwardButton()
        {
            buttForward = new UIButton();
            buttForward.SetImage(UIImage.FromBundle("ArrowForwardGray"), UIControlState.Disabled);
            buttForward.SetImage(UIImage.FromBundle("ArrowForward"), UIControlState.Normal);
            buttForward.Enabled = false;
            buttForward.TintColor = Colors.HIGHLIGHT_COLOR;
            buttForward.TouchUpInside += (sender, e) =>
            {
                if (webView.CanGoForward)
                    webView.GoForward();
            };
        }
        #endregion

        #region CreateWebView
        private void CreateWebView()
        {

            webView = new WKWebView(View.Bounds, new WKWebViewConfiguration());
            webView.ScrollView.ScrollEnabled = true;

            webView.BackgroundColor = Colors.BACKGROUND_COLOR;
            webView.ScrollView.BackgroundColor = Colors.BACKGROUND_COLOR;
            
            webView.SizeToFit();
            var javaScript = @"
            var meta = document.createElement('meta');
            meta.setAttribute('name', 'viewport');
            meta.setAttribute('content', 'width=device-width');
            document.getElementsByTagName('head')[0].appendChild(meta);
            ";
            var jS = new NSString(javaScript);
            webView.EvaluateJavaScript(jS, (NSObject result, NSError error) => Console.WriteLine("SUCCESSS"));

            #region setup event handlers 
            CustomWebViewDelegate webvDelegate = new CustomWebViewDelegate(webView, this);
            webView.NavigationDelegate = webvDelegate;
            #endregion
        }
        #endregion

        #region CreateBusyIndicator
        private void CreateBusyIndicator()
        {
            busy = new UIActivityIndicatorView();
            busy.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            busy.HidesWhenStopped = true;

        }
        #endregion
        #endregion

        #region Layout and Constraints
        #region CreateLayoutConstraints
        private void CreateLayoutConstraints()
        {
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            toolbar.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            //View.TranslatesAutoresizingMaskIntoConstraints = false;
            toolbar.TranslatesAutoresizingMaskIntoConstraints = false;
            //webView.TranslatesAutoresizingMaskIntoConstraints = false;
            //webView.ScrollView.TranslatesAutoresizingMaskIntoConstraints = false;
            toolbar.RemoveConstraints(toolbar.Constraints);
            webView.RemoveConstraints(webView.Constraints);
            webView.ScrollView.RemoveConstraints(webView.ScrollView.Constraints);
            View.RemoveConstraints(View.Constraints);

            if (UIScreen.MainScreen.NativeBounds.Height == 2436 && Helpers.IsInLandscapeMode())
            {
                View.AddConstraints(buttBack.WithSameLeft(toolbar)
                                   , buttBack.Width().EqualTo(44f)
                                   , buttBack.Height().EqualTo(44f)
                                   , buttBack.WithSameCenterY(toolbar)
                                   , buttForward.WithSameCenterY(buttBack)
                                   , buttForward.ToRightOf(buttBack, 44f)
                                   , buttForward.Width().EqualTo(44f)
                                   , buttForward.Height().EqualTo(44f)
                                , toolbar.AtTopOf(View, 20)
                                , toolbar.AtLeftOf(View, 44)
                                , toolbar.AtRightOf(View, 44)
                                , toolbar.WithSameWidth(webView)
                                 , toolbar.WithSameHeight(buttBack)
                                 , webView.Below(toolbar)
                                , webView.AtLeftOf(View, 44)
                                , webView.AtRightOf(View, 44)
                                 , webView.WithSameBottom(View).Minus(Helpers.BottomNavHeight())
                                 , busy.WithSameCenterX(webView)
                                 , busy.WithSameCenterY(webView)

                              );
            }
            else
            {
                View.AddConstraints(buttBack.WithSameLeft(toolbar)
                                  , buttBack.Width().EqualTo(44f)
                                  , buttBack.Height().EqualTo(44f)
                                  , buttBack.WithSameCenterY(toolbar)
                                  , buttForward.WithSameCenterY(buttBack)
                                  , buttForward.ToRightOf(buttBack, 44f)
                                  , buttForward.Width().EqualTo(44f)
                                  , buttForward.Height().EqualTo(44f)
                                , toolbar.AtTopOf(View, 36)
                                , toolbar.WithSameLeft(View)
                                , toolbar.WithSameWidth(View)
                                , toolbar.WithSameHeight(buttBack)
                                , webView.Below(toolbar)
                                , webView.AtLeftOf(View)
                                , webView.WithSameRight(View)
                                , webView.WithSameBottom(View).Minus(Helpers.BottomNavHeight())
                                , busy.WithSameCenterX(webView)
                                , busy.WithSameCenterY(webView)

                             );

            }

        }
        #endregion
        #endregion

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
            }
            catch (Exception ex)
            {
                object ext = ex;
            }
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            CreateToolbar();
            CreateWebView();
            CreateBusyIndicator();
            _model = (ChangeForLifeViewModel)this.ViewModel;
            View.AddSubview(toolbar);
            View.AddSubview(webView);
            View.AddSubview(busy);
            CreateLayoutConstraints();

            busy.Center = this.View.Center;

            var set = this.CreateBindingSet<ChangeForLifeView, ChangeForLifeViewModel>();
            set.Bind(this).For(v => v.Busy).To(vm => vm.Busy);
            set.Bind(this).For(v => v.RsUri).To(vm => vm.Uri);
            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            busy.StartAnimating();
        }
        private void LoadWebView()
        {
            if (string.IsNullOrEmpty(_model?.C4LOAuth?.RS_URI))
                req = new NSMutableUrlRequest(new NSUrl("https://google.com"));
            else
                req = new NSMutableUrlRequest(new NSUrl(_model.C4LOAuth.RS_URI));
            NSDictionary myHeader = req.Headers;
            var headerString = "Bearer " + _model.C4LOAuth.AccessToken;
            var keys = new object[] { "Authorization" };
            var objects = new object[] { headerString };
            var dictionnary = NSDictionary.FromObjectsAndKeys(objects, keys);
            req.Headers = dictionnary;
            webView.LoadRequest(req);
        }

        public override void ViewDidLayoutSubviews()
        {
            try
            {
                //SetFrames ();
            }
            catch (Exception ex)
            {
                MvxTrace.Trace(MvxTraceLevel.Error, ex.ToString());
            }

            //SetFrames ();
            base.ViewDidLayoutSubviews();
            //SetFrames();
            CreateLayoutConstraints();
        }



        private void SetFrames()
        {
            if (View.Superview == null)
                return;
            if (View == null || View.Superview == null)
            {
                return;
            }
            float TOP_PADDING = Constants.IsPhone() ? 0.0f : 20.0f;
            float heightToRemove = 0.0f;
            if (Helpers.IsInPortraitMode())
                heightToRemove = Constants.IsPhone() ? 19.0f : 0.0f;
            float viewWidth = (float)View.Superview.Frame.Width;
            float viewHeight = (float)View.Superview.Frame.Height - Helpers.BottomNavHeight();
            if (scrollContainer == null)
                scrollContainer = new UIScrollView();
            scrollContainer.Frame = new CGRect(0, 0, viewWidth, viewHeight);
            toolbar.Frame = new CGRect(0, TOP_PADDING, viewWidth, 40);
            webView.Frame = new CGRect(0, toolbar.Frame.GetMaxY(), viewWidth, viewHeight - (toolbar.Frame.GetMaxY() + heightToRemove));

            webView.SizeToFit();
            var javaScript = @"
            var meta = document.createElement('meta');
            meta.setAttribute('name', 'viewport');
            meta.setAttribute('content', 'width=device-width');
            document.getElementsByTagName('head')[0].appendChild(meta);
            ";
            var jS = new NSString(javaScript);
            webView.EvaluateJavaScript(jS, (NSObject result, NSError error) => Console.WriteLine("SUCCESSS"));

            //SetwebviewScale ();
            busy.Center = this.View.Center;

        }

        public class CustomWebViewDelegate : WKNavigationDelegate
        {
            private WKWebView _view;
            ChangeForLifeView _parentView;
            public CustomWebViewDelegate(WKWebView view, ChangeForLifeView parentView)
            {
                _view = view;
                _parentView = parentView;
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                System.Console.WriteLine("Starting load");
                decisionHandler(WKNavigationActionPolicy.Allow);
            }

            public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
            {
               _parentView.busy.StartAnimating();
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                _parentView.buttBack.Enabled = webView.CanGoBack;
                _parentView.buttForward.Enabled = webView.CanGoForward;
                _parentView.busy.StopAnimating();
            }

            public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
            {
                _parentView.busy.StopAnimating();
            }
        }
    }
}