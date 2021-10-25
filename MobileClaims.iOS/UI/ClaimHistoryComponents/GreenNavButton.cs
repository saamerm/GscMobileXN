using System;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
    public class GreenNavButton : UIButton
    {
        private bool isHighlightText;
        //this button copy from GrayNavButton, but change background color to green and text color to white when selected  
        public GreenNavButton (bool highlightTextOnly)
        {
            isHighlightText = highlightTextOnly;
            
            this.BackgroundColor = Colors.LightGrayColor;
            this.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            this.SetTitleColor (Colors.DARK_GREY_COLOR, UIControlState.Normal);

            this.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            this.TitleLabel.TextAlignment = UITextAlignment.Center;
            if (!isHighlightText)
            {
                this.TitleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.GS_SELECTION_BUTTON);
            }
            else
            {
                this.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, (nfloat)14f);// (nfloat)Constants.TEXT_FIELD_SUB_HEADING_SIZE);
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

            setHighlight (this.Selected);
        }

        public override void TouchesCancelled (NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled (touches, evt);
            setHighlight (this.Selected);
        }

        public override bool Selected {
            get {
                return base.Selected;
            }
            set {
                base.Selected = value;

                setHighlight (value);

            }
        }

        protected void setHighlight(bool highlighted)
        {
            if (isHighlightText) 
            {
                if (highlighted)
                {
                    this.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal); 
                }
                else
                {
                    this.SetTitleColor(Colors.DARK_GREY_COLOR, UIControlState.Normal); 
                }
            }
            else
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
        }


        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();

        }

    }
}

