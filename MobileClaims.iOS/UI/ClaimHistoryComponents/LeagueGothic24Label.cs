using System;
using UIKit;

namespace MobileClaims.iOS 
{
    public class LeagueGothic24Label : UILabel
    {
        public LeagueGothic24Label()
        {
            this.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.HEADING_CLAIMTYPES); 
            this.BackgroundColor = Colors.Clear;
            this.LineBreakMode = UILineBreakMode.WordWrap;
            this.Lines = 0;
            this.TextAlignment = UITextAlignment.Left;
        }
    }
}

