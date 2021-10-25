using System;

namespace MobileClaims.iOS
{
	public class ClaimOtherBenefitsGSTView : CollapseableClaimComponent
	{

		public ClaimOtherBenefitsGSTView ()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			headingTitleLabel.Text = "gstIncludedQ".tr();

		}

		protected override void HandleSwitch (object sender, EventArgs e)
		{
			base.HandleSwitch (sender, e);
		}
			
	}
}

