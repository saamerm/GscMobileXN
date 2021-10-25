using UIKit;
using Foundation;
using System;
using MobileClaims.iOS;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;

public class MvxDeleteTableViewSource : MvxTableViewSource
{

	public IRemove m_ViewModel;
	public Type cellType;
	public String cellName;
    public NSIndexPath selectedRow;
	#region Constructors
	public MvxDeleteTableViewSource(IRemove viewModel, UITableView tableView, String cellName, Type cellType) : base(tableView)
	{
		this.cellType = cellType;
		this.cellName = cellName;
		m_ViewModel = viewModel;
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

	public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
	{
		var cell = this.GetCell (tableView, indexPath);
		var showDeleteButton = true;

		if (cell.GetType().GetMethod("ShowsDeleteButton") != null) {
			showDeleteButton = ((MvxDeleteTableViewCell)cell).ShowsDeleteButton ();
		}

		if (showDeleteButton) {
			switch (editingStyle) {
			case UITableViewCellEditingStyle.Delete:
				m_ViewModel.RemoveCommand.Execute (indexPath.Row);
				break;
			case UITableViewCellEditingStyle.None:
				break;
			}
		}
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

    /// <summary>
    /// Wills the display.
    /// </summary>
    /// <param name="tableView">Table view.</param>
    /// <param name="cell">Cell.</param>
    /// <param name="indexPath">Index path.</param>
    /// added in this method to fix the issue of the default selected not show up when participant view appear
    public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
    {
        if (cell.GetType() == typeof(ClaimHistoryParticipantCell))
        {    
            ClaimHistoryParticipantCell c = cell as ClaimHistoryParticipantCell;   
            if (selectedRow != null && selectedRow.Row == indexPath.Row && selectedRow.Section == indexPath.Section)
            {     
                cell.SetSelected(true, false);  
            } 
        }
    } 
    /// <summary>
    /// Rows the selected.
    /// </summary>
    /// <param name="tableView">Table view.</param>
    /// <param name="indexPath">Index path.</param>
    /// added in this method to fix the issue of default selected not disapper when trying to select others
    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        if (tableView.VisibleCells[indexPath.Row] is  ClaimHistoryParticipantCell)
        {
            if (selectedRow != null)
            {
                if ((int)indexPath.Row != selectedRow.Row)
                {
                    tableView.VisibleCells[indexPath.Row].SetSelected(true, false);
                    tableView.VisibleCells[selectedRow.Row].SetSelected(false, false); 
                    int newRow = indexPath.Row;
                    selectedRow = NSIndexPath.FromRowSection((nint)newRow, (nint)0);
               
                } 
            }
        }
        base.RowSelected(tableView, indexPath);
    }  
}