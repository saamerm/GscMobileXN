using System;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimForTxtLabel : UILabel
    {  
        public ClaimForTxtLabel()
        {
            this.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.GS_SELECTION_BUTTON);
            this.TextAlignment = UITextAlignment.Left; 
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.BackgroundColor = Colors.Clear;
        }
    }
}

