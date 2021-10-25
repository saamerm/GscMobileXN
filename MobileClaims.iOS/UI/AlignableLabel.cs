using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("AlignableLabel")]
    [DesignTimeVisible(true)]
    public class AlignableLabel : UILabel
    {
        public AlignableLabel(IntPtr handler)
            : base(handler)
        {
        }

        public AlignableLabel()
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();            
        }

        public override void DrawText(CGRect rect)
        {
            var newRect = new CGRect(rect.X, rect.Y, rect.Width, rect.Height);
            var fittingSize = SizeThatFits(rect.Size);

            switch (ContentMode)
            {
                case UIViewContentMode.Top:
                    newRect = new CGRect(newRect.X, 0, newRect.Width, fittingSize.Height);
                    break;
                case UIViewContentMode.Bottom:
                    var y = Math.Max(0, rect.Size.Height - fittingSize.Height);
                    var height = Math.Min(rect.Size.Height, fittingSize.Height);
                    newRect = new CGRect(newRect.X, y, newRect.Width, height);
                    break;
            }
            base.DrawText(newRect);
        }
    }
}