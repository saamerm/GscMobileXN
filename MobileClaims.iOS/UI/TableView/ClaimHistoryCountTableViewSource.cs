using System;
using UIKit;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;


namespace MobileClaims.iOS
{
    public class ClaimHistoryCountTableViewSource<T> : MvxTableViewSource 
	{ 
        public NSIndexPath SelectedRow;
		public Type Value {get;private set;} 
		 
        public ClaimHistoryCountTableViewSource(UITableView tableView)
			: base(tableView) 
		{ 
			Value = typeof (T);  
			tableView.RegisterClassForCellReuse(Value, new Foundation.NSString(Value.Name));
            //tableView.RegisterClassForCellReuse(typeof(ItemTableViewCell), new Foundation.NSString("ItemTableViewCell"));
		} 
		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        { 
            switch (Value.Name)
            {
                case "ClaimHistoryCountTableViewCell" :
                    return  (ClaimHistoryCountTableViewCell)tableView.DequeueReusableCell(Value.Name); 
                    break ;
//                case Constants .ItemNoPlusSignTableViewCell :
//                    return (ItemNoPlusSignTableViewCell)tableView.DequeueReusableCell(Value.Name);
//                    break ; 
                default :
                    return new UITableViewCell ();
            }
        } 

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return Constants.ClaimHistoryCountTableViewCellHeight;
        }

        /// <summary>
        /// Wills the display.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="cell">Cell.</param>
        /// <param name="indexPath">Index path.</param>
        /// added in this method to fix the issue of the default selected not show up when view appear
        public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if (cell.GetType() == typeof(ClaimHistoryCountTableViewCell))
            {    
                ClaimHistoryCountTableViewCell c = cell as ClaimHistoryCountTableViewCell;   
				if (SelectedRow != null && SelectedRow.Row == indexPath.Row && SelectedRow.Section == indexPath.Section) {     
					c.IsCellSelected = true;
					cell.SetSelected (true, false);  
				} else {
					c.IsCellSelected = false;
					c._itemButton.IsButtSelected = false; 
					c.SetHighlight(); 
					cell.SetSelected (false, false);
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
            if (tableView.VisibleCells[indexPath.Row] is  ClaimHistoryCountTableViewCell)
            {
                ClaimHistoryCountTableViewCell c = tableView.VisibleCells[indexPath.Row] as ClaimHistoryCountTableViewCell;  
                c.IsCellSelected = true; 
                c._itemButton.IsButtSelected = true; 
                c.SetHighlight();
                tableView.VisibleCells[indexPath.Row].SetSelected(true, false);

                if (SelectedRow != null)
                {
                    ClaimHistoryCountTableViewCell deselectedCell;
                    // We should only access a row if it is within the array values, or else apply the change just for the first one
                    if (SelectedRow.Row >= tableView.VisibleCells.Length)
                        deselectedCell = tableView.VisibleCells[0] as ClaimHistoryCountTableViewCell;
                    else
                        deselectedCell = tableView.VisibleCells[SelectedRow.Row] as ClaimHistoryCountTableViewCell;
                    if ((int)indexPath.Row != SelectedRow.Row)
                    { 
                        deselectedCell.IsCellSelected = false; 
                        deselectedCell._itemButton.IsButtSelected = false; 
                        deselectedCell.SetHighlight();
                        // We should only access a row if it is within the array values, or else apply the change just for the first one
                        if (SelectedRow.Row >= tableView.VisibleCells.Length)
                            tableView.VisibleCells[0].SetSelected(false, false);
                        else
                            tableView.VisibleCells[SelectedRow.Row].SetSelected(false, false);
                    }
                } 
                int newRow = indexPath.Row; 
                SelectedRow = NSIndexPath.FromRowSection((nint)newRow, (nint)0); 
            }
            base.RowSelected(tableView, indexPath);
        } 
    }
}

