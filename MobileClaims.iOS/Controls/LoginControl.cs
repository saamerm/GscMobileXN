using System;
using System.Drawing;

using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MupApps.MvvmCross.Plugins.ControlsNavigation.Touch;

namespace MobileClaims.iOS.Controls
{
	[Register("LoginControl")]
	public class LoginControl : MvxTouchControl
    {
		public LoginControl() : base(null,null)
		{
			Initialize();
		}
        private RectangleF _frame;
        public RectangleF Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
                this.View.Frame = _frame;
            }
        }
        void Initialize()
        {
			this.View.BackgroundColor = UIColor.Red;
            UILabel ScreenLabel = new UILabel();
            ScreenLabel.Text = "Hello from Drug Lookup By DIN!";
            ScreenLabel.Frame = new RectangleF(40,40,400,40);
            this.Add(ScreenLabel);
        }
    }
}