using Foundation;
using System;
using UIKit;

namespace MobileClaims.iOS
{
    [Register("GCButtonFixedFontSize")]
    public class GCButtonFixedFontSize : UIButton
    {
        private float TEXT_HEIGHT = Constants.IsPhone() ? 25 : 45;

        public GCButtonFixedFontSize(IntPtr handle) : base(handle) { }

        public GCButtonFixedFontSize()
        {
            Initialize();
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            setHighlight(true);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            setHighlight(false);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            setHighlight(false);
        }

        protected void setHighlight(bool highlighted)
        {
            if (highlighted)
            {
                this.BackgroundColor = Colors.LightGrayColor;
                this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            }
            else
            {
                this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);
            }
        }

        public float buttonSizeWithText()
        {
            float padding = Constants.BUTTON_TEXT_PADDING;
            float finalHeight = 0;

            this.TitleLabel.SizeToFit();

            finalHeight = (float)this.TitleLabel.Frame.Height;

            return finalHeight;
        }

        public override void AwakeFromNib()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);

            this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            this.TitleLabel.TextAlignment = UITextAlignment.Center;
            this.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.GS_BUTTON_FONT_SIZE);
            this.ContentEdgeInsets = new UIEdgeInsets(3, 0, 0, 0);
        }
    }
}