using System;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimResultCountTxt : UILabel
    {  
        public ClaimResultCountTxt()
        {
            this.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.HEADING_HCSA_FONT_SIZE);
            this.TextAlignment = UITextAlignment.Left; 
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.TextColor = Colors.Black;
        }
    }
}

