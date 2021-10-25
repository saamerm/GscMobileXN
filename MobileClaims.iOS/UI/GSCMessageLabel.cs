using System;
using UIKit;

namespace MobileClaims.iOS
{
	public class GSCMessageLabel : UILabel
	{
		public GSCMessageLabel ()
		{
			this.TextColor = Colors.DARK_GREY_COLOR;
			this.BackgroundColor = Colors.Clear;
			this.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
			this.LineBreakMode = UILineBreakMode.WordWrap;
			this.TextAlignment = UITextAlignment.Center;
			this.Lines = 0;

		}
	}
}

