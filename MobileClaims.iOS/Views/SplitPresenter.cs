using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;

namespace MobileClaims.iOS.Views
{
    public class SplitPresenter : MvxBaseTouchViewPresenter
    {
        private SplitViewController _svc;
        public SplitPresenter(UIWindow window)
        {
            _svc = new SplitViewController();
            window.RootViewController = _svc;
        }
        public override void Show(Cirrious.MvvmCross.ViewModels.MvxViewModelRequest request)
        {
            base.Show(request);
        }
        
    }
}