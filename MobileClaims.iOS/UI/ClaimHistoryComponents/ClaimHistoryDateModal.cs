using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
    public class ClaimHistoryDateModal: UIViewController
    {
        public UIDatePicker datePicker;
        public GSButton dismissButton;

        private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
        private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

        public ClaimHistoryDateModal ()
        {
        }

        public override void ViewDidLayoutSubviews ()
        {
            base.ViewDidLayoutSubviews ();
            BUTTON_WIDTH = BUTTON_WIDTH / 2;

            View.BackgroundColor = Colors.BACKGROUND_COLOR;
            float viewwidth = (float)base.View.Bounds.Width;
            float viewHeight = (float)base.View.Bounds.Height/2;

           // if (Constants.IsPhone () && datePicker != null && dismissButton != null) {
            if (datePicker != null && dismissButton != null) {
              //  dismissButton.Frame = new CGRect (viewwidth / 2 - BUTTON_WIDTH / 2, (float)datePicker.Frame.Y + (float)datePicker.Frame.Height + BUTTON_HEIGHT/2, BUTTON_WIDTH, BUTTON_HEIGHT);
                //datePicker.Frame = new CGRect (viewwidth / 2 - (float)datePicker.Frame.Width / 2, viewHeight/2 - (float)datePicker.Frame.Height/2 - BUTTON_HEIGHT /2, (float)datePicker.Frame.Width, (float)datePicker.Frame.Height);
                dismissButton.Frame = new CGRect (viewwidth  - BUTTON_WIDTH-20,viewHeight, BUTTON_WIDTH, BUTTON_HEIGHT);
                datePicker.Frame = new CGRect (viewwidth / 2 - (float)datePicker.Frame.Width / 2, viewHeight + BUTTON_HEIGHT, (float)datePicker.Frame.Width, (float)datePicker.Frame.Height);
               
            }
        }
    }
}

