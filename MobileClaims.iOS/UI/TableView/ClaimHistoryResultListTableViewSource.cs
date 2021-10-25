using System;
using UIKit;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;


namespace MobileClaims.iOS
{
    public class ClaimHistoryResultListTableViewSource<T> : MvxTableViewSource 
    {   
        public NSIndexPath SelectedRow;
        public Type Value {get;private set;} 

        public ClaimHistoryResultListTableViewSource(UITableView tableView)
            : base(tableView) 
        { 
            Value = typeof (T);  
            tableView.RegisterClassForCellReuse(Value, new Foundation.NSString(Value.Name));
            //tableView.RegisterClassForCellReuse(typeof(ItemTableViewCell), new Foundation.NSString("ItemTableViewCell"));
        } 

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        {        
            return  (ClaimHistoryResultListTableViewCell)tableView.DequeueReusableCell(Value.Name);   
        } 

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            return 90f;
        }
//        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
//        {
//            float result = 0f; 
//            if (this.ItemsSource != null)
//            {
//                ObservableCollection<ClaimState > kk = this.ItemsSource as ObservableCollection <ClaimState>;
//            } 
//            int currentRow = indexPath.Row; 
//            if (this.ItemsSource != null)
//            {
//                ObservableCollection<ClaimState > RowItemCollection = this.ItemsSource as ObservableCollection<ClaimState >;
//                if (RowItemCollection.Count > 0)
//                {   
//                    for (int i = 0; i < RowItemCollection.Count; i++)
//                    {
//                        if (currentRow == i)
//                        {
//                            ClaimState subCollection = RowItemCollection[i] as ClaimState;
//                            int subRows = 0;
//                            if (subCollection.SpendingAccounts != null)
//                            {
//                                subRows = subCollection.SpendingAccounts.Count;
//                            } 
//                            result = (subRows * 150 + 100); 
//                        }
//                    }
//
//                }   
//            }        
//            return (nfloat)result;
//        } 

        /// <summary>
        /// Wills the display.
        /// </summary>
        /// <param name="tableView">Table view.</param>
        /// <param name="cell">Cell.</param>
        /// <param name="indexPath">Index path.</param>
        /// added in this method to fix the issue of the default selected not show up when view appear
        public override void WillDisplay (UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if (cell.GetType() == typeof(ClaimHistoryResultListTableViewCell))
            {    
                ClaimHistoryResultListTableViewCell c = cell as ClaimHistoryResultListTableViewCell;   
                if (SelectedRow != null && SelectedRow.Row == indexPath.Row && SelectedRow.Section == indexPath.Section)
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
            if (tableView.VisibleCells[indexPath.Row] is  ClaimHistoryResultListTableViewCell)
            { 
                tableView.VisibleCells[indexPath.Row].SetSelected(true, false); 
                if (SelectedRow != null)
                {   
                    tableView.VisibleCells[SelectedRow.Row].SetSelected(false, false);  
                } 
                int newRow = indexPath.Row; 
                SelectedRow = NSIndexPath.FromRowSection((nint)newRow, (nint)0);  
            }
            base.RowSelected(tableView, indexPath);
        }  

    }
}

