using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Models;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Dashboard
{
    public partial class Dashboard : GSCBaseViewController
    {
        private DashboardViewModel _viewModel;
        private UICollectionViewFlowLayout _layout;
        private RecentClaimsCollectionViewSource _recentClaimsCollectionViewSource;

        public Dashboard()
            : base()
        {
            _layout = new UICollectionViewFlowLayout()
            {
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(true, false);

            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
                        
            AuditNotificationTopConstraint.Constant = Constants.IS_OS_VERSION_OR_LATER(11, 0) ? 0 : 20;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBarHidden = true;
            NavigationItem.SetHidesBackButton(true, false);

            _viewModel = (DashboardViewModel)ViewModel;

            AuditNotificationView.BackgroundColor = Colors.DARK_RED;

            AuditNotificationLabel.TextColor = UIColor.White;
            AuditNotificationLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, 14.0f);
            AuditNotificationImage.Image = UIImage.FromBundle("ArrowForwardWhite");

            WelcomeLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            WelcomeLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.DASHBOARD_WELCOME_FONT_SIZE);

            ParticipantNameLabel.TextColor = Colors.DarkGrayColor;
            ParticipantNameLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.DASHBOARD_PARTICIPANT_NAME_LABEL_SIZE);

            RecentClaimsLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            RecentClaimsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.DASHBOARD_SECTION_TITLE_FONT_SIZE);

            NoRecentClaimsLabel.TextColor = Colors.DarkGrayColor;
            NoRecentClaimsLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_FONT_SIZE);

            MyBenefitsLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            MyBenefitsLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.DASHBOARD_SECTION_TITLE_FONT_SIZE);

            ViewAllButton.BackgroundColor = Colors.Clear;
            ViewAllButton.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
            ViewAllButton.Layer.BorderColor = Colors.HIGHLIGHT_COLOR.CGColor;
            ViewAllButton.Layer.BorderWidth = Constants.ButtonBorderWidth;
            ViewAllButton.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.DASHBOARD_MEDIUM_BUTTON_FONT_SIZE);

            var buttonFontSize = Constants.IsPhone() ? Constants.DASHBOARD_SMALL_BUTTON_FONT_SIZE : Constants.DASHBOARD_LARGE_BUTTON_FONT_SIZE;

            DentalImageView.Image = UIImage.FromBundle("Dental");
            ChiroImageView.Image = UIImage.FromBundle("Chiropractor");
            MassageImageView.Image = UIImage.FromBundle("Massage");
            DrugsOnTheGoImageView.Image = UIImage.FromBundle("DrugsOnTheGo");
            HCSAImageView.Image = UIImage.FromBundle("HCSA");
            PSAImageView.Image = UIImage.FromBundle("PSA");

            SetMyBenefitButton(DRELabel, DREView, buttonFontSize);
            SetMyBenefitButton(ChiroLabel, ChiroView, buttonFontSize);
            SetMyBenefitButton(MassageLabel, MassageView, buttonFontSize);
            SetMyBenefitButton(DrugsOnTheGoLabel, DrugsOnTheGoView, buttonFontSize);
            SetMyBenefitButton(HealthCareSpendingLabel, HCSAView, buttonFontSize);
            SetMyBenefitButton(PersonalSpendingLabel, PSAView, buttonFontSize);

            RecentClaimsCollectionView.AllowsSelection = true;
            RecentClaimsCollectionView.AllowsMultipleSelection = false;
            RecentClaimsCollectionView.CollectionViewLayout = _layout;
            RecentClaimsCollectionView.RegisterNibForCell(RecentClaimCellView.Nib, RecentClaimCellView.Key);

            SetBinding();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _viewModel.ClaimsFetched += OnClaimsFetched;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _viewModel.ClaimsFetched -= OnClaimsFetched;
        }

        private void SetBinding()
        {
            BoolOppositeValueConverter boolOppositeValueConverter = new BoolOppositeValueConverter();
            var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter();
            var boolToDefaultStringValueConverter = new BoolToDefaultStringValueConverter(true);

            _recentClaimsCollectionViewSource = new RecentClaimsCollectionViewSource(RecentClaimsCollectionView, _viewModel);
            RecentClaimsCollectionView.Source = _recentClaimsCollectionViewSource;

            var set = this.CreateBindingSet<Dashboard, DashboardViewModel>();

            set.Bind(WelcomeLabel).To(vm => vm.WelcomeTitle);
            set.Bind(AuditNotificationLabel).To(vm => vm.ActionRequiredLabel);
            set.Bind(RecentClaimsLabel).To(vm => vm.RecentClaimsTitle);
            set.Bind(MyBenefitsLabel).To(vm => vm.MyBenefitsTitle);
            set.Bind(ParticipantNameLabel).To(vm => vm.UserName);
            set.Bind(DRELabel).To(vm => vm.DentalRecallExam);
            set.Bind(ChiroLabel).To(vm => vm.ChiropracticTreatment);
            set.Bind(MassageLabel).To(vm => vm.MassageTherapy);
            set.Bind(DrugsOnTheGoLabel).To(vm => vm.DrugsOnTheGo);
            set.Bind(NoRecentClaimsLabel).To(vm => vm.HasRecentClaims).WithConversion(boolToDefaultStringValueConverter, _viewModel.NoRecentClaimsLabel);
            set.Bind(ViewAllButton).For("Title").To(vm => vm.ViewAll);
            set.Bind(PersonalSpendingLabel).To(vm => vm.PsaTitle);
            set.Bind(HealthCareSpendingLabel).To(vm => vm.HcsaTitle);

            set.Bind(AuditNotificationButton).To(vm => vm.OpenAuditCommand);
            set.Bind(DREButton).To(vm => vm.ShowEligibilityCheckCommand).CommandParameter(DashboardEligibilityCheckType.RECALLEXAM);
            set.Bind(ChiroButton).To(vm => vm.ShowEligibilityCheckCommand).CommandParameter(DashboardEligibilityCheckType.CHIRO);
            set.Bind(MassageButton).To(vm => vm.ShowEligibilityCheckCommand).CommandParameter(DashboardEligibilityCheckType.MASSAGE);
            set.Bind(DrugsOnTheGoButton).To(vm => vm.ShowDrugsOnTheGoCommand);
            set.Bind(HCSAButton).To(vm => vm.ShowHcsaCommand);
            set.Bind(PSAButton).To(vm => vm.ShowPsaCommand);
            set.Bind(ViewAllButton).To(vm => vm.ShowAllClaimsCommand);

            set.Bind(AuditNotificationView).For(x => x.Hidden).To(vm => vm.ShouldRibbonBeDisplayed).WithConversion(boolOppositeValueConverter, null);         
            set.Bind(HCSAView).For(x => x.Hidden).To(vm => vm.IsHcsaVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(PSAView).For(x => x.Hidden).To(vm => vm.IsPsaVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(_recentClaimsCollectionViewSource).To(vm => vm.RecentClaims);
            set.Bind(_recentClaimsCollectionViewSource).For(source => source.SelectionChangedCommand).To(vm => vm.SelectRecentClaimCommand);
            set.Bind(RecentClaimsCollectionView).For(x => x.Hidden).To(vm => vm.HasAnyClaims).WithConversion(boolOppositeValueConverter, null);
            set.Bind(NoRecentClaimsLabel).For(x => x.Hidden).To(vm => vm.HasRecentClaims);

            set.Bind(IntroductionContainerView).For(x => x.Hidden).To(vm => vm.IsFeatureMarketingAvailable).WithConversion(boolOppositeValueConverter, null);
            set.Bind(IntroductionViewHeightConstraint).For(x => x.Constant).To(x => x.IsFeatureMarketingAvailable).WithConversion(boolToDefaultFloatValueConverter, 139.0f);
            //set.Bind(AuditNotificationHeightConstraint).For(x => x.Constant).To(x => x.ShouldRibbonBeDisplayed).WithConversion(boolToDefaultFloatValueConverter, 44.0f);

            set.Apply();

            RecentClaimsCollectionView.ReloadData();
            SetAuditNotificationViewHeightConstraint();
            SetRecentClaimsCollectionHeightConstraint();
        }

        private void SetAuditNotificationViewHeightConstraint()
        {
            AuditNotificationHeightConstraint.Constant = 0;
            if (_viewModel.ShouldRibbonBeDisplayed)
            {
                AuditNotificationHeightConstraint.Constant = Constants.DashboardNotificationHeight;
            }
        }

        private void SetRecentClaimsCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (Constants.IsPhone())
            {
                var rowCount = _viewModel.RecentClaims.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    var size = _recentClaimsCollectionViewSource.GetSizeForItem(RecentClaimsCollectionView, _layout, NSIndexPath.FromRowSection(i, 0));
                    height += size.Height;
                }

                if (rowCount > 0)
                {
                    height += (rowCount - 1) * Constants.RecentClaimsCollectionViewCellSpacing;
                }
            }
            else
            {
                var rowCount = (int)Math.Ceiling((double)_viewModel.RecentClaims.Count / (double)2);
                height = rowCount * Constants.RecentClaimsCellHeightOniPad + (rowCount - 1) * Constants.RecentClaimsCollectionViewCellSpacing;
            }
            RecentClaimsCollectionHeight.Constant = height;
        }

        private void SetMyBenefitButton(UILabel label, UIView view, float buttonFontSize)
        {
            label.TextColor = Colors.DarkGrayColor;
            label.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, buttonFontSize);
            view.BackgroundColor = Colors.LightGrayColor;
        }

        private void OnClaimsFetched(object sender, EventArgs e)
        {
            SetRecentClaimsCollectionHeightConstraint();
        }
    }
}