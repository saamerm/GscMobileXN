using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class ClaimAuditModal : UIViewController
	{
		public UIScrollView scrollContainer;

		public GSButton dismissButton;
		//public GSButton submitAnotherButton;
		public UILabel auditInstructionsTextArea;
        public UILabel auditHeadingLabel;

		private float BUTTON_WIDTH = Constants.IsPhone() ? 270 : 400;
		private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

		public ClaimAuditModal ()
		{

		}

        public override void ViewDidLoad()
        {           
            View = new GSCBaseView() { BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();

            scrollContainer = ((GSCBaseView)View).baseScrollContainer;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

            scrollContainer.AddSubview(auditHeadingLabel);
            scrollContainer.AddSubview(auditInstructionsTextArea);

        }
        public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
            if (auditHeadingLabel == null)
                auditHeadingLabel = new UILabel();

            float viewwidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height;

			float innerPadding = 20;

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                scrollContainer.Frame = new CGRect(View.SafeAreaInsets.Left, View.SafeAreaInsets.Top, viewwidth, viewHeight);
            } else {
                scrollContainer.Frame = new CGRect(0, innerPadding, viewwidth, viewHeight);

            }

			float auditControllerYPos = innerPadding *2;


			if (auditInstructionsTextArea != null && dismissButton != null) {
			
                auditHeadingLabel.Frame = new CGRect(0, auditControllerYPos, viewwidth, innerPadding);

                auditControllerYPos += (float)auditHeadingLabel.Frame.Height + innerPadding;

				auditInstructionsTextArea.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, auditControllerYPos, viewwidth - Constants.DRUG_LOOKUP_SIDE_PADDING*2, (float)auditInstructionsTextArea.Frame.Height);
				auditInstructionsTextArea.SizeToFit ();

				auditControllerYPos += (float)auditInstructionsTextArea.Frame.Height + innerPadding;

				dismissButton.Frame = new CGRect (viewwidth/2 - BUTTON_WIDTH/2, auditControllerYPos, BUTTON_WIDTH, BUTTON_HEIGHT);
				auditControllerYPos += (float)(dismissButton.Frame.Height + innerPadding);

//				if (submitAnotherButton != null) {
//					submitAnotherButton.Frame = new RectangleF (viewwidth/2 - BUTTON_WIDTH/2, auditControllerYPos, BUTTON_WIDTH, BUTTON_HEIGHT);
//					auditControllerYPos += submitAnotherButton.Frame.Height + innerPadding;
//				}

			}

			scrollContainer.ContentSize = new CGSize (viewwidth, auditControllerYPos);
		}
	}
}

