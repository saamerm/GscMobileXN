using CoreGraphics;
using UIKit;

namespace MobileClaims.iOS
{
	public class CorrectedTextField : UITextField
	{
		public CorrectedTextField ()
		{

		}
			
		public override CGRect PlaceholderRect (CGRect forBounds)
		{
            CGRect adjustedRect = (CGRect)forBounds;

			if (!Constants.IS_OS_7_OR_LATER ()) {
				adjustedRect.Y += 3;
			}

            return (CGRect)base.PlaceholderRect((CGRect)adjustedRect);
		}
	}
}

