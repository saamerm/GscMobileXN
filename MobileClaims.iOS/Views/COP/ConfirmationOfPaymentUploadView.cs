using System;
using System.Globalization;
using System.Linq;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public partial class ConfirmationOfPaymentUploadView : GSCBaseViewController
    {
        private UIButton _backButton;
        private DocumentListSource<ConfirmationOfPaymentUploadViewModel> _documentListSource;
        private ConfirmationOfPaymentUploadViewModel _viewModel;

        public ConfirmationOfPaymentUploadView()
            : base()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = (ConfirmationOfPaymentUploadViewModel)ViewModel;

            NavigationController.NavigationBarHidden = false;

            NavigationController.NavigationItem.SetHidesBackButton(true, false);

            // Show the custom back button when COP page was displayed from ClaimHistoryDetail page and on iPad
            if (_viewModel.NavigationCatalog != null && !Constants.IsPhone())
            {
                NavigationItem.SetHidesBackButton(true, false);
                SetBackButton();
            }

            SetBinding();
            SetResourceStrings();
            SetBorder();
            SetFonts();

            _viewModel.ViewModelWillDestroy += ViewModel_ViewModelWillDestroy;
            _viewModel.AttachmentsAdded += ViewModel_AttachmentsAdded;
            _viewModel.AttachmentRemoved += ViewModel_AttachmentsAdded;
        }

        private void SetBackButton()
        {
            var customNavView = new UIView(new CGRect(20, 0, 63, Constants.NAV_HEIGHT));
            var arrowImage = new UIImageView(UIImage.FromBundle("ArrowBack"));
            var navigationHeight = Constants.NAV_HEIGHT;
            var arrowHeight = arrowImage.Image.Size.Height;

            if (!Constants.IsPhone())
            {
                navigationHeight = Constants.NAV_HEIGHT / 4;
                arrowHeight = arrowImage.Image.Size.Height / 4;
            }
            else
            {
                navigationHeight = Constants.NAV_HEIGHT / 3;
                arrowHeight = arrowImage.Image.Size.Height / 3;
            }

            arrowImage.Frame = new CGRect(20, navigationHeight - arrowHeight, arrowImage.Image.Size.Width, arrowImage.Image.Size.Height);
            customNavView.AddSubview(arrowImage);
            var backLabel = new UILabel(new CGRect(20, navigationHeight - arrowHeight, 43, navigationHeight))
            {
                Text = " ".tr(),
                TextColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.SystemFontOfSize(13)
            };
            customNavView.AddSubview(backLabel);

            _backButton = new UIButton(UIButtonType.Custom);
            _backButton.Frame = customNavView.Bounds;
            customNavView.Add(_backButton);

            var cancelButton = new UIBarButtonItem(customNavView);
            cancelButton.Style = UIBarButtonItemStyle.Plain;
            NavigationItem.LeftBarButtonItem = cancelButton;
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            SetDocumentListTableViewHeight();
        }

        private void SetBinding()
        {
            _documentListSource = new DocumentListSource<ConfirmationOfPaymentUploadViewModel>(DocumentListTableView, _viewModel);
            DocumentListTableView.Source = _documentListSource;

            var set = this.CreateBindingSet<ConfirmationOfPaymentUploadView, ConfirmationOfPaymentUploadViewModel>();
            set.Bind(ParticipantNameLabel).To(vm => vm.TopCardViewData.UserName);
            set.Bind(ServiceDateValueLabel).To(vm => vm.TopCardViewData.ServiceDate);
            set.Bind(ClaimFormNumberValueLabel).To(vm => vm.TopCardViewData.ClaimForm);
            set.Bind(ServiceDescriptionValueLabel).To(vm => vm.TopCardViewData.ServiceDescription);
            set.Bind(ClaimedAmountValueLabel).To(vm => vm.TopCardViewData.ClaimedAmount);
            set.Bind(_documentListSource).To(vm => vm.Attachments);
            set.Bind(AttachDocumentButton).To(vm => vm.ShowDocumentSelectionPopoverCommand);
            set.Bind(UploadDocumentsButton).To(vm => vm.SubmitAttachmentsCommand);

            set.Bind(AttachDocumentButton).For("Title").To(vm => vm.AddAnotherDocument);
            set.Bind(UploadDocumentsButton).For("Title").To(vm => vm.Next);
            set.Bind(NavigationItem).For("Title").To(vm => vm.Title);

            if (_viewModel.NavigationCatalog != null && !Constants.IsPhone())
            {
                set.Bind(_backButton).To(vm => vm.NavigateToPreviousPageCommand);
            }
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
        }

        private void SetBorder()
        {
            ActiveClaimDetailContainer.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            ActiveClaimDetailContainer.Layer.BorderWidth = Constants.TopCardBorderWidth;

            SeperatorView.BackgroundColor = Colors.LightGrayColor;
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

            AttachDocumentButton.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_FONT_SIZE);
            SetTextColor();
        }

        private void SetTextColor()
        {
            SetTextColor(Colors.DARK_GREY_COLOR);
            DocumentsToUploadLabel.TextColor = Colors.HIGHLIGHT_COLOR;
            AttachDocumentButton.SetTitleColor(Colors.HIGHLIGHT_COLOR, UIControlState.Normal);
            AttachDocumentButton.SetImage(UIImage.FromBundle("Add"), UIControlState.Normal);

            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_RED
            };
            var footNoteString = new NSMutableAttributedString(string.Format("{0} {1}", _viewModel.CombinedSizeOfFilesMustBe, _viewModel.TwentyFourMb));
            footNoteString.SetAttributes(highlightFontAttribute, new NSRange(_viewModel.CombinedSizeOfFilesMustBe.Length + 1, _viewModel.TwentyFourMb.Length));
            FootNoteLabel.AttributedText = footNoteString;
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
            AttachmentsListHeight.Constant = listHeight;
            var maxY = UploadDocumentsButton.Frame.GetMaxY() + 200;
            var contentSize = ScrollView.ContentSize;
            var newContent = new CGSize(contentSize.Width, maxY);
            ScrollView.ContentSize = newContent;

            SeperatorViewLeadingConstraint.Constant = 20 + DocumentListTableView.SeparatorInset.Left;
        }

        private void ViewModel_ViewModelWillDestroy(object sender, EventArgs e)
        {
            _viewModel.ViewModelWillDestroy -= ViewModel_ViewModelWillDestroy;
            _viewModel.AttachmentsAdded -= ViewModel_AttachmentsAdded;
            _viewModel.AttachmentRemoved -= ViewModel_AttachmentsAdded;
        }

        private void ViewModel_AttachmentsAdded(object sender, EventArgs e)
        {
            SetDocumentListTableViewHeight();
            DocumentListTableView.ReloadData();
        }
    }
}