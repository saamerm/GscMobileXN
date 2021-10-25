using System;
using Foundation;
using UIKit;
using System.ComponentModel;

namespace MobileClaims.iOS.UI
{
    [Register("ExpandButton"), DesignTimeVisible(true)]
    public class ExpandButton : UIButton
    {
        public ExpandButton(IntPtr handle) : base(handle)
        {

        }

        public ExpandButton()
        {

        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            this.TouchUpInside += OnTouchUpInside;
            SetImage(UIImage.FromBundle("ArrowUp"), UIControlState.Selected);
            SetImage(UIImage.FromBundle("ArrowDown"), UIControlState.Normal);
        }

        private void OnTouchUpInside(object sender, EventArgs e)
        {
            this.Selected = !this.Selected;
        }
    }
}
