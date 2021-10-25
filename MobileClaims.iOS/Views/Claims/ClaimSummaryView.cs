using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public partial class ClaimSummaryView : GSCBaseViewController
    {
        private nfloat _copContainerHeight;
        private ClaimSummaryViewModel _viewModel;

        public ClaimSummaryView()
            : base()
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (_viewModel != null && _viewModel.ClaimSummary != null)
            {
                CopContainer.Hidden = !_viewModel.ClaimSummary.IsCOP;
            }
            else
            {
                CopContainer.Hidden = true;
            }

            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ClaimSummaryViewModel)ViewModel;
            NavigationController.NavigationBarHidden = false;

            ParticipantNameLabel.TextColor = Colors.DARK_GREY_COLOR;
            ParticipantNameLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_CLAIMTYPES);

            ServiceDateLabel.TextColor = Colors.DARK_GREY_COLOR;
            ServiceDateLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            ClaimFormLabel.TextColor = Colors.DARK_GREY_COLOR;
            ClaimFormLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            ServiceDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
            ServiceDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            ClaimedAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
            ClaimedAmountLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            OtherPaidAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
            OtherPaidAmountLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            PaidAmountLabel.TextColor = Colors.DARK_GREY_COLOR;
            PaidAmountLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            CoPayLabel.TextColor = Colors.DARK_GREY_COLOR;
            CoPayLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            PaymentDateLabel.TextColor = Colors.DARK_GREY_COLOR;
            PaymentDateLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            PaidToLabel.TextColor = Colors.DARK_GREY_COLOR;
            PaidToLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            EobLabel.TextColor = Colors.DARK_GREY_COLOR;
            EobLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            ServiceDateValueLabel.TextColor = Colors.DarkGrayColor;
            ServiceDateValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            ClaimFormValueLabel.TextColor = Colors.DarkGrayColor;
            ClaimFormValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            ServiceDescriptionValueLabel.TextColor = Colors.DarkGrayColor;
            ServiceDescriptionValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            ClaimedAmountValueLabel.TextColor = Colors.DarkGrayColor;
            ClaimedAmountValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            OtherPaidAmountValueLabel.TextColor = Colors.DarkGrayColor;
            OtherPaidAmountValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            PaidAmountValueLabel.TextColor = Colors.DarkGrayColor;
            PaidAmountValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            CoPayValueLabel.TextColor = Colors.DarkGrayColor;
            CoPayValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            PaymentDateValueLabel.TextColor = Colors.DarkGrayColor;
            PaymentDateValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            PaidToValueLabel.TextColor = Colors.DarkGrayColor;
            PaidToValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            EobValueLabel.TextColor = Colors.DarkGrayColor;
            EobValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);

            UploadDocMessageLabel.TextColor = Colors.DARK_RED;
            UploadDocMessageLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_RED
            };
            var footNoteString = new NSMutableAttributedString(string.Format("{0} {1}", _viewModel.CombinedSizeOfFilesMustBe, _viewModel.TwentyFourMb));
            footNoteString.SetAttributes(highlightFontAttribute, new NSRange(_viewModel.CombinedSizeOfFilesMustBe.Length + 1, _viewModel.TwentyFourMb.Length));
            FootNoteLabel.Lines = 0;
            FootNoteLabel.TextColor = Colors.DarkGrayColor;
            FootNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
            FootNoteLabel.AttributedText = footNoteString;

            ComputeCopContainerHeight();

            SetBorder();
            SetBinding();
        }

        private void ComputeCopContainerHeight()
        {
            _copContainerHeight = UploadDocMessageLabel.Frame.Height + 5 + UploadDocumentsButton.Bounds.Height + 5 + FootNoteLabel.Frame.Height + 8;
        }

        private void SetBorder()
        {
            ClaimSummaryDetailView.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            ClaimSummaryDetailView.Layer.BorderWidth = Constants.TopCardBorderWidth;
        }

        private void SetBinding()
        {
            BoolOppositeValueConverter boolOppositeValueConverter = new BoolOppositeValueConverter();
            var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter();

            var set = this.CreateBindingSet<ClaimSummaryView, ClaimSummaryViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(ParticipantNameLabel).To(vm => vm.ClaimSummary.UserName);
            set.Bind(ServiceDateLabel).To(vm => vm.ServiceDate);
            set.Bind(ServiceDateValueLabel).To(vm => vm.ClaimSummary.ServiceDate);
            set.Bind(ClaimFormLabel).To(vm => vm.ClaimForm);
            set.Bind(ClaimFormValueLabel).To(vm => vm.ClaimSummary.ClaimForm);
            set.Bind(ServiceDescriptionLabel).To(vm => vm.ServiceDescription);
            set.Bind(ServiceDescriptionValueLabel).To(vm => vm.ClaimSummary.ServiceDescription);
            set.Bind(ClaimedAmountLabel).To(vm => vm.ClaimedAmount);
            set.Bind(ClaimedAmountValueLabel).To(vm => vm.ClaimSummary.ClaimedAmount);
            set.Bind(OtherPaidAmountLabel).To(vm => vm.OtherPaidAmount);
            set.Bind(OtherPaidAmountValueLabel).To(vm => vm.ClaimSummary.OtherPaidAmount);
            set.Bind(PaidAmountLabel).To(vm => vm.PaidAmount);
            set.Bind(PaidAmountValueLabel).To(vm => vm.ClaimSummary.PaidAmount);
            set.Bind(CoPayLabel).To(vm => vm.CoPay);
            set.Bind(CoPayValueLabel).To(vm => vm.ClaimSummary.Copay);
            set.Bind(PaymentDateLabel).To(vm => vm.PaymentDate);
            set.Bind(PaymentDateValueLabel).To(vm => vm.ClaimSummary.PaymentDate);
            set.Bind(PaidToLabel).To(vm => vm.PaidTo);
            set.Bind(PaidToValueLabel).To(vm => vm.ClaimSummary.PayTo);
            set.Bind(EobLabel).To(vm => vm.Eob);
            set.Bind(EobValueLabel).To(vm => vm.ClaimSummary.EobMessages);

            set.Bind(UploadDocMessageLabel).To(vm => vm.SubmitAdditionalInformationLabel);
            set.Bind(UploadDocumentsButton).To(vm => vm.OpenUploadDocumentsCommand);
            set.Bind(UploadDocumentsButton).For("Title").To(vm => vm.UploadButtonText);

            set.Bind(CopContainer).For(x => x.Hidden).To(vm => vm.ClaimSummary.IsCOP).WithConversion(boolOppositeValueConverter, null);
            set.Bind(CopContainerHeightConstraint).For(x => x.Constant).To(vm => vm.ClaimSummary.IsCOP).WithConversion(boolToDefaultFloatValueConverter, _copContainerHeight);

            set.Apply();
        }
    }
}