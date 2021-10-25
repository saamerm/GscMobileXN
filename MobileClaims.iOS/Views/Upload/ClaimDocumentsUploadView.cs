using System;
using System.Linq;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.COP;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Upload
{
    public partial class ClaimDocumentsUploadView : GSCBaseViewController
    {
        private ClaimDocumentsUploadViewModel _viewModel;
        private DocumentListSource<ClaimDocumentsUploadViewModel> _documentListSource;

        public ClaimDocumentsUploadView()
            : base()
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            AdditionalInformationTextView.ShouldChangeText += AdditionalInformationTextViewShouldChangeText;
            _viewModel.AttachmentsAdded += ReloadList;
            _viewModel.AttachmentRemoved += ReloadList;
        }
        
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            AdditionalInformationTextView.ShouldChangeText -= AdditionalInformationTextViewShouldChangeText;
            _viewModel.AttachmentsAdded -= ReloadList;
            _viewModel.AttachmentRemoved -= ReloadList;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _viewModel = (ClaimDocumentsUploadViewModel)ViewModel;

            AddDocumentButton.SetImage(UIImage.FromBundle("Add"), UIControlState.Normal);
            SetBindings();
            SetBorder();
            SetFonts();            
            SetPhotoSizeText();
        }

        private void SetBindings()
        {
            _documentListSource = new DocumentListSource<ClaimDocumentsUploadViewModel>(DocumentsTableView, _viewModel);
            DocumentsTableView.Source = _documentListSource;

            var set = this.CreateBindingSet<ClaimDocumentsUploadView, ClaimDocumentsUploadViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(SubmitDocumentsLabel).To(vm => vm.PleaseSubmitDocuments);
            set.Bind(AdditionalInfornationLabel).To(vm => vm.AdditionalInformation);
            set.Bind(AddDocumentButton).For("Title").To(vm => vm.AddAnotherDocument);
            set.Bind(NextButton).For("Title").To(vm => vm.Next);
            set.Bind(_documentListSource).To(vm => vm.Attachments);
            set.Bind(AddDocumentButton).To(vm => vm.ShowDocumentSelectionPopoverCommand);
            set.Bind(NextButton).To(vm => vm.OpenDisclaimerCommand);
            set.Bind(AdditionalInformationTextView).To(vm => vm.Comments);
            set.Apply();

            DocumentsTableView.ReloadData();
        }

        private void SetPhotoSizeText()
        {
            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_RED
            };
            var footNoteString = new NSMutableAttributedString(string.Format("{0} {1}", _viewModel.CombinedSizeOfFilesMustBe, _viewModel.TwentyFourMb));
            footNoteString.SetAttributes(highlightFontAttribute, new NSRange(_viewModel.CombinedSizeOfFilesMustBe.Length + 1, _viewModel.TwentyFourMb.Length));
            PhotoSizeLimitLabel.AttributedText = footNoteString;
        }

        private void SetFonts()
        {
            SubmitDocumentsLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimDocumentsUploadSectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            AdditionalInfornationLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimDocumentsUploadSectionHeaderFontSize, Colors.DARK_GREY_COLOR);

            PhotoSizeLimitLabel.SetLabel(Constants.NUNITO_REGULAR, Constants.ClaimDocumentsUploadFootNoteFontSize, Colors.Black);            
            SetButton(AddDocumentButton, Constants.NUNITO_REGULAR, Constants.ClaimDocumentsUploadAnotherDocumentButtonFontSize, Colors.HIGHLIGHT_COLOR);
        }

        private void SetButton(UIButton button, string fontName, float fontSize, UIColor textColor)
        {
            button.Font = UIFont.FromName(fontName, fontSize);
            AddDocumentButton.SetTitleColor(textColor, UIControlState.Normal);
        }

        private void SetBorder()
        {
            AdditionalInformationTextView.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            AdditionalInformationTextView.Layer.BorderWidth = 0.5f;

            SeperatorView.BackgroundColor = Colors.LightGrayColor;
        }

        private bool AdditionalInformationTextViewShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text == "\n")
            {
                textView.ResignFirstResponder();
            }
            return true;
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

            DocumentsTableViewHeightConstraint.Constant = listHeight;
            SeperatorViewLeadingConstraint.Constant = 20 + DocumentsTableView.SeparatorInset.Left;
        }


        private void ReloadList(object sender, EventArgs e)
        {
            SetDocumentListTableViewHeight();
            DocumentsTableView.ReloadData();
        }
    }
}