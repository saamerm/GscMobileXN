using System;

namespace MobileClaims.iOS
{
	public class ClaimWorkInjuryView : CollapseableClaimComponent
	{
		public ClaimDetailsDatePickerSubComponent DateOfWorkRelatedInjurySelect;
		public ClaimDetailsTextFieldSubComponent WorkRelatedInjuryCaseNumberField;

		public ClaimWorkInjuryView ()
		{


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			headingTitleLabel.Text = "workInjuryTitle".tr();

			DateOfWorkRelatedInjurySelect = new ClaimDetailsDatePickerSubComponent (this, "workInjuryDate".tr());
			toggleStage.AddSubview (DateOfWorkRelatedInjurySelect);
			subComponentsArray.Add (DateOfWorkRelatedInjurySelect);

			WorkRelatedInjuryCaseNumberField = new ClaimDetailsTextFieldSubComponent (this, "workInjuryCaseNumber".tr());
			toggleStage.AddSubview (WorkRelatedInjuryCaseNumberField);
			subComponentsArray.Add (WorkRelatedInjuryCaseNumberField);
		}

		protected override void HandleSwitch (object sender, EventArgs e)
		{
			base.HandleSwitch (sender, e);
		}
			
	}
}

