using System;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
    public class GSButtonGrayTextUpdate : UIButton
    {

        private float TEXT_HEIGHT = Constants.IsPhone() ? 25 : 45; 
        public GSButtonGrayTextUpdate()
        {

            this.BackgroundColor = Colors.LightGrayColor;//Colors.LightGrayColor ;
//            this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
//            this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            this.SetTitleColor (Colors.DARK_GREY_COLOR, UIControlState.Normal);

            this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            this.TitleLabel.TextAlignment = UITextAlignment.Center;
            this.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)Constants.HEADING_SIZE);//Constants.GS_BUTTON_FONT_SIZE);
            this.ContentEdgeInsets = new UIEdgeInsets (3, 0, 0, 0);
            this.Enabled = true;  
            
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                this.SetTitle(Content, UIControlState.Normal);
            }
        } 


        public override void TouchesBegan (NSSet touches, UIEvent evt)
        {
            base.TouchesBegan (touches, evt);

            setHighlight (true);
        }

        public override void TouchesEnded (NSSet touches, UIEvent evt)
        {
            base.TouchesEnded (touches, evt);

            setHighlight (false);
        }

        public override void TouchesCancelled (NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled (touches, evt);
            setHighlight (false);
        }

        protected void setHighlight(bool highlighted)
        {
            if (highlighted)
            {
                this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                this.SetTitleColor(Colors.BACKGROUND_COLOR, UIControlState.Normal);
            }
            else
            {
                this.BackgroundColor = Colors.LightGrayColor;
                this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal);
            }
        }



        public float buttonSizeWithText()
        {
            float padding = Constants.BUTTON_TEXT_PADDING;
            float finalHeight = 0;

            this.TitleLabel.SizeToFit ();

            finalHeight = (float)this.TitleLabel.Frame.Height;

            return finalHeight;
        }
 
    }
}

