using UIKit;
using Foundation;
using System;
using MobileClaims.iOS;
using MvvmCross.Platforms.Ios.Binding.Views;

public class DismmssVCTableViewSource : MvxTableViewSource
{

	public UIPopoverController _vcToDismiss;
	public Type cellType;
	public String cellName;
    private NSIndexPath _selectedrow;
    public NSIndexPath selectedRow
    {
        get
        {
            return _selectedrow;
        }
        set
        {
            _selectedrow = value;
        }
    }

	#region Constructors
	public DismmssVCTableViewSource(UIPopoverController vcToDismiss, UITableView tableView, String cellName, Type cellType) : base(tableView)
	{
		this.cellType = cellType;
		this.cellName = cellName;
		this._vcToDismiss = vcToDismiss;
		tableView.RegisterClassForCellReuse(cellType, new NSString(cellName));
	}
	#endregion

	protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
	{
		UITableViewCell cell = tableView.DequeueReusableCell (cellName);
		return cell;
	}
	public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
	{
        if (selectedRow != null && selectedRow.Row == indexPath.Row && selectedRow.Section == indexPath.Section)
        { 
            if (cell.GetType() == typeof(TypeOfClaimTableViewCell)
            || cell.GetType() == typeof(TypeOfExpenseTableViewCell)
            || cell.GetType() == typeof(MedicalProfessionalTableCell)
            || cell.GetType() == typeof(ServiceProviderProvinceTableCell))
                cell.SetSelected(true, false);
        }
        else
        {
            cell.SetSelected(false, false);
        }
	}


	public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {   
        /*if (tableView.VisibleCells == null || tableView.VisibleCells.Length == 0)
        {
            return;
        }
        for (int i = 0; i < tableView.VisibleCells.Length; i++)
        {
            if (i != indexPath.Row)
            {
                if (tableView.VisibleCells[0] is TypeOfClaimTableViewCell)
                {
                    TypeOfClaimTableViewCell cell = (TypeOfClaimTableViewCell)(tableView.VisibleCells[i]);
                    if (cell != null)
                        cell.SetSelected(false, false);  
                }
            }
            else
            {
                if (tableView.VisibleCells[0] is TypeOfClaimTableViewCell)
                {
                    TypeOfClaimTableViewCell cell = (TypeOfClaimTableViewCell)(tableView.VisibleCells[i]);
                    if (cell != null)
                        cell.SetSelected(true, false);  
                }
            }
        }*/
        if (selectedRow != null && indexPath.Item == selectedRow.Item)
        {
            return;
        }
        if (selectedRow != null)
        {
            if (tableView.VisibleCells != null && tableView.VisibleCells.Length > 0)
            {
                if (tableView.VisibleCells[0] is TypeOfClaimTableViewCell)
                {
                    TypeOfClaimTableViewCell cell = (TypeOfClaimTableViewCell)(tableView.CellAt(selectedRow));
                    if(cell!=null)
                     cell.SetSelected(false, false);  
                }
            }
        }
        selectedRow = indexPath; 
        base.RowSelected(tableView, indexPath);
        if (!Constants.IsPhone())
        {
            if (_vcToDismiss != null)
                _vcToDismiss.Dismiss(true);

        }
    } 

}
