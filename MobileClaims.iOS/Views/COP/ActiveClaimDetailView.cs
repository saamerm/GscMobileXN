using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public partial class ActiveClaimDetailView : GSCBaseViewController
    {
        private ActiveClaimDetailViewModel _viewModel;

        public ActiveClaimDetailView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ActiveClaimDetailViewModel)ViewModel;

            NavigationController.NavigationBarHidden = false;

            SetBinding();
            SetBorder();
            SetFonts();
            SetResourceStrings();
        }
       
        private void SetBorder()
        {
            ActiveClaimDetailContainer.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            ActiveClaimDetailContainer.Layer.BorderWidth = Constants.TopCardBorderWidth;
        }

        private void SetFonts()
        {
            ParticipantNameLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_CLAIMTYPES);

            ServiceDateLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ClaimFormNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ServiceDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ClaimedAmountLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            EOBMessagesLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            ServiceDateValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            ClaimFormNumberValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            ServiceDescriptionValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            ClaimedAmountValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            EOBMessagesValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            UploadDocumentMessageLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            SetTextColor();
        }

        private void SetTextColor()
        {
            SetTextColor(Colors.DARK_GREY_COLOR);
            UploadDocumentMessageLabel.TextColor = Colors.DARK_RED;

            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_RED
            };           
            var footNoteString = new NSMutableAttributedString(string.Format("{0} {1}", _viewModel.CombinedSizeOfFilesMustBe, _viewModel.TwentyFourMb));
            footNoteString.SetAttributes(highlightFontAttribute, new NSRange(_viewModel.CombinedSizeOfFilesMustBe.Length + 1, _viewModel.TwentyFourMb.Length));
            FootNoteLabel.Lines = 0;
            FootNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
            FootNoteLabel.AttributedText = footNoteString;
        }

        private void SetTextColor(UIColor color)
        {
            ServiceDateLabel.TextColor = color;
            ClaimFormNumberLabel.TextColor = color;
            ServiceDescriptionLabel.TextColor = color;
            ClaimedAmountLabel.TextColor = color;
            EOBMessagesLabel.TextColor = color;
        }

        private void SetBinding()
        {
            var set = this.CreateBindingSet<ActiveClaimDetailView, ActiveClaimDetailViewModel>();
            set.Bind(ParticipantNameLabel).To(vm => vm.TopCardViewData.UserName);
            set.Bind(ServiceDateValueLabel).To(vm => vm.TopCardViewData.ServiceDate);
            set.Bind(ClaimFormNumberValueLabel).To(vm => vm.TopCardViewData.ClaimForm);
            set.Bind(ServiceDescriptionValueLabel).To(vm => vm.TopCardViewData.ServiceDescription);
            set.Bind(ClaimedAmountValueLabel).To(vm => vm.TopCardViewData.ClaimedAmount);
            set.Bind(EOBMessagesLabel).To(vm => vm.ExplanationOfBenefitsLabel);
            set.Bind(EOBMessagesValueLabel).To(vm => vm.TopCardViewData.EobMessages);
            set.Bind(UploadDocumentsButton).To(vm => vm.OpenUploadDocumentsCommand);
            set.Bind(UploadDocumentsButton).For("Title").To(vm => vm.UploadButtonText);
            set.Bind(NavigationItem).For("Title").To(vm => vm.Title);
            set.Bind(UploadDocumentMessageLabel).To(vm => vm.SubmitAdditionalInformationLabel);
            set.Apply();
        }

        private void SetResourceStrings()
        {
            ServiceDateLabel.Text = "COP_ServiceDate".tr();
            ClaimFormNumberLabel.Text = "COP_ClaimFormNumber".tr();
            ServiceDescriptionLabel.Text = "COP_ServiceDescription".tr();
            ClaimedAmountLabel.Text = "COP_ClaimedAmount".tr();
        }
    }
}