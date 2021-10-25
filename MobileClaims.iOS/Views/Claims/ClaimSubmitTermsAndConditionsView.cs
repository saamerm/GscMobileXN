using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public partial class ClaimSubmitTermsAndConditionsView : GSCBaseViewController
    {
        private ClaimSubmitTermsAndConditionsViewModel _viewModel;
        private UIStringAttributes _disclaimerParagraphAttributes;

        public ClaimSubmitTermsAndConditionsView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ButtonContainerBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = (ClaimSubmitTermsAndConditionsViewModel)ViewModel;

            NavigationController.NavigationBarHidden = false;

            DisclaimerLabel.TextColor = Colors.Black;
            DisclaimerLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_CLAIMTYPES);

            _disclaimerParagraphAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.DarkGrayColor,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.DisclaimerContentFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.DisclaimerContentLineSpacing,
                    LineHeightMultiple = Constants.DisclaimerContentLineSpacing
                }
            };
            SetBinding();
        }
        
        private void SetBinding()
        {
            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<ClaimSubmitTermsAndConditionsView, ClaimSubmitTermsAndConditionsViewModel>();
            set.Bind(NavigationItem).For("Title").To(vm => vm.Title);
            set.Bind(DisclaimerLabel).To(vm => vm.DisclaimerLabel);
            set.Bind(DisclaimerParagraph1Label).For(x => x.AttributedText).To(vm => vm.FirstParagraph).WithConversion("StringToAttributedString", _disclaimerParagraphAttributes);
            set.Bind(DisclaimerParagraph2Label).For(x => x.AttributedText).To(vm => vm.SecondParagraph).WithConversion("StringToAttributedString", _disclaimerParagraphAttributes);
            set.Bind(AgreeButton).To(vm => vm.AcceptTermsAndConditionsCommand);
            set.Bind(AgreeButton).For("Title").To(vm => vm.AgreeButtonLabel);
            set.Bind(ButtonContainerView).For(x => x.Hidden).To(vm => vm.ClaimError);
            set.Apply();
        }
    }
}