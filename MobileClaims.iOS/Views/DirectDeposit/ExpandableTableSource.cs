using System;
using System.Collections.ObjectModel;
using Foundation;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit
{
    public class ExpandableTableSource : MvxTableViewSource
    {
        private readonly DirectDepositViewModel _viewModel;
        private readonly DirectDepositStep1Model _step1Model;
        private readonly DirectDepositStep2Model _step2Model;
        private readonly DirectDepositStep3Model _step3Model;
        private readonly ObservableCollection<DirectDepositStep> _stepItems;
        private bool[] _isSectionOpen;
        private UITableView _tableView;

        public ExpandableTableSource(UITableView tableView, DirectDepositViewModel viewModel)
            : base(tableView)
        {
            _viewModel = viewModel;
            _step1Model = _viewModel.Step1Model;
            _step2Model = _viewModel.Step2Model;
            _step3Model = _viewModel.Step3Model;

            _stepItems = _viewModel.Steps;
            _isSectionOpen = new bool[_stepItems.Count];

            _tableView = tableView;
            _tableView.RegisterNibForCellReuse(Step1TableViewCell.Nib, Step1TableViewCell.Key);
            _tableView.RegisterNibForCellReuse(Step2TableViewCell.Nib, Step2TableViewCell.Key);
            _tableView.RegisterNibForCellReuse(Step3TableViewCell.Nib, Step3TableViewCell.Key);
            _tableView.RegisterNibForHeaderFooterViewReuse(DirectDepositParentCellView.Nib, DirectDepositParentCellView.Key);
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return _stepItems.Count;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _isSectionOpen[(int)section] ? 1 : 0;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 75f;
        }

        public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section)
        {
            return 75f;
        }

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var header = tableView.DequeueReusableHeaderFooterView(DirectDepositParentCellView.Key)
                as DirectDepositParentCellView;

            var model = _stepItems[(int)section];
            model.ShouldExpandSectionEvent -= (s, e) => Model_ShouldExpandSectionEvent(s, e, header.Button);
            header.Button.TouchUpInside -= SelectRowForExpand;
            header.DataContext = model;
            header.Button.Tag = section;
            model.ShouldExpandSectionEvent += (s, e) => Model_ShouldExpandSectionEvent(s, e, header.Button);
            header.Button.TouchUpInside += SelectRowForExpand;

            if ((int)section == 2)
            {
                header.ShowBottomBorder();
            }
            header.NeedsUpdateConstraints();
            header.InvalidateIntrinsicContentSize();
            header.LayoutSubviews();
            return header;
        }

        private void Model_ShouldExpandSectionEvent(object sender, ShouldExpandSectionEventArgs e, UIButton button)
        {
            var model = _stepItems[e.StepNumber];
            if (model.IsExpanded != e.ShouldExpand)
            {
                SelectRowForExpand(button, new EventArgs());
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 0)
            {
                var cell = GetOrCreateCellFor(tableView, indexPath, _step1Model) as Step1TableViewCell;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.Button.TouchUpInside -= SelectAuthorizeDirectDeposit;
                cell.DataContext = _step1Model;
                cell.Highlight(_step1Model.IsDirectDepositAuthorized);
                cell.Button.TouchUpInside += SelectAuthorizeDirectDeposit;
                return cell;
            }
            else if (indexPath.Section == 1)
            {
                var cell = tableView.DequeueReusableCell(Step2TableViewCell.Key, indexPath) as Step2TableViewCell;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.GoToStep3Button.TouchUpInside -= GoToStep3Button_TouchUpInside;
                cell.DataContext = _step2Model;
                cell.GoToStep3Button.TouchUpInside += GoToStep3Button_TouchUpInside;
                return cell ?? new Step2TableViewCell();
            }
            else if (indexPath.Section == 2)
            {
                var cell = tableView.DequeueReusableCell(Step3TableViewCell.Key, indexPath) as Step3TableViewCell;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.ReceiveNotificationButton.TouchUpInside -= ReceiveNotificationButton_TouchUpInside;
                cell.DoNotReceiveNotificationButto.TouchUpInside -= ReceiveNotificationButton_TouchUpInside;
                cell.DataContext = _step3Model;
                cell.Highlight(_step3Model.IsOptedForNotification);
                cell.ReceiveNotificationButton.TouchUpInside += ReceiveNotificationButton_TouchUpInside;
                cell.DoNotReceiveNotificationButto.TouchUpInside += ReceiveNotificationButton_TouchUpInside;
                return cell ?? new Step3TableViewCell();
            }
            return null;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (indexPath.Section == 0)
            {
                var cell = tableView.DequeueReusableCell(Step1TableViewCell.Key, indexPath) as Step1TableViewCell;
                return cell ?? new Step1TableViewCell();
            }
            else if (indexPath.Section == 1)
            {
                var cell = tableView.DequeueReusableCell(Step2TableViewCell.Key, indexPath) as Step2TableViewCell;
                return cell ?? new Step2TableViewCell();
            }
            else if (indexPath.Section == 2)
            {
                var cell = tableView.DequeueReusableCell(Step3TableViewCell.Key, indexPath) as Step3TableViewCell;
                return cell ?? new Step3TableViewCell();
            }
            return null;
        }

        private void SelectAuthorizeDirectDeposit(object sender, EventArgs e)
        {
            _viewModel.SelectAuthorizeDirectDepositCommand.Execute();
        }

        private void GoToStep3Button_TouchUpInside(object sender, EventArgs e)
        {
            _viewModel.ValidateAndSaveBankingInfoCommand.Execute();
        }

        private void ReceiveNotificationButton_TouchUpInside(object sender, EventArgs e)
        {
            _viewModel.SelectReceiveNotificationCommand.Execute();
        }

        private void SelectRowForExpand(object sender, EventArgs e)
        {
            var button = (UIButton)sender;
            var section = button.Tag;
            var model = _stepItems[(int)section];

            ExpandRow(section);
            model.IsExpanded = !model.IsExpanded;
        }

        private void ExpandRow(nint section)
        {
            var expanded = !_isSectionOpen[(int)section];
            _isSectionOpen[(int)section] = expanded;
            _tableView.ReloadData();

            var paths = new NSIndexPath[RowsInSection(_tableView, section)];

            for (var i = 0; i < paths.Length; i++)
            {
                paths[i] = NSIndexPath.FromItemSection(i, section);
            }

            _tableView.ReloadRows(paths, UITableViewRowAnimation.Automatic);
        }
    }
}