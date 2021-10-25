using CoreGraphics;
using UIKit;

namespace MobileClaims.iOS
{
	public class TabBar : UITabBarController
	{
		UIViewController leftTab, rightTab;

		public TabBar ()
		{
			int numButtons = 2;
			float borderWidth = 1.0f;
			float barWidth = numButtons * Constants.TAB_BAR_BUTTON_WIDTH;
            var deviceWidth = (float)UIScreen.MainScreen.Bounds.Width;
            var deviceHeight = (float)UIScreen.MainScreen.Bounds.Height;

            this.View.Frame = new CGRect (0, deviceHeight - Constants.TAB_BAR_HEIGHT, deviceWidth, Constants.TAB_BAR_HEIGHT);
			this.View.BackgroundColor = Colors.BACKGROUND_COLOR;

			leftTab = new UIViewController ();
			leftTab.View.BackgroundColor = Colors.BACKGROUND_COLOR;
			leftTab.View.Layer.BorderColor = UIColor.Blue.CGColor;
			leftTab.View.Layer.BorderWidth = borderWidth;
			leftTab.Title = "G";
			leftTab.View.Frame = new CGRect ((float)this.View.Frame.Width / 2 - barWidth / 2 - borderWidth, Constants.TAB_BAR_HEIGHT / 2 - Constants.TAB_BAR_BUTTON_HEIGHT / 2, Constants.TAB_BAR_BUTTON_WIDTH, Constants.TAB_BAR_BUTTON_HEIGHT); 

			rightTab = new UIViewController ();
			rightTab.View.BackgroundColor = Colors.BACKGROUND_COLOR;
			rightTab.View.Layer.BorderColor = UIColor.Blue.CGColor;
			rightTab.View.Layer.BorderWidth = borderWidth;
			rightTab.Title = "I";
			rightTab.View.Frame = new CGRect ((float)leftTab.View.Frame.X + (float)leftTab.View.Frame.Width + borderWidth, Constants.TAB_BAR_HEIGHT / 2 - Constants.TAB_BAR_BUTTON_HEIGHT / 2, Constants.TAB_BAR_BUTTON_WIDTH, Constants.TAB_BAR_BUTTON_HEIGHT);

			var tabs = new UIViewController [] {
				leftTab, rightTab
			};
			ViewControllers = tabs;
		}
	}
}

