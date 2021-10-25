using System;
using Foundation;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.HealthProvider;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views
{
    [FromBottomTransition]
    public partial class ServiceDetailsListViewController : GSCBaseViewController
    {
        private ServiceDetailsListViewModel _viewModel;

        public UITableView TableView => tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            bool isPhone = Constants.IsPhone();
            float navigationBarHeight = isPhone ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;

            bottomTableViewMarginConstraint.Constant = navigationBarHeight;

            tableView.TableFooterView = new UIView(CoreGraphics.CGRect.Empty);
            _viewModel = (ServiceDetailsListViewModel)ViewModel;
            var source= new TableSource(tableView, _viewModel);
            tableView.Source = source;
            var set = this.CreateBindingSet<ServiceDetailsListViewController, ServiceDetailsListViewModel>();

            set.Bind(source).To(x => x.ViewModelParameter);
            set.Bind(source).For(x => x.SelectedItem).To(vm => vm.SelectedServiceProvider);
            set.Apply();

            HideDetailsList.TouchUpInside += OnHideDetailsListTouchUpInside;
            source.SelectedItemChanged += async (sender, e) => {
                var provider = (HealthProviderSummaryModel)source.SelectedItem;
                if (!Constants.IsPhone())
                {
                    await _viewModel.SelectServiceProvider(provider);
                    _viewModel.CenterMapToProvider.Execute(null);
                }
                await _viewModel.ShowDetails(provider);               
                tableView.DeselectRow(tableView.IndexPathForSelectedRow, false);
            };
        }

        private async void OnHideDetailsListTouchUpInside(object sender, EventArgs e)
        {
            _viewModel.CloseCommand.Execute(null);
            if (Constants.IsPhone())
            {
                await _viewModel.SelectServiceProvider(_viewModel.SelectedServiceProvider);
                _viewModel.CenterMapToProvider.Execute(null);    
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            tableView.ReloadData();
        }

        public class TableSource : MvxSimpleTableViewSource
        {
            private readonly ServiceDetailsListViewModel _viewModel;

            public TableSource(UITableView tableView, ServiceDetailsListViewModel viewModel)
                : base(tableView, HealthProviderCellView.Key, HealthProviderCellView.Key)
            {
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.SingleLine;
                _viewModel = viewModel;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = base.GetCell(tableView, indexPath);
                if(cell is HealthProviderCellView cellHealthProvider){
                    cellHealthProvider.ToggleFavourite = _viewModel.ToggleFavouriteDetails;
                }
                return cell;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 180.0f;
            }
        }
    }
}