using UIKit;
using Foundation;
using System;
using MobileClaims.iOS;
using MobileClaims.Core.ViewModels;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Binding.Views;

public class MvxResultsTableViewSource : MvxTableViewSource
{
	public ClaimSubmissionResultViewModel _model;
	public Type cellType;
	public String cellName;
	public MvxResultsTableViewSource(ClaimSubmissionResultViewModel viewModel, UITableView tableView, String cellName, Type cellType) : base(tableView)
	{
		this.cellType = cellType;
		this.cellName = cellName;
		_model = viewModel;
		tableView.RegisterClassForCellReuse(cellType, new NSString(cellName));
	}

	public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
	{
		return true;
	}

	protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
	{
		return tableView.DequeueReusableCell(cellName);
	}

	public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
	{
		float detailsHeight = 0;
		int i = indexPath.Row;

		float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
		float fieldPadding = 20;

		UILabel sizingLabel = new UILabel();
		sizingLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
		sizingLabel.TextAlignment = UITextAlignment.Left;
		sizingLabel.LineBreakMode = UILineBreakMode.WordWrap;
		sizingLabel.Lines = 0;

        switch (_model.Claim.Results[i].ResultTypeID)
        {
            case 1:
                sizingLabel.Text = "spouseResults".tr();
                sizingLabel.Frame = new CGRect(sidePadding, 0, (float)tableView.Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
                sizingLabel.SizeToFit();
                detailsHeight += (float)sizingLabel.Frame.Height + fieldPadding;
                break;
            case 2:
                if (!string.IsNullOrEmpty(_model.Claim.Results[i].SpendingAccountModelName))
                {
                    sizingLabel.Text = "spendingAccountResults".tr() + "-" + _model.Claim.Results[i].SpendingAccountModelName.ToUpper();
                }
                else
                {
                    sizingLabel.Text = "spendingAccountResults".tr() + _model.Claim.Results[i].AwaitingPaymentMessage;
                }
                sizingLabel.Text = "spendingAccountResults".tr() + _model.Claim.Results[i].AwaitingPaymentMessage;
                sizingLabel.Frame = new CGRect(sidePadding, 0, (float)tableView.Frame.Width, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
                sizingLabel.SizeToFit();
                detailsHeight += (float)sizingLabel.Frame.Height + fieldPadding;
                break;
            default:
                break;
        }

		detailsHeight += Constants.CLAIM_RESULTS_INITIAL_HEIGHT;
        string lang= System.Globalization .CultureInfo.CurrentCulture.Name .ToString();
        bool isFrench = false;
        if (lang.Contains("fr") || lang.Contains("Fr"))
        {
            if (Constants.IsPhone())
            {
                if (Helpers.IsInPortraitMode())
                {
                    isFrench = true;
                }
            }
        }
        if (!isFrench)
            detailsHeight -= 40;

		for (var ii = 0; ii < _model.Claim.Results[i].ClaimResultDetails.Count; ii++) {
			detailsHeight += Constants.CLAIM_RESULTS_DETAILS_HEIGHT;
			detailsHeight += _model.Claim.Results[i].ClaimResultDetails[ii].EOBMessages.Count * 40;
		}

		if (_model.Claim.Results [i].IsPlanLimitationVisible) {

			//Limitation label
			//detailsHeight += 45;  
            
			float fieldLimitationPadding = 25;

			for (var l = 0; l < _model.Claim.Results[i].PlanLimitations.Count; l++) {

				float totalHeight = 0;
				float numLabels = 0;
				float contentWidth = (float)tableView.Frame.Width;
				float innerPadding = 10;

				sizingLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
				sizingLabel.TextAlignment = UITextAlignment.Left;
				sizingLabel.LineBreakMode = UILineBreakMode.WordWrap;
				sizingLabel.ClipsToBounds = false;
				sizingLabel.Lines = 0;

				sizingLabel.Text = _model.Claim.Results[i].PlanLimitations [l].BenefitDescription;
				sizingLabel.Frame = new CGRect (sidePadding, 0, contentWidth /2, (float)sizingLabel.Frame.Height);
				sizingLabel.SizeToFit ();

				float tempHeight = (float)sizingLabel.Frame.Height;

				sizingLabel.Text = _model.Claim.Results[i].PlanLimitations [l].LimitationDescription;
				sizingLabel.Frame = new CGRect (sidePadding, 0, contentWidth /2, (float)sizingLabel.Frame.Height);
				sizingLabel.SizeToFit ();

				totalHeight += (Math.Max ((float)sizingLabel.Frame.Height, tempHeight) + fieldLimitationPadding);

				sizingLabel.Text = "particpantFamily".tr();
				sizingLabel.Frame = new CGRect (sidePadding, 0, contentWidth /2, (float)sizingLabel.Frame.Height);
				sizingLabel.SizeToFit ();
				totalHeight += (float)sizingLabel.Frame.Height + fieldLimitationPadding;

				sizingLabel.Text = "startDate".tr();
				sizingLabel.Frame = new CGRect (sidePadding, 0, contentWidth /2, (float)sizingLabel.Frame.Height);
				sizingLabel.SizeToFit ();
				totalHeight += (float)sizingLabel.Frame.Height + fieldLimitationPadding;

				sizingLabel.Text = "amountToDate".tr ();
				sizingLabel.Frame = new CGRect (sidePadding, 0, contentWidth /2, (float)sizingLabel.Frame.Height);
				sizingLabel.SizeToFit ();


				bool twoRows;

				if (_model.Claim.Results[i].PlanLimitations [l].AccumAmountUsed > 0 && _model.Claim.Results[i].PlanLimitations [l].AccumUnitsUsed <= 0) {

					twoRows = false;

				} else if (_model.Claim.Results[i].PlanLimitations [l].AccumUnitsUsed > 0 && _model.Claim.Results[i].PlanLimitations [l].AccumAmountUsed <= 0) {

					twoRows = false;

				} else {
					twoRows = true;
				}

				totalHeight += (twoRows ? 2 : 1) * ((float)sizingLabel.Frame.Height + fieldLimitationPadding);

				detailsHeight += totalHeight;

			}

			detailsHeight += fieldLimitationPadding;

		}

		sizingLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
		switch (_model.Claim.Results[i].ResultTypeID) {
		case 1:
			sizingLabel.Text = "awaitingPaymentNote".tr ();
			break;
		case 2:
			sizingLabel.Text = "awaitingPaymentNote2".tr() + _model.Claim.Results[i].AwaitingPaymentMessage + "awaitingPaymentNote3".tr();
			break;
		default:
			sizingLabel.Text = "awaitingPaymentNote".tr();
			break;

		}
		sizingLabel.Frame = new CGRect (sidePadding,0, (float)tableView.Frame.Width - sidePadding*2, (float)sizingLabel.Frame.Height);
		sizingLabel.SizeToFit ();
		sizingLabel.Frame = new CGRect (sidePadding,0, (float)sizingLabel.Frame.Width, (float)sizingLabel.Frame.Height);

		detailsHeight += (float)sizingLabel.Frame.Height + fieldPadding;

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