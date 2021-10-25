using System;
using System.ComponentModel;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.UI
{
    [Register("RightAlignIconButton")]
    [DesignTimeVisible(true)]
    public class RightAlignIconButton : UIButton
    {
        public RightAlignIconButton(IntPtr handler)
            : base(handler)
        {
        }

        public RightAlignIconButton()
        {           
        }

        public override void LayoutSubviews()
        {
            Initialize();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        private void Initialize()
        {
            base.LayoutSubviews();         
            HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            TitleEdgeInsets = new UIEdgeInsets(0, (float)-ImageView?.Frame.Width, 0, 0);
            ImageEdgeInsets = new UIEdgeInsets(0, (float)(Bounds.Width - ImageView?.Frame.Width), 0, 0);
        }
    }
}
