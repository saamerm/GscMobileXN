using System;

namespace MobileClaims.iOS
{
	public class ClaimOtherBenefitsHCSAView : CollapseableClaimComponent
	{
        public string Title=string.Empty;
		public ClaimOtherBenefitsHCSAView ()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            headingTitleLabel.Text = (!string.IsNullOrEmpty (Title))? Title:"otherBenefitsHSCA".tr();
			headingTitleLabel.Lines = 5;
		}

		protected override void HandleSwitch (object sender, EventArgs e)
		{
			base.HandleSwitch (sender, e);
		}
			
	}
}

