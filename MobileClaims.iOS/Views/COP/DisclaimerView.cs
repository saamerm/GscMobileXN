using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public partial class DisclaimerView : GSCBaseViewController
    {
        private DisclaimerViewModel _viewModel;
        private UIStringAttributes _disclaimerParagraphAttributes;

        public DisclaimerView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (DisclaimerViewModel)ViewModel;

            NavigationController.NavigationBarHidden = false;
            NavigationItem.Title = string.Empty;
                        
            DisclaimerLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_CLAIMTYPES);
            DisclaimerLabel.TextColor = Colors.Black;
            SetAttribute();
            SetBindings();
        }

        private void SetBindings()
        {
            var set = this.CreateBindingSet<DisclaimerView, DisclaimerViewModel>();
            set.Bind(DisclaimerLabel).To(vm => vm.Disclaimer);
            set.Bind(DisclaimerParagraph1Label).For(x => x.AttributedText).To(vm => vm.FirstParagraph).WithConversion("StringToAttributedString", _disclaimerParagraphAttributes);
            set.Bind(DisclaimerParagraph2Label).For(x => x.AttributedText).To(vm => vm.SecondParagraph).WithConversion("StringToAttributedString", _disclaimerParagraphAttributes); ;
            set.Bind(DisclaimerParagraph3Label).For(x => x.AttributedText).To(vm => vm.ThirdParagraph).WithConversion("StringToAttributedString", _disclaimerParagraphAttributes);;
            set.Apply();
        }

        private void SetAttribute()
        {
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
        }
    }
}