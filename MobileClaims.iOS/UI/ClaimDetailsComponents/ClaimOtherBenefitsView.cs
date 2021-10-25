using System;

namespace MobileClaims.iOS
{
    public class ClaimOtherBenefitsView : CollapseableClaimComponent
	{
		public ClaimDetailsToggleSubComponent IsOtherCoverageGSCToggle;
		public ClaimDetailsToggleSubComponent HasClaimBeenSubmittedToOtherBenefitPlanToggle;
		public ClaimDetailsToggleSubComponent PayAnyUnpaidBalanceThroughOtherGSCPlanToggle;
		public ClaimDetailsTextFieldSubComponent OtherGscNumberField;

		public ClaimOtherBenefitsView ()
		{

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			headingTitleLabel.Text = "otherBenefitsTitle".tr();

			IsOtherCoverageGSCToggle = new ClaimDetailsToggleSubComponent ("otherBenefitsWithGSC".FormatWithBrandKeywords(LocalizableBrand.GSC));
			toggleStage.AddSubview (IsOtherCoverageGSCToggle);
			subComponentsArray.Add (IsOtherCoverageGSCToggle);

			HasClaimBeenSubmittedToOtherBenefitPlanToggle = new ClaimDetailsToggleSubComponent ("otherBenefitsSubmitted".tr());
			toggleStage.AddSubview (HasClaimBeenSubmittedToOtherBenefitPlanToggle);
			subComponentsArray.Add (HasClaimBeenSubmittedToOtherBenefitPlanToggle);
			HasClaimBeenSubmittedToOtherBenefitPlanToggle.VisibilityToggled += HandleClaimBeenSubmitted;

			PayAnyUnpaidBalanceThroughOtherGSCPlanToggle = new ClaimDetailsToggleSubComponent ("otherBenefitsPayBalanceThroughOtherGSC".FormatWithBrandKeywords(LocalizableBrand.GSC));
			toggleStage.AddSubview (PayAnyUnpaidBalanceThroughOtherGSCPlanToggle);
			subComponentsArray.Add (PayAnyUnpaidBalanceThroughOtherGSCPlanToggle);
			PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.VisibilityToggled += HandlePayAnyUnpaidBalance;
			PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.setIsEnabled (true, false);

			OtherGscNumberField = new ClaimDetailsTextFieldSubComponent (this, "otherBenefitsEnterGSC".FormatWithBrandKeywords(LocalizableBrand.GSC));
			toggleStage.AddSubview (OtherGscNumberField);
			subComponentsArray.Add (OtherGscNumberField);
			OtherGscNumberField.setIsEnabled (false, false);
		}

		void HandleClaimBeenSubmitted (object sender, EventArgs e)
		{
			if (HasClaimBeenSubmittedToOtherBenefitPlanToggle.toggleSwitch.On) {
				PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.setIsEnabled (false, true);
				OtherGscNumberField.setIsEnabled (false, true);
			} else {
				PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.setIsEnabled (true, true);
				OtherGscNumberField.setIsEnabled ( PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.toggleSwitch.On ? true : false, true);
			}
		}

		void HandlePayAnyUnpaidBalance (object sender, EventArgs e)
		{
			if (!HasClaimBeenSubmittedToOtherBenefitPlanToggle.toggleSwitch.On && PayAnyUnpaidBalanceThroughOtherGSCPlanToggle.toggleSwitch.On) {
				OtherGscNumberField.setIsEnabled (true, true);
			} else {
				OtherGscNumberField.setIsEnabled (false, true);
			}
		}

		protected override void HandleSwitch (object sender, EventArgs e)
		{
			base.HandleSwitch (sender, e);
		}
			
	}
}

