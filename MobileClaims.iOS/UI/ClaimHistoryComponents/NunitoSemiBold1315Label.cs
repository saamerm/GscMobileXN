using System;
using UIKit;

namespace MobileClaims.iOS 
{
    public class NunitoSemiBold1315Label: UILabel
    {
        public NunitoSemiBold1315Label()
        {
            this.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.FONT_SIZE_13_15);
            this.TextAlignment = UITextAlignment.Left; 
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.TextColor = Colors.DARK_GREY_COLOR; 
        }
    }
}

