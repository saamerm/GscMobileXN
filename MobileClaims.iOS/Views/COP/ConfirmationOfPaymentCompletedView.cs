using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public partial class ConfirmationOfPaymentCompletedView : GSCBaseViewController
    {
        private ConfirmationOfPaymentCompletedViewModel _viewModel;
        private UIStringAttributes _successfulAdditionalMessageAttributes;

        public ConfirmationOfPaymentCompletedView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ConfirmationOfPaymentCompletedViewModel)ViewModel;
            base.NavigationItem.SetHidesBackButton(true, false);

            SuccessfulLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, 20);
            SuccessfulLabel.TextColor = Colors.HIGHLIGHT_COLOR;

            _successfulAdditionalMessageAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.UploadCompletedNoteFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.UploadCompletedNoteLineSpacing,
                    LineHeightMultiple = Constants.UploadCompletedNoteLineSpacing,
                    Alignment = UITextAlignment.Center
                }
            };
            SetBindings();
        }

        private void SetBindings()
        {
            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var set = this.CreateBindingSet<ConfirmationOfPaymentCompletedView, ConfirmationOfPaymentCompletedViewModel>();
            set.Bind(SuccessfulLabel).To(vm => vm.UploadSuccess);
            set.Bind(SuccessfulAdditionalMessageLabel)
               .For(x => x.AttributedText)
               .To(vm => vm.UploadCompletedNote)
               .WithConversion("StringToAttributedString", _successfulAdditionalMessageAttributes);
            set.Bind(SuccessfulAdditionalMessageLabel)
                .For(x => x.Hidden)
                .To(vm => vm.IsNoteVisible)
                .WithConversion(boolOppositeValueConverter, null);
            set.Bind(BackToClaimsButton).For("Title").To(vm => vm.BackToMyClaimsText);
            set.Bind(BackToClaimsButton).To(vm => vm.BackToMyClaimsCommand);
            set.Bind(NavigationItem).For("Title").To(vm => vm.Title);

            set.Apply();
        }
    }
}