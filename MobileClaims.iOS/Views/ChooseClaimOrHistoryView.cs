using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.COP;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public partial class ChooseClaimOrHistoryView : GSCBaseViewController
    {
        private ChooseClaimOrHistoryViewModel _viewModel;

        public ChooseClaimOrHistoryView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ChooseClaimOrHistoryViewModel)ViewModel;

            NavigationController.NavigationBarHidden = false;
            NavigationItem.Title = "myclaims".tr();
            NavigationItem.SetHidesBackButton(true, false);

            ActiveClaimsLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            ActiveClaimsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.PARTICPAINT_RESULTS_FONT_SIZE);
			ActiveClaimsTableView.ContentInset = new UIEdgeInsets(0, 0, Constants.Bottom / 2 + 8, 0);
			ActiveClaimsTableView.ScrollIndicatorInsets = new UIEdgeInsets(0, 0, Constants.Bottom / 2, 0);
            ActiveClaimsTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            NoActiveClaimsLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_FONT_SIZE);

			SetBindings();
            if (!Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                if (Helpers.IsInPortraitMode())
                {
                    ClaimButtonViewTopConstraint.Constant = 80;
                }
                else
                {
                    ClaimButtonViewTopConstraintLandscape.Constant = 60;
                }
            }
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            ActiveClaimsTableView.ReloadData();

            if (!Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                if (fromInterfaceOrientation.IsPortrait())
                {
                    ClaimButtonViewTopConstraintLandscape.Constant = 60;
                }
                else
                {
                    ClaimButtonViewTopConstraint.Constant = 80;
                }
            }
        }

        private void SetBindings()
        {
            var activeClaimsSource = new ActiveClaimsSource(ActiveClaimsTableView, _viewModel);
            ActiveClaimsTableView.Source = activeClaimsSource;

            var set = this.CreateBindingSet<ChooseClaimOrHistoryView, ChooseClaimOrHistoryViewModel>();

            set.Bind(this.ClaimHistoryButton).To(vm => vm.ClaimsHistoryCommand);
            set.Bind(this.SubmitClaimButton).To(vm => vm.ClaimCommand);
            set.Bind(activeClaimsSource).To(vm => vm.COPClaims);
            set.Bind(activeClaimsSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectActiveClaimCommand);

            set.Bind(ActiveClaimsLabel).To(vm => vm.ActiveClaims);
            set.Bind(NoActiveClaimsLabel).To(vm => vm.NoActiveClaims);
            set.Bind(SubmitClaimButton).For("Title").To(vm => vm.ClaimsCommandLabel);
            set.Bind(ClaimHistoryButton).For("Title").To(vm => vm.ClaimsHistoryLabel);

            BoolOppositeValueConverter boolOppositeValueConverter = new BoolOppositeValueConverter();
            set.Bind(ActiveClaimsTableView).For(x => x.Hidden).To(vm => vm.AreAnyActiveClaims).WithConversion(boolOppositeValueConverter, null);
            set.Bind(NoActiveClaimsLabel).For(x => x.Hidden).To(vm => vm.AreAnyActiveClaims);

            set.Apply();
            ActiveClaimsTableView.ReloadData();
        }

        public class ActiveClaimsSource : MvxSimpleTableViewSource
        {
            private readonly ChooseClaimOrHistoryViewModel _viewModel;

            public ActiveClaimsSource(UITableView tableView, ChooseClaimOrHistoryViewModel viewModel)
                : base(tableView, ConfirmationOfPaymentCellView.Key, ConfirmationOfPaymentCellView.Key)
            {
                _viewModel = viewModel;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var item = GetItemAt(indexPath);
                var cell = GetOrCreateCellFor(tableView, indexPath, item);

                var bindableCell = cell as IMvxDataConsumer;
                if (bindableCell != null)
                {
                    bindableCell.DataContext = item;
                }

                var documentListCell = (ConfirmationOfPaymentCellView)cell;
                return cell;
            }

            protected override object GetItemAt(NSIndexPath indexPath)
            {
                return _viewModel.COPClaims[indexPath.Section];
            }

            protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
            {
                var existingCell = (ConfirmationOfPaymentCellView)tableView.DequeueReusableCell(ConfirmationOfPaymentCellView.Key);
                if (existingCell != null)
                {
                    return existingCell;
                }

                return new ConfirmationOfPaymentCellView();
            }

            public override nint NumberOfSections(UITableView tableView)
            {
                return _viewModel.COPClaims.Count;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return 1;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 108.0f;
            }

            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                return 15f;
            }

            public override UIView GetViewForHeader(UITableView tableView, nint section)
            {
                UIView view = new UIView
                {
                    BackgroundColor = Colors.Clear
                };
                return view;
            }
        }
    }
}