using UIKit;

namespace MobileClaims.iOS
{
    public class ErrorButton : UIButton
    {
        public ErrorButton()
        {   
            this.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            this.AdjustsImageWhenHighlighted = true;
        }
    }
}

