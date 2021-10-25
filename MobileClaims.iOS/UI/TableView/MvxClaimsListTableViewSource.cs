using Cirrious.MvvmCross.Binding.Touch.Views;
using UIKit;
using Foundation;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Bindings;
using System;
using MobileClaims.iOS;
using MobileClaims.Core.ViewModels;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels.HCSA;

namespace MobileClaims.iOS
{
	public class MvxClaimsListTableViewSource:MvxTableViewSource
	{
		public ClaimReviewAndEditViewModel _model;
		public Type cellType;
		public String cellName;

//		public MvxClaimsListTableViewSource ()
//		{
//		}

		public MvxClaimsListTableViewSource(ClaimReviewAndEditViewModel viewModel, UITableView tableView, String cellName, Type cellType) : base(tableView)
		{
			this.cellType = cellType;
			this.cellName = cellName;
			_model = viewModel;
			tableView.RegisterClassForCellReuse(cellType, new NSString(cellName));
		}
		#endregion

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			return tableView.DequeueReusableCell(cellName);
		}
		public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
		{
			if(!Constants.IsPhone()){
				ClaimConfirmationIPadTableViewCell myCell = (ClaimConfirmationIPadTableViewCell)cell;
				myCell.reorderCellWithModel(_model);
			}
		}

		//	public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		//	{
		//		var cell = this.GetCell (tableView, indexPath);
		//		var showDeleteButton = true;
		//
		//		if (cell.GetType().GetMethod("ShowsDeleteButton") != null) {
		//			showDeleteButton = ((MvxDeleteTableViewCell)cell).ShowsDeleteButton ();
		//		}
		//
		//		if (showDeleteButton) {
		//			switch (editingStyle) {
		//			case UITableViewCellEditingStyle.Delete:
		//				m_ViewModel.RemoveCommand.Execute (0);
		//				break;
		//			case UITableViewCellEditingStyle.None:
		//				break;
		//			}
		//		}
		//	}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			float detailsHeight = 0;
			float numLabels = 0;
			if (_model.Claim == null) //Special case -> after the claim has been submitted, Model.Claim gets reset to NULL.  This tableview is getting re-rendered on iPhones
				//running iOS 8 and above for some reason, so just bail out by returning 0 for the row height.
				return (nfloat)0f;
			TreatmentDetail currentDetail = _model.Claim.TreatmentDetails [indexPath.Row];

			if (!Constants.IsPhone ()) {

				detailsHeight = 50;

			} else {

				detailsHeight += Constants.CLAIM_CONFIRMATION_DETAILS_TOP_PADDING;

				float topPadding = 15;
				float fieldPadding = 10;
				float yPos = topPadding;
				float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
				float totalSidePadding = sidePadding * 2;

				UILabel titleTestLabel = new UILabel();
				titleTestLabel.Font = UIFont.FromName (Constants.AVENIR_STD_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
				titleTestLabel.TextAlignment = UITextAlignment.Left;
				titleTestLabel.Lines = 0;

				UILabel testLabel = new UILabel();
				testLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
				testLabel.TextAlignment = UITextAlignment.Right;
				testLabel.Lines = 0;

				if (_model.Claim.IsAlternateCarrierPaymentVisible) {

					titleTestLabel.Text = "amountPaidAlt".tr ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width - sidePadding * 2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = "$" + currentDetail.AlternateCarrierPayment.ToString();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);

					detailsHeight +=( Math.Max ((float)titleTestLabel.Frame.Height, (float)testLabel.Frame.Height) + fieldPadding);
				}

				if (_model.Claim.IsItemDescriptionVisible) {
					titleTestLabel.Text = "itemDescription".tr ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width - sidePadding * 2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = currentDetail.ItemDescription.Name;
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding - 20, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)testLabel.Frame.Width, (float)testLabel.Frame.Height);

					detailsHeight +=( Math.Max ((float)titleTestLabel.Frame.Height, (float)testLabel.Frame.Height) + fieldPadding);

				}

				if (_model.Claim.IsTypeOfTreatmentVisible) {

					titleTestLabel.Text = "typeOfTreatmentTitle".tr ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width - sidePadding * 2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = currentDetail.TypeOfTreatmentListViewItem.Name;
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);

					detailsHeight +=( Math.Max ((float)titleTestLabel.Frame.Height, (float)testLabel.Frame.Height) + fieldPadding);

				}

				if (_model.Claim.IsTreatmentDurationVisible)
					numLabels++;

				if (_model.Claim.IsDateOfMonthlyTreatmentVisible || _model.Claim.IsDateOfExaminationVisible || _model.Claim.IsTreatementDateVisible || _model.Claim.IsDateOfPurchaseVisible || _model.Claim.IsPickupDateVisible)
					numLabels++;

				if (_model.Claim.IsQuantityVisible)
					numLabels++;

				if (_model.Claim.IsTreatmentAmountVisible)
					numLabels++;

				if (_model.Claim.IsTotalAmountChargedForMIVisible) {
					titleTestLabel.Text = "totalAmountMedicalItems".tr ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width/2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = "$" +  currentDetail.TreatmentAmount.ToString();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding - 20, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)testLabel.Frame.Width, (float)testLabel.Frame.Height);

					detailsHeight +=( Math.Max ((float)titleTestLabel.Frame.Height, (float)testLabel.Frame.Height) + fieldPadding);
				}

				if (_model.Claim.IsOrthodonticMonthlyFeeVisible)
					numLabels++;

				if (_model.Claim.IsGSTHSTIncludedInTotalVisible)
					numLabels++;

				if (_model.Claim.IsPSTIncludedInTotalVisible)
					numLabels++;

				if (_model.Claim.IsDateOfReferralVisibleForTreatment)
					numLabels++;

				if (_model.Claim.IsTypeOfMedicalProfessionalVisibleForTreatment) {
					titleTestLabel.Text = "typeOfProfessional".tr ();
					//NOTE: Especially long title label. Cutting this one short to force multi line. Same adjustment made in ClaimConfirmationTableViewCell.
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width/2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = currentDetail.TypeOfMedicalProfessional.Name;
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - sidePadding - (float)testLabel.Frame.Width, yPos, (float)testLabel.Frame.Width, (float)testLabel.Frame.Height);

					detailsHeight +=( Math.Max ((float)titleTestLabel.Frame.Height, (float)testLabel.Frame.Height) + fieldPadding);

				}

				if (_model.Claim.IsTypeOfEyewearVisible) {
					titleTestLabel.Text = "typeOfEyewearConf".tr ();
					//NOTE: Especially long title label. Cutting this one short to force multi line. Same adjustment made in ClaimConfirmationTableViewCell.
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width / 2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = currentDetail.TypeOfEyewear.Name;
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - sidePadding - (float)testLabel.Frame.Width, yPos, (float)testLabel.Frame.Width, (float)testLabel.Frame.Height);

					detailsHeight +=  (Math.Max ((float)(titleTestLabel.Frame.Height), (float)(testLabel.Frame.Height)) + fieldPadding);
				}

				if (_model.Claim.IsTypeOfLensVisible) {
					titleTestLabel.Text = "typeOfLensConf".tr ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width / 2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);

					testLabel.Text = currentDetail.TypeOfLens.Name;
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - totalSidePadding - (float)testLabel.Frame.Width, yPos, (float)tableView.Frame.Width - (float)titleTestLabel.Frame.Width - totalSidePadding, (float)testLabel.Frame.Height);
					testLabel.SizeToFit ();
					testLabel.Frame = new CGRect ((float)tableView.Frame.Width - sidePadding - (float)testLabel.Frame.Width, yPos, (float)testLabel.Frame.Width, (float)testLabel.Frame.Height);

					detailsHeight += (Math.Max ((float)titleTestLabel.Frame.Height, (float)testLabel.Frame.Height) + fieldPadding);
				}

				if (_model.Claim.IsFrameAmountVisible)
					numLabels++;

				if (_model.Claim.IsFeeAmountVisible)
					numLabels++;

				if (_model.Claim.IsEyeglassLensesAmountVisible)
					numLabels++;

				if (_model.Claim.IsTotalAmountChargedVisible)
					numLabels++;

				if (_model.Claim.IsPrescriptionDetailsVisible) {

					titleTestLabel.Text = "rightCylinder".tr ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)tableView.Frame.Width - sidePadding * 2, (float)titleTestLabel.Frame.Height);
					titleTestLabel.SizeToFit ();
					titleTestLabel.Frame = new CGRect (0, 0, (float)titleTestLabel.Frame.Width, (float)titleTestLabel.Frame.Height);


					if (_model.Claim.IsRightSphereVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsLeftSphereVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsRightCylinderVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsLeftCylinderVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsRightAxisVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsLeftAxisVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsRightPrismVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsLeftPrismVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsRightBifocalVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);
					if (_model.Claim.IsLeftBifocalVisible)
						detailsHeight += ((float)titleTestLabel.Frame.Height + fieldPadding);

				}

				detailsHeight += (numLabels * Constants.CLAIM_CONFIRMATION_LABEL_HEIGHT);

				detailsHeight += fieldPadding;
			}


			return (nfloat)detailsHeight;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = this.GetCell (tableView, indexPath);
			var showDeleteButton = true;

			if (cell.GetType().GetMethod("ShowsDeleteButton") != null) {
				showDeleteButton = ((MvxDeleteTableViewCell)cell).ShowsDeleteButton ();
			}

			if (showDeleteButton) {
				return UITableViewCellEditingStyle.Delete;
			} else {
				return UITableViewCellEditingStyle.None;
			}
		}

		public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}
	}
}

