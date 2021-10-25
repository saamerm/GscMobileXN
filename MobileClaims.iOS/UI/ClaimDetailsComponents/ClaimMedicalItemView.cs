 using System;
 using MobileClaims.Core.ViewModels;

 namespace MobileClaims.iOS
{
	public class ClaimMedicalItemView : CollapseableClaimComponent
	{
		public ClaimDetailsDatePickerSubComponent DateOfReferral;
		public ClaimDetailsListSubComponent TypeOfMedicalProfessionalSelect;
		ClaimDetailsViewModel _model;

		public ClaimMedicalItemView (ClaimDetailsViewModel model)
		{
			_model = model;

		}


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			openWithOff = false;

			headingTitleLabel.Text = "medicalItemTitle".tr();

			DateOfReferral = new ClaimDetailsDatePickerSubComponent (this, "medicalItemReferralDate".tr(), true);
			toggleStage.AddSubview (DateOfReferral);
			subComponentsArray.Add (DateOfReferral);
			DateOfReferral.DateSet += HandleDateSet;
			DateOfReferral.DateCleared += HandleDateCleared;

			TypeOfMedicalProfessionalSelect = new ClaimDetailsListSubComponent (this, "medicalItemProfessional".tr(), true);
			toggleStage.AddSubview (TypeOfMedicalProfessionalSelect);
			subComponentsArray.Add (TypeOfMedicalProfessionalSelect);
			TypeOfMedicalProfessionalSelect.ItemSet += HandleItemSet;;
			TypeOfMedicalProfessionalSelect.ItemCleared += HandleItemCleared;;
		}

		void HandleItemCleared (object sender, EventArgs e)
		{
			if(_model != null)
				_model.ClaimMedicalItemViewModel.TypeOfMedicalProfessional = null;
		}

		void HandleItemSet (object sender, EventArgs e)
		{
			
		}

		void HandleDateCleared (object sender, EventArgs e)
		{
			_model.ClaimMedicalItemViewModel.DateOfReferral = DateTime.MinValue;
		}

		void HandleDateSet (object sender, EventArgs e)
		{
			if (!_model.ClaimMedicalItemViewModel.IsDateOfReferralSetByUser)
            {
                _model.ClaimMedicalItemViewModel.DateOfReferral = DateTime.Now;
            }
		}

		public void setQuestion12()
		{
			if (DateOfReferral != null)
				DateOfReferral.RemoveFromSuperview ();

			if (TypeOfMedicalProfessionalSelect != null)
				TypeOfMedicalProfessionalSelect.RemoveFromSuperview ();

			subComponentsArray.RemoveAllObjects ();

			openWithOff = true;

			headingTitleLabel.Text = "medicalItemSport".tr ();

		}
			
	}
}

