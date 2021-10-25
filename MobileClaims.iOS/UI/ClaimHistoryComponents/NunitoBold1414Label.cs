using System;
using UIKit;

namespace MobileClaims.iOS
{
    public class NunitoBold1414Label: UILabel
    {  
        public NunitoBold1414Label()
        {
            this.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE);
            this.TextAlignment = UITextAlignment.Left; 
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.BackgroundColor = Colors.Clear;
        }
    }
}

