using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using System.ComponentModel;
using Android.Util;
 

namespace MobileClaims.Droid.Views
{
    public class ExtendedWebView : WebView, INotifyPropertyChanged
    {

        #region ctors
        [Register(".ctor", "(Landroid/content/Context;)V", "")]
        public ExtendedWebView(Context context) : base(context){}
        [Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;)V", "")]
        public ExtendedWebView(Context context, IAttributeSet attrs):base(context, attrs){}
        protected ExtendedWebView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer){}
        [Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;I)V", "")]
        public ExtendedWebView(Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle){}
        [Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;IZ)V", "")]
        public ExtendedWebView(Context context, IAttributeSet attrs, int defStyle, bool privateBrowsing) : base(context, attrs, defStyle, privateBrowsing) { }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        private bool _viewCangoback;
        public bool ViewCanGoBack
        {
            get { return _viewCangoback; }
            set 
            {
                _viewCangoback = value;
                if(_viewCangoback )
                {

                    BackButtVisible = ViewStates.Visible;
                }
                else
                {

                    BackButtVisible = ViewStates.Gone;
                }

               // NotifyPropertyChanged("ViewCanGoBack");
            }
        }

        private bool _viewCanGoForward;
        public bool ViewCanGoForward
        {
            get { return _viewCanGoForward; }
            set
            {
                _viewCanGoForward = value;
                if (_viewCanGoForward)
                {

                    ForwardButtVisible = ViewStates.Visible;
                }
                else
                {

                    ForwardButtVisible = ViewStates.Gone;
                }

                // NotifyPropertyChanged("ViewCanGoForward");
            }
        }

        private ViewStates _backButtVisible = ViewStates.Gone;
        public ViewStates BackButtVisible
        {
            get { return _backButtVisible; }
            set
            {
                _backButtVisible = value;
                NotifyPropertyChanged("BackButtVisible");
            }
        }


        private ViewStates _forwardButtVisible = ViewStates.Gone;
        public ViewStates ForwardButtVisible
        {
            get { return _forwardButtVisible; }
            set
            {
                _forwardButtVisible = value;
                NotifyPropertyChanged("ForwardButtVisible");
            }
        }
    }
}