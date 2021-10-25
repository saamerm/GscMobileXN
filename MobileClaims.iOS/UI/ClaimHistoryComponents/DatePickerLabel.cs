using System;
using UIKit;

namespace MobileClaims.iOS 
{
    public class DatePickerLabel : UILabel
    {  
        public DatePickerLabel()
        {
            this.UserInteractionEnabled = true;
            this.Font = UIFont.FromName(Constants.NUNITO_BLACK, (nfloat)Constants.SELECTION_ITEM_FONT_SIZE);
            this.TextAlignment = UITextAlignment.Center;
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0; 
            this.BackgroundColor = Colors.LightGrayColor;
            this.Layer.BorderWidth = 1f;
            this.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;  
        }

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler ShowDatePickerEvent;
        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            if (this.ShowDatePickerEvent != null)
            {
                this.ShowDatePickerEvent(this, EventArgs.Empty);
            }
        } 
    }
}

