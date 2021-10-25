using System;
using UIKit;

namespace MobileClaims.iOS
{
    public class ClaimsSearchLabel : UILabel
    {
        /// <summary>
        /// copy from claimserviceprovider--byNameLabel
        /// </summary>
        public ClaimsSearchLabel()
        { 
            this.Font = UIFont.FromName (Constants.NUNITO_BLACK, (nfloat)Constants.HEADING_HCSA_FONT_SIZE);
            this.TextColor = Colors.DARK_GREY_COLOR;
            this.TextAlignment = UITextAlignment.Left;
            this.Lines = 0;
            this.LineBreakMode = UILineBreakMode.WordWrap; 
            this.BackgroundColor = Colors.Clear;
           // this.Font = UIFont.FromName (Constants.A, (nfloat)Constants.HEADING_HCSA_FONT_SIZE);
        }
    }
}

