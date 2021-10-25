using System;
using System.Collections.Generic;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.CellTemplates;
using MobileClaims.iOS.UI;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public partial class HealthProviderTypeListView : GSCBaseViewController
    {
        private HealthProviderTypeViewModel _selectedProvider;
        private HealthProviderTypeListViewModel _viewModel;

        public HealthProviderTypeViewModel SelectedProvider
        {
            get => _selectedProvider;
            set
            {
                _selectedProvider = value;
                tableView.ReloadData();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            headerLabel.Font = UIFont.FromName("LeagueGothic", 22.0f);
            headerLabel.Text = "REFINE YOUR SEARCH".tr();

            var isPhone = Constants.IsPhone();
            if (!isPhone)
            {
                bottomTableViewSafeAreaMarginConstraint.Constant = Constants.NAV_BUTTON_SIZE_IPAD;
            }

            tableView.TableFooterView = new UIView(CoreGraphics.CGRect.Empty);
            _viewModel = (HealthProviderTypeListViewModel) ViewModel;

            var source =
                new ExpandableTableSource<HealthProviderTypeViewModel>(_viewModel.HealthProviderTypeList, tableView);
            tableView.Source = source;

            var set = this.CreateBindingSet<HealthProviderTypeListView, HealthProviderTypeListViewModel>();
            set.Bind(backButton).To(vm => vm.GoBackCommand);
            set.Bind(SelectedProvider).To(vm => vm.ViewModelParameter).Mode(MvxBindingMode.OneWay);
            set.Apply();
        }
    }

    public class ExpandableTableSource<T> : UITableViewSource
    {
        private readonly List<HealthProviderTypeViewModel> TableItems;
        private bool[] _isSectionOpen;
        private EventHandler _expandAction;
        private EventHandler _selectAction;

        public ExpandableTableSource(List<HealthProviderTypeViewModel> items, UITableView tableView)
        {
            TableItems = items;
            _isSectionOpen = new bool[items.Count];

            tableView.RegisterNibForCellReuse(HealthProviderTypeChildViewCell.Nib, HealthProviderTypeChildViewCell.Key);
            tableView.RegisterNibForHeaderFooterViewReuse(HealthProviderTypeParentViewCell.Nib,
                HealthProviderTypeParentViewCell.Key);

            _expandAction = (sender, e) =>
            {
                var button = (UIButton) sender;
                var section = button.Tag;
                var expanded = !_isSectionOpen[(int) section];
                _isSectionOpen[(int) section] = expanded;
                tableView.ReloadData();

                var paths = new NSIndexPath[RowsInSection(tableView, section)];

                for (var i = 0; i < paths.Length; i++)
                {
                    paths[i] = NSIndexPath.FromItemSection(i, section);
                }

                tableView.ReloadRows(paths, UITableViewRowAnimation.None);
            };

            _selectAction = (sender, e) =>
            {
                var button = (UIButton) sender;
                var section = button.Tag;
                var model = TableItems[(int) section];
                if (model.IsSearchable == false)
                {
                    _expandAction.Invoke(sender, e);
                    return;
                }

                model.SelectHealthProviderTypeCommand.Execute(model);
            };
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return TableItems.Count;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _isSectionOpen[(int) section] ? TableItems[(int) section].ChildItems.Count : 0;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 65f;
        }

        public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section)
        {
            return 65f;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header =
                tableView.DequeueReusableHeaderFooterView(HealthProviderTypeParentViewCell.Key) as
                    HealthProviderTypeParentViewCell;
            var model = TableItems[(int) section];
            header.TitleLabel.Text = model.Title;
            header.ProviderTypeImage.ImagePath = model.ImageUrl;

            header.ExpandButton.Hidden = model.ChildItems.Count == 0;
            header.ExpandButton.TouchUpInside -= _expandAction;
            header.ExpandButton.Selected = _isSectionOpen[(int) section];
            header.CheckedImage.Hidden = !model.IsSelected;
            header.TitleLabel.TextColor =
                model.IsSelected ? Colors.HIGHLIGHT_COLOR : Colors.HealthProviderTypeTextNotSelectedColor;

            if (!header.ExpandButton.Hidden)
            {
                header.ExpandButton.Tag = section;
                header.ExpandButton.TouchUpInside += _expandAction;
            }

            header.SelectButton.Tag = section;
            header.SelectButton.TouchUpInside -= _selectAction;
            header.SelectButton.TouchUpInside += _selectAction;

            return header;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell =
                tableView.DequeueReusableCell(HealthProviderTypeChildViewCell.Key, indexPath) as
                    HealthProviderTypeChildViewCell;
            var model = TableItems[indexPath.Section].ChildItems[indexPath.Row];

            cell.CheckedImage.Hidden = !model.IsSelected;
            cell.TitleLabel.Text = model.Title;
            cell.TitleLabel.TextColor =
                model.IsSelected ? Colors.HIGHLIGHT_COLOR : Colors.HealthProviderTypeTextNotSelectedColor;
            cell.ProviderTypeImage.ImagePath = model.ImageUrl;
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.CellAt(indexPath) as HealthProviderTypeChildViewCell;
            cell.CheckedImage.Hidden = false;

            var model = TableItems[indexPath.Section].ChildItems[indexPath.Row];
            model.SelectHealthProviderTypeCommand.Execute(model);
        }
    }
}