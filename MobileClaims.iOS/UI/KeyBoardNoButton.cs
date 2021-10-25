using UIKit;
namespace MobileClaims.iOS
{
    public class KeyBoardNoButton: UIButton
    {
        public KeyBoardNoButton()
        {
            this.UserInteractionEnabled = true;
            this.SetTitleColor(Colors.Black, UIControlState.Normal);
            this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            this.Layer.BorderWidth = 1f; 
        }
    }
}

