using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
    public class ClaimListModal : UIViewController
    {
        public UITableView tableView;
        public GSButton dismissButton;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        public ClaimListModal()
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            float viewwidth;
            float viewHeight;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                viewwidth = (float)base.View.Bounds.Width - (float)base.View.SafeAreaInsets.Right * 2;
                viewHeight = (float)base.View.Bounds.Height - (float)base.View.SafeAreaInsets.Bottom - Helpers.BottomNavHeight();
            }
            else
            {
                viewwidth = (float)base.View.Bounds.Width;
                viewHeight = (float)base.View.Bounds.Height - Helpers.BottomNavHeight();
            }

            View.BackgroundColor = Colors.BACKGROUND_COLOR;

            float startY = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;

            if (Constants.IsPhone() && dismissButton != null)
            {
                if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    dismissButton.Frame = new CGRect((float)base.View.SafeAreaInsets.Left + viewwidth / 2 - BUTTON_WIDTH / 2,
                                                      (float)tableView.Frame.Y + (float)tableView.Frame.Height,
                                                      BUTTON_WIDTH,
                                                      BUTTON_HEIGHT);
                }
                else
                {
                    dismissButton.Frame = new CGRect(viewwidth / 2 - BUTTON_WIDTH / 2, viewHeight - BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT);
                }
            }
           
            if (Constants.IsPhone() && tableView != null && dismissButton != null)
            {
                if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    tableView.Frame = new CGRect((float)base.View.SafeAreaInsets.Left + Constants.SINGLE_SELECTION_TOP_TABLE_PADDING,
                                                 startY,
                                                 viewwidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING * 2,
                                                 viewHeight - BUTTON_HEIGHT - startY);

                    dismissButton.Frame = new CGRect((float)base.View.SafeAreaInsets.Left + viewwidth / 2 - BUTTON_WIDTH / 2,
                                                     (float)tableView.Frame.Y + (float)tableView.Frame.Height,
                                                     BUTTON_WIDTH,
                                                     BUTTON_HEIGHT);
                }
                else
                {
                    tableView.Frame = new CGRect(0, startY, viewwidth, viewHeight - BUTTON_HEIGHT - startY);
                    dismissButton.Frame = new CGRect(viewwidth / 2 - BUTTON_WIDTH / 2, (float)tableView.Frame.Y + (float)tableView.Frame.Height, BUTTON_WIDTH, BUTTON_HEIGHT);
                }
            }
        }
    }
}

