using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("RadioButton"), DesignTimeVisible(true)]
    public class RadioButton : UIButton
    {
        public RadioButton(IntPtr handle) : base(handle)
        {

        }

        public RadioButton()
        {

        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TitleLabel.Font = UIFont.FromName("Roboto-Regular", 14.0f);
            TitleEdgeInsets = new UIEdgeInsets(0, 15, 0, 0);
            this.TouchUpInside += OnTouchUpInside;
            SetImage(UIImage.FromBundle("RadioButtonSelected"), UIControlState.Selected);
            SetImage(UIImage.FromBundle("RadioButtonUnselected"), UIControlState.Normal);
            ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            SetTitleColor(Colors.DarkGrayColor, UIControlState.Selected | UIControlState.Normal);
        }

        private void OnTouchUpInside(object sender, EventArgs e)
        {
            this.Selected = true;
        }
    }
}