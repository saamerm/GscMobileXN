using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MobileClaims.Core.ViewModels;
using System;
using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;

namespace MobileClaims.Droid.Views.Fragments
{
    [Region(Resource.Id.phone_main_region, false, BackstackTypes.FIRST_ITEM)]
    public class SureHealthFragment_ : BaseFragment
    {
        private WebView _webView;
        private SureHealthViewModel _model;

		public ImageView _imgBack;
		public ImageView _imgForward;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.SureHealthView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            var _webViewClient = new SureHealthWebViewClient();
            _webViewClient._sH = this;
            _model = (SureHealthViewModel)ViewModel;
            try
            {
                _webView = Activity.FindViewById<WebView>(Resource.Id.webview);

                _webView.SetWebViewClient(_webViewClient);

                _webView.Settings.JavaScriptEnabled = true;
                _webView.Settings.UseWideViewPort = true;
                _webView.Settings.LoadWithOverviewMode = true;
                _webView.Settings.BuiltInZoomControls = true;
                _webView.Settings.SetSupportZoom(true);
                _webView.SetInitialScale(1);

                _imgBack = Activity.FindViewById<ImageView>(Resource.Id.webviewBack_button);
                _imgBack.Click += _buttBack_Click;

                _imgForward = Activity.FindViewById<ImageView>(Resource.Id.webviewGo_button);
                _imgForward.Click += _buttForward_Click;

                var set = this.CreateBindingSet<SureHealthFragment_, SureHealthViewModel>();
                set.Bind(this).For(v => v.RsUri).To(vm => vm.Uri);
                set.Bind(this).For(v => v.Busy).To(vm => vm.Busy);
                set.Apply();
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        private string _rsUri;
        public string RsUri 
        {
            get => _rsUri;
            set
            {
                _rsUri = value;
                LoadWebContent();
            }
        }

        private bool _busy;
        public bool Busy
        {
            get => _busy;
            set 
            {
                if(_busy != value)
                {
                    if(_busy)
                    {
                        Mvx.IoCProvider.Resolve<IUserDialogs>().ShowLoading();
                    }
                    else
                    {
                        Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                    }
                }
                _busy = value;
            }
        }

        private void LoadWebContent()
        {
            _webView.LoadUrl(_rsUri);
        }

        private void _buttForward_Click(object sender, EventArgs e)
        {      
            _webView.GoForward();
        }

        private void _buttBack_Click(object sender, EventArgs e)
        {   
            _webView.GoBack();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
        }
    }

    public class SureHealthWebViewClient : WebViewClient
    {
        public SureHealthFragment_ _sH;

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            return false;
        }
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            return false;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            if (view.CanGoBack())
            {
                _sH._imgBack.SetImageResource(Resource.Drawable.back_active);
            }
            else
            {
                _sH._imgBack.SetImageResource(Resource.Drawable.back_inactive);
            }

            if (view.CanGoForward())
            {
                _sH._imgForward.SetImageResource(Resource.Drawable.forward_active);
            }
            else
            {
                _sH._imgForward.SetImageResource(Resource.Drawable.forward_inactive);
            }

            Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
        }
    }

}