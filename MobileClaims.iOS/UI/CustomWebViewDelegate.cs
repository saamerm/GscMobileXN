using System;
using Foundation;
using UIKit;
using WebKit;

namespace MobileClaims.iOS.UI
{
    public class CustomWebViewDelegate<T> : WKNavigationDelegate
     where T : IWebViewSupportedViewController
    {
        private T _parentView;

        public CustomWebViewDelegate(T parentView)
        {
            _parentView = parentView;
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            var url = navigationAction.Request.Url;
            if (true) //Whatever your test happens to be
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
            else
            {
                decisionHandler(WKNavigationActionPolicy.Cancel);
            }
        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            _parentView.ActivityIndicatorView.StartAnimating();
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            _parentView.ActivityIndicatorView.StopAnimating();
            _parentView.Back.Enabled = webView.CanGoBack;
            _parentView.Forward.Enabled = webView.CanGoForward;
        }

        public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            _parentView.ActivityIndicatorView.StopAnimating();
        }
    }
}
