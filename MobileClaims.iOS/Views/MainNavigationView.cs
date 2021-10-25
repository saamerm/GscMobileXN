using System;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views
{
    public partial class MainNavigationView : GSCBaseViewController
    {
        public MainNavigationView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.View.BackgroundColor = Colors.BottomNavBarBackgroundColor;

            SetBottomNavigationButton(DashboardLabel);
            SetBottomNavigationButton(ClaimsLabel);
            SetBottomNavigationButton(FindAProviderLabel);
            SetBottomNavigationButton(MenuLabel);

            DashboardImageView.Image = UIImage.FromBundle("DashboardIcon");
            ClaimsImageView.Image = UIImage.FromBundle("MyClaimsTouch");
            FindAProviderImageView.Image = UIImage.FromBundle("FindAProviderTouch");
            MenuImageView.Image = UIImage.FromBundle("HamburgerMenuIcon");

            SetIconTintColor();

            SetEventHandlers();

            SetConstraints();

            SetBindings();
        }

        private void SetBindings()
        {
            var set = this.CreateBindingSet<MainNavigationView, MainNavigationViewModel>();
            set.Bind(DashboardLabel).To(vm => vm.Dashboard);
            set.Bind(ClaimsLabel).To(vm => vm.Claims);
            set.Bind(FindAProviderLabel).To(vm => vm.FindProvider);
            set.Bind(MenuLabel).To(vm => vm.Menu);

            set.Bind(DashboardButton).To(vm => vm.ShowDashboardCommand);
            set.Bind(ClaimsButton).To(vm => vm.ShowClaimsCommand);
            set.Bind(FindAProviderButton).To(vm => vm.ShowFindAProviderCommand);
            set.Bind(MenuButton).To(vm => vm.ShowLandingPageCommand);
            set.Apply();
        }

        private void SetIconTintColor()
        {
            DashboardImageView.TintColor = Colors.BACKGROUND_COLOR;
            MenuImageView.TintColor = Colors.BACKGROUND_COLOR;
            FindAProviderLabel.TintColor = Colors.BACKGROUND_COLOR;
            ClaimsLabel.TintColor = Colors.BACKGROUND_COLOR;
        }

        private void SetEventHandlers()
        {
            DashboardButton.TouchDown += DashboardButton_TouchDown;
            DashboardButton.TouchUpInside += DashboardButton_TouchUpInside;
            DashboardButton.TouchCancel += DashboardButton_TouchUpInside;

            ClaimsButton.TouchDown += ClaimsButton_TouchDown;
            ClaimsButton.TouchUpInside += ClaimsButton_TouchUpInside;
            ClaimsButton.TouchCancel += ClaimsButton_TouchUpInside;

            FindAProviderButton.TouchDown += FindAProviderButton_TouchDown;
            FindAProviderButton.TouchUpInside += FindAProviderButton_TouchUpInside;
            FindAProviderButton.TouchCancel += FindAProviderButton_TouchUpInside;

            MenuButton.TouchDown += MenuButton_TouchDown;
            MenuButton.TouchUpInside += MenuButton_TouchUpInside;
            MenuButton.TouchCancel += MenuButton_TouchUpInside;
        }

        private void SetConstraints()
        {
            var rootApplication = UIApplication.SharedApplication.KeyWindow;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0) && Constants.IsPhone() && rootApplication.SafeAreaInsets.Bottom > 0)
            {
                View.RemoveConstraint(SafeBottomConstraint);
            }
            else
            {
                View.RemoveConstraint(BottomConstraint);
            }
        }

        private void SetBottomNavigationButton(UILabel label)
        {
            label.TextColor = Colors.BACKGROUND_COLOR;
            label.HighlightedTextColor = Colors.DARK_GREY_COLOR;
            label.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.BOTTOM_NAVIGATION_BAR_FONT_SIZE);
        }

        private void MenuButton_TouchDown(object sender, EventArgs e)
        {
            MenuImageView.TintColor = Colors.DARK_GREY_COLOR;
            MenuLabel.Highlighted = true;
        }

        private void MenuButton_TouchUpInside(object sender, EventArgs e)
        {
            MenuImageView.TintColor = Colors.BACKGROUND_COLOR;
            MenuLabel.Highlighted = false;
        }

        private void FindAProviderButton_TouchDown(object sender, EventArgs e)
        {
            FindAProviderImageView.TintColor = Colors.DARK_GREY_COLOR;
            FindAProviderLabel.Highlighted = true;
        }

        private void FindAProviderButton_TouchUpInside(object sender, EventArgs e)
        {
            FindAProviderImageView.TintColor = Colors.BACKGROUND_COLOR;
            FindAProviderLabel.Highlighted = false;
        }

        private void ClaimsButton_TouchDown(object sender, EventArgs e)
        {
            ClaimsImageView.TintColor = Colors.DARK_GREY_COLOR;
            ClaimsLabel.Highlighted = true;
        }

        private void ClaimsButton_TouchUpInside(object sender, EventArgs e)
        {
            ClaimsImageView.TintColor = Colors.BACKGROUND_COLOR;
            ClaimsLabel.Highlighted = false;
        }

        private void DashboardButton_TouchDown(object sender, EventArgs e)
        {
            DashboardImageView.TintColor = Colors.DARK_GREY_COLOR;
            DashboardLabel.Highlighted = true;
        }

        private void DashboardButton_TouchUpInside(object sender, EventArgs e)
        {
            DashboardImageView.TintColor = Colors.BACKGROUND_COLOR;
            DashboardLabel.Highlighted = false;
        }
    }
}