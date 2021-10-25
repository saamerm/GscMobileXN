using UIKit;
using Foundation;
using System.Collections.Generic;
using System;
using MobileClaims.iOS;
using MobileClaims.Core.Entities;
using MvvmCross.Platforms.Ios.Binding.Views;

public class MvxResultsDetailsTableViewSource : MvxTableViewSource
{

	public List<ClaimResultDetailGSC> _detailObj;
	public Type cellType;
	public String cellName;

	#region Constructors
	public MvxResultsDetailsTableViewSource(List<ClaimResultDetailGSC> detailObj, UITableView tableView, String cellName, Type cellType) : base(tableView)
	{
		this.cellType = cellType;
		this.cellName = cellName;
		_detailObj = detailObj;
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

	public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
	{
		float detailsHeight = 0;
		int i = indexPath.Row;
        float baseHeight = Constants.CLAIM_RESULTS_DETAILS_HEIGHT;
        int eobHeigth = 40;
        string lang= System.Globalization .CultureInfo.CurrentCulture.Name .ToString(); 
        if (lang.Contains("fr") || lang.Contains("Fr"))
        { 
            if (Constants.IsPhone())
            {
                if (Helpers.IsInPortraitMode())
                {
                    eobHeigth = 50;
                }
                else
                {
                    baseHeight -= 10;
                }
            }
        }
        else
        {
            baseHeight -= 10;
        }
        
        detailsHeight = baseHeight + (((List<ClaimResultDetailGSC>)_detailObj)[i].EOBMessages.Count * eobHeigth);
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