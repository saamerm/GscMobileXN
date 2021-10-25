using UIKit;
using Foundation;
using System.Collections.Generic;
using System;
using MobileClaims.iOS;
using MobileClaims.Core.Entities;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Binding.Views;

public class MvxLimitationsTableViewSource : MvxTableViewSource
{
    public List<ClaimPlanLimitationGSC> _limitationObj;
    public Type cellType;
    public String cellName;
    public bool _isEligibility;

    public MvxLimitationsTableViewSource(List<ClaimPlanLimitationGSC> limitationObj, UITableView tableView, String cellName, Type cellType, bool isEligibility = false)
        : base(tableView)
    {
        this.cellType = cellType;
        this.cellName = cellName;
        _limitationObj = limitationObj;
        _isEligibility = isEligibility;
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

    public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
    {
        float totalHeight = 0;
        float contentWidth = (float)tableView.Frame.Width;
        float fieldPadding = 25;
        float innerPadding = 10;
        float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

        var titleLabelWidth = Constants.IsPhone() ? contentWidth * 0.55f : contentWidth * 0.45f;
        var fieldLabelWidth = Constants.IsPhone() ? contentWidth * 0.45f : contentWidth * 0.55f;

        UILabel sizingLabel = new UILabel();
        sizingLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoLabelFontSize);
        sizingLabel.TextAlignment = UITextAlignment.Left;
        sizingLabel.LineBreakMode = UILineBreakMode.WordWrap;
        sizingLabel.ClipsToBounds = false;
        sizingLabel.Lines = 0;

        sizingLabel.Text = _limitationObj[indexPath.Row].BenefitDescription;
        sizingLabel.Frame = new CGRect(sidePadding, 0, titleLabelWidth - sidePadding, sizingLabel.Frame.Height);
        sizingLabel.SizeToFit();

        float tempHeight = (float)sizingLabel.Frame.Height;

        sizingLabel.Text = _limitationObj[indexPath.Row].LimitationDescription;
        sizingLabel.Frame = new CGRect(sidePadding, 0, fieldLabelWidth - sidePadding - innerPadding, sizingLabel.Frame.Height);
        sizingLabel.SizeToFit();
        totalHeight += Math.Max((float)sizingLabel.Frame.Height, tempHeight) + fieldPadding;

        sizingLabel.Text = "startDate".tr();
        sizingLabel.Frame = new CGRect(sidePadding, 0, titleLabelWidth - sidePadding, sizingLabel.Frame.Height);
        sizingLabel.SizeToFit();
        totalHeight += (float)sizingLabel.Frame.Height + fieldPadding;

        sizingLabel.Text = "particpantFamily".tr();
        sizingLabel.Frame = new CGRect(sidePadding, 0, titleLabelWidth - sidePadding, sizingLabel.Frame.Height);
        sizingLabel.SizeToFit();

        if (!_isEligibility || _limitationObj[indexPath.Row].IsParticipantFamilyLabelVisibleForEligibility)
        {
            totalHeight += (float)sizingLabel.Frame.Height + fieldPadding;
        }

        sizingLabel.Text = "amountToDate".tr();
        sizingLabel.Frame = new CGRect(sidePadding, 0, titleLabelWidth - sidePadding, sizingLabel.Frame.Height);
        sizingLabel.SizeToFit();


        if (_isEligibility)
        {

            float numRows = 0;

            if (_limitationObj[indexPath.Row].IsAccumAmountUsedVisibleForEligibility)
                numRows++;

            if (_limitationObj[indexPath.Row].IsAccumUnitsUsedVisibleForEligibility)
                numRows++;

            totalHeight += (numRows * ((float)sizingLabel.Frame.Height + fieldPadding));

        }
        else
        {
            bool twoRows;

            if (_limitationObj[indexPath.Row].AccumAmountUsed > 0 && _limitationObj[indexPath.Row].AccumUnitsUsed <= 0)
            {

                twoRows = false;

            }
            else if (_limitationObj[indexPath.Row].AccumUnitsUsed > 0 && _limitationObj[indexPath.Row].AccumAmountUsed <= 0)
            {

                twoRows = false;

            }
            else
            {
                twoRows = true;
            }

            totalHeight += (twoRows ? 2 : 1) * ((float)sizingLabel.Frame.Height);//+ fieldPadding); 
        }


        return (nfloat)totalHeight + 25f;
    }

    public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
    {
        var cell = this.GetCell(tableView, indexPath);
        var showDeleteButton = true;

        if (cell.GetType().GetMethod("ShowsDeleteButton") != null)
        {
            showDeleteButton = ((MvxDeleteTableViewCell)cell).ShowsDeleteButton();
        }

        if (showDeleteButton)
        {
            return UITableViewCellEditingStyle.Delete;
        }
        else
        {
            return UITableViewCellEditingStyle.None;
        }
    }

    public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
    {
        return false;
    }
}
