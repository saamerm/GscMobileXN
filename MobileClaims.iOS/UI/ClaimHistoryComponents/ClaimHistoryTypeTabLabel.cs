using System;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimHistoryTypeTabLabel : UILabel
    {
        public ClaimHistoryTypeTabLabel()
        {
            this.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.SELECTION_ITEM_FONT_SIZE);
            this.TextAlignment = UITextAlignment.Center; 
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.TextColor = Colors.DARK_GREY_COLOR; 
            this.BackgroundColor = Colors.Clear; 
        } 
    }
}

