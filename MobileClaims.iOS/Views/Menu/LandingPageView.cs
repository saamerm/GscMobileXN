using System.Threading.Tasks;
using CoreLocation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Menu
{
    public partial class LandingPageView : GSCBaseViewController
    {
        private LandingPageViewModel _viewModel;
        private MenuItemReadOnlySource _menuItemSource;

        public LandingPageView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            MenuTableViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (LandingPageViewModel)ViewModel;
            NavigationController.NavigationBarHidden = true;
            NavigationItem.SetHidesBackButton(true, false);

            MenuTableView.RowHeight = UITableView.AutomaticDimension;
            MenuTableView.EstimatedRowHeight = 500;
            MenuTableView.BackgroundColor = Colors.BACKGROUND_COLOR;

            SetBinding();
            ///OnLocationRequest Invokes when user has CLAuthorization status undetermined.
            ///Property that sets the value is HasMapLocationPermission in viewmodel.
            ///means user did not selected from the map pops up location.
            _viewModel.OnMapLocationRequest += OnLocationRequest;
            CheckLocationPermission();
        }

        private void SetBinding()
        {
            _menuItemSource = new MenuItemReadOnlySource(MenuTableView, _viewModel);
            MenuTableView.Source = _menuItemSource;

            var set = this.CreateBindingSet<LandingPageView, LandingPageViewModel>();
            set.Bind(_menuItemSource).To(vm => vm.DynamicMenuItems);
            set.Bind(_menuItemSource).For(source => source.SelectionChangedCommand).To(vm => vm.OpenMenuItemCommand);
            set.Apply();

            MenuTableView.ReloadData();
        }

        private void OnLocationRequest()
        {
            var locationManager = new CLLocationManager();
            locationManager.RequestWhenInUseAuthorization();
            _viewModel.HasMapLocationPermission = !(CLLocationManager.Status == CLAuthorizationStatus.NotDetermined);
        }

        private void CheckLocationPermission()
        {
            _viewModel.HasMapLocationPermission = !(CLLocationManager.Status == CLAuthorizationStatus.NotDetermined);
        }
    }
}