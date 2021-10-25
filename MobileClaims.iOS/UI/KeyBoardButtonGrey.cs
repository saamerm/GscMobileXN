using UIKit;

namespace MobileClaims.iOS
{
    public class KeyBoardButtonGrey: UIButton
    {
        public KeyBoardButtonGrey()
        {
            this.UserInteractionEnabled = true;
            this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            this.Layer.BorderWidth = 1f;
            this.BackgroundColor = Colors.LightGrayColor;
            this.SetTitleColor(Colors.Black, UIControlState.Normal);
        }
    }
}

