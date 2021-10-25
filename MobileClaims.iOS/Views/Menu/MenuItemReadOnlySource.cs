using System;
using Foundation;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Menu
{
    public class MenuItemReadOnlySource : MvxSimpleTableViewSource
    {
        private readonly LandingPageViewModel _viewModel;

        public MenuItemReadOnlySource(UITableView tableView, LandingPageViewModel viewModel)
            : base(tableView, MenuItemCellView.Key, MenuItemCellView.Key)
        {
            _viewModel = viewModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath) as Core.Entities.MenuItem;
            var cell = GetOrCreateCellFor(tableView, indexPath, item) as MenuItemCellView;
         
            cell.DataContext = item;
            cell.SetConstraints(item.ShouldShowCounter);
            cell.NeedsUpdateConstraints();
            cell.InvalidateIntrinsicContentSize();
            cell.LayoutSubviews();

            return cell;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return _viewModel.DynamicMenuItems[indexPath.Section];
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var existingCell = (MenuItemCellView)tableView.DequeueReusableCell(MenuItemCellView.Key);
            if (existingCell != null)
            {
                return existingCell;
            }

            return new MenuItemCellView();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _viewModel.DynamicMenuItems.Count;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 1;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 5;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIView view = new UIView()
            {
                BackgroundColor = Colors.Clear
            };
            return view;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return Constants.IsPhone() ? Constants.LANDING_BUTTON_HEIGHT_IPHONE : Constants.LANDING_BUTTON_HEIGHT_IPAD;
        }
    }
}