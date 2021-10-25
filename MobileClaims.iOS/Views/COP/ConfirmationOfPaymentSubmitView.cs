using System;
using System.Globalization;
using System.Linq;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public partial class ConfirmationOfPaymentSubmitView : GSCBaseViewController
    {
        private DocumentListReadOnlySource<ConfirmationOfPaymentSubmitViewModel> _documentListSource;
        private ConfirmationOfPaymentSubmitViewModel _viewModel;

        public ConfirmationOfPaymentSubmitView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ConfirmationOfPaymentSubmitViewModel)ViewModel;

            NavigationController.NavigationBarHidden = false;

            SetBinding();
            SetResourceStrings();
            SetBorder();
            SetFonts();

            DisclaimerCheckBoxButton.SetBackgroundImage(UIImage.FromBundle("CheckboxNormal"), UIControlState.Normal);
            DisclaimerCheckBoxButton.SetBackgroundImage(UIImage.FromBundle("CheckboxSelected"), UIControlState.Selected);
            DisclaimerCheckBoxButton.AdjustsImageWhenHighlighted = true;
            DisclaimerCheckBoxButton.Selected = _viewModel.IsDisclaimerChecked;

            CommentsTextView.TextColor = Colors.DARK_GREY_COLOR;
            CommentsTextView.ReturnKeyType = UIReturnKeyType.Done;
            SetDocumentListTableViewHeight();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            DisclaimerCheckBoxButton.TouchUpInside += DisclaimerChecked;
            CommentsTextView.ShouldChangeText += CommentsTextViewShouldChangeText;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            DisclaimerCheckBoxButton.TouchUpInside -= DisclaimerChecked;
            CommentsTextView.ShouldChangeText -= CommentsTextViewShouldChangeText;
        }

        private void SetBinding()
        {
            _documentListSource = new DocumentListReadOnlySource<ConfirmationOfPaymentSubmitViewModel>(DocumentListTableView, _viewModel);
            DocumentListTableView.Source = _documentListSource;

            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<ConfirmationOfPaymentSubmitView, ConfirmationOfPaymentSubmitViewModel>();
            set.Bind(ParticipantNameLabel).To(vm => vm.TopCardViewData.UserName);
            set.Bind(ServiceDateValueLabel).To(vm => vm.TopCardViewData.ServiceDate);
            set.Bind(ClaimFormNumberValueLabel).To(vm => vm.TopCardViewData.ClaimForm);
            set.Bind(ServiceDescriptionValueLabel).To(vm => vm.TopCardViewData.ServiceDescription);
            set.Bind(ClaimedAmountValueLabel).To(vm => vm.TopCardViewData.ClaimedAmount);

            set.Bind(CommentsTextView).To(vm => vm.Comments);
            set.Bind(DescribeDocumentsLabel).For(x => x.Hidden).To(vm => vm.IsCommentVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(CommentsTextView).For(x => x.Hidden).To(vm => vm.IsCommentVisible).WithConversion(boolOppositeValueConverter, null);
      
            set.Bind(_documentListSource).To(vm => vm.Attachments);
            set.Bind(UploadDocumentsButton).To(vm => vm.SubmitDocumentsCommand);
            set.Bind(DisclaimerButton).To(vm => vm.OpenDisclaimerCommand);

            set.Bind(DescribeDocumentsLabel).To(vm => vm.AdditionalInformation);
            set.Bind(DisclaimerLabel).To(vm => vm.HaveReadAndAcceptThe);
            set.Bind(UploadDocumentsButton).For("Title").To(vm => vm.Submit);
            set.Bind(NavigationItem).For("Title").To(vm => vm.Title);

            set.Apply();

            DocumentListTableView.ReloadData();
        }

        private void SetResourceStrings()
        {
            ServiceDateLabel.Text = "COP_ServiceDate".tr();
            ClaimFormNumberLabel.Text = "COP_ClaimFormNumber".tr();
            ServiceDescriptionLabel.Text = "COP_ServiceDescription".tr();
            ClaimedAmountLabel.Text = "COP_ClaimedAmount".tr();
            DocumentsToUploadLabel.Text = "COP_DocumentsToUpload".tr().ToUpper(CultureInfo.CurrentUICulture);

            var underlineAttribute = new UIStringAttributes
            {
                UnderlineStyle = NSUnderlineStyle.Single,
                ForegroundColor = Colors.Black
            };
            DisclaimerButton.SetAttributedTitle(new NSAttributedString("COP_Disclaimer".tr(), underlineAttribute), UIControlState.Normal);
        }

        private void SetBorder()
        {
            ActiveClaimDetailContainer.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            ActiveClaimDetailContainer.Layer.BorderWidth = Constants.TopCardBorderWidth;

            CommentsTextView.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            CommentsTextView.Layer.BorderWidth = 0.5f;
        }

        private void SetFonts()
        {
            ParticipantNameLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_CLAIMTYPES);
            DocumentsToUploadLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.HEADING_CLAIMTYPES);

            ServiceDateLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ClaimFormNumberLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ServiceDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ClaimedAmountLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);

            ServiceDateValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            ClaimFormNumberValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            ServiceDescriptionValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            ClaimedAmountValueLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.GS_SELECTION_BUTTON);
            CommentsTextView.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_FONT_SIZE);
            SetTextColor();
        }

        private void SetTextColor()
        {
            SetTextColor(Colors.DARK_GREY_COLOR);
            DocumentsToUploadLabel.TextColor = Colors.HIGHLIGHT_COLOR;
        }

        private void SetTextColor(UIColor color)
        {
            ServiceDateLabel.TextColor = color;
            ClaimFormNumberLabel.TextColor = color;
            ServiceDescriptionLabel.TextColor = color;
            ClaimedAmountLabel.TextColor = color;
        }

        private void SetDocumentListTableViewHeight()
        {
            float listHeight;
            if (_viewModel.Attachments.Any())
            {
                listHeight = _viewModel.Attachments.Count * (float)Constants.DocumentListCellHeight;
            }
            else
            {
                listHeight = 0;
            }
            DocumentsTableViewHeightConstraints.Constant = listHeight;
            var maxY = UploadDocumentsButton.Frame.GetMaxY() + 200;
            var contentSize = ScrollView.ContentSize;
            var newContent = new CGSize(contentSize.Width, maxY);
            ScrollView.ContentSize = newContent;
        }

        private bool CommentsTextViewShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text == "\n")
            {
                textView.ResignFirstResponder();
            }
            return true;
        }

        private void DisclaimerChecked(object sender, EventArgs e)
        {
            var checkBox = sender as UIButton;
            checkBox.Selected = !checkBox.Selected;
            _viewModel.IsDisclaimerChecked = checkBox.Selected;
        }
    }
}