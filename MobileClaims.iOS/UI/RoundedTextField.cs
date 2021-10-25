using UIKit;

namespace MobileClaims.iOS
{
	public class RoundedTextField : UITextField
	{
		public RoundedTextField ()
		{
			this.TextAlignment = UITextAlignment.Left;
			this.BackgroundColor = Colors.BACKGROUND_COLOR;
			this.Layer.CornerRadius = Constants.CORNER_RADIUS;
			this.VerticalAlignment = UIControlContentVerticalAlignment.Center;

		}
	}
}

