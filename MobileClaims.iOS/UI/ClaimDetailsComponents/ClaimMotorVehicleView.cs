using System;

namespace MobileClaims.iOS
{
	public class ClaimMotorVehicleView : CollapseableClaimComponent
	{
		public ClaimDetailsDatePickerSubComponent DateOfMotorVehicleAccidentSelect;

		public ClaimMotorVehicleView ()
		{


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			headingTitleLabel.Text = "motorVehicleTitle".tr();

			DateOfMotorVehicleAccidentSelect = new ClaimDetailsDatePickerSubComponent( this, "motorVehicleDate".tr());
			toggleStage.AddSubview (DateOfMotorVehicleAccidentSelect);
			subComponentsArray.Add (DateOfMotorVehicleAccidentSelect);
		}

		protected override void HandleSwitch (object sender, EventArgs e)
		{
			base.HandleSwitch (sender, e);
		}
			
	}
}

