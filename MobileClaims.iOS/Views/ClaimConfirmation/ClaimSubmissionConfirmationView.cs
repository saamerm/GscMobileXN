using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.Claims;
using MobileClaims.iOS.Views.COP;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public partial class ClaimSubmissionConfirmationView : GSCBaseViewController
    {
        private UIStringAttributes _footerNoteAttributes;
        private UIStringAttributes _labelTextAttributes;

        private ClaimSubmissionConfirmationViewModel _viewModel;
        private UICollectionViewFlowLayout _layout;
        private UICollectionViewFlowLayout _treatmentLayout;
        private DocumentListReadOnlySource<ClaimSubmissionConfirmationViewModel> _documentListSource;
        private ClaimDetailsConfirmationCollectionViewSource _claimDetailsConfirmationCollectionViewSource;
        private ClaimDetailsConfirmationCollectionFlowLayout _claimDetailsConfirmationCollectionFlowLayout;

        private TreatmentDetailsConfirmationCollectionViewSource _treatmentDetailsConfirmationCollectionViewSource;
        private TreatmentDetailsConfirmationCollectionFlowLayout _treatmentDetailsConfirmationCollectionFlowLayout;

        protected UIBarButtonItem CancelButton;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = ((Constants.Bottom / 2) + bnbHeight);

            _viewModel.ListsCreated += _viewModel_ListsCreated;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            _viewModel.ListsCreated -= _viewModel_ListsCreated;
        }

        public ClaimSubmissionConfirmationView()
            : base()
        {
            _claimDetailsConfirmationCollectionFlowLayout = new ClaimDetailsConfirmationCollectionFlowLayout();
            _treatmentDetailsConfirmationCollectionFlowLayout = new TreatmentDetailsConfirmationCollectionFlowLayout();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _viewModel = (ClaimSubmissionConfirmationViewModel)ViewModel;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(true, false);

            CancelButton = new UIBarButtonItem();
            CancelButton.Style = UIBarButtonItemStyle.Plain;
            CancelButton.Clicked += HandleCancelButton;
            CancelButton.Title = Resource.Cancel;
            CancelButton.TintColor = Colors.HIGHLIGHT_COLOR;
            UITextAttributes attributes = new UITextAttributes();
            attributes.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.NAV_BAR_BUTTON_SIZE);
            CancelButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
            base.NavigationItem.LeftBarButtonItem = CancelButton;

            _layout = new UICollectionViewFlowLayout()
            {
                ItemSize = UICollectionViewFlowLayout.AutomaticSize,
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing
            };

            _treatmentLayout = new UICollectionViewFlowLayout()
            {
                ItemSize = UICollectionViewFlowLayout.AutomaticSize,
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing,
            };

            ClaimDetailsCollectionView.AllowsSelection = true;
            ClaimDetailsCollectionView.AllowsMultipleSelection = false;
            ClaimDetailsCollectionView.RegisterNibForCell(ClaimDetailConfirmationCellView.Nib, ClaimDetailConfirmationCellView.Key);
            ClaimDetailsCollectionView.Delegate = _claimDetailsConfirmationCollectionFlowLayout;
            ClaimDetailsCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            TreatmentDetailsCollectionView.AllowsSelection = true;
            TreatmentDetailsCollectionView.AllowsMultipleSelection = false;
            TreatmentDetailsCollectionView.RegisterNibForCell(ClaimDetailConfirmationCellView.Nib, ClaimDetailConfirmationCellView.Key);
            TreatmentDetailsCollectionView.RegisterClassForSupplementaryView(typeof(CliamSubmissionConfirmationHeaderCellView), UICollectionElementKindSection.Header, CliamSubmissionConfirmationHeaderCellView.Key);
            TreatmentDetailsCollectionView.Delegate = _treatmentDetailsConfirmationCollectionFlowLayout;
            TreatmentDetailsCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            SerperatorView.BackgroundColor = Colors.DARK_GREY_COLOR;

            PlanInformationHeaderLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.LightGrayColorForLabelValuePair);
            ClaimDetailsLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.LightGrayColorForLabelValuePair);
            TreatmentDetailsLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.LightGrayColorForLabelValuePair);
            DocumentsLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.LightGrayColorForLabelValuePair);

            GSCNumberLabel.SetLabel(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);
            ParticipantNameLabel.SetLabel(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);
            HealthProviderLabel.SetLabel(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);

            _labelTextAttributes = GetLabelStringFontAttributes(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize, Colors.Black);

            AdditionalInformationLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.LightGrayColorForLabelValuePair);
            AdditionalInformationValueLabel.SetLabel(Constants.NUNITO_BOLD, Constants.ClaimSubmitConfirmationAdditionalInfoFontSize, Colors.Black);

            SetFont();

            SetBindings();
        }

        private void SetBindings()
        {
            _claimDetailsConfirmationCollectionViewSource = new ClaimDetailsConfirmationCollectionViewSource(ClaimDetailsCollectionView, _viewModel);
            ClaimDetailsCollectionView.DataSource = _claimDetailsConfirmationCollectionViewSource;

            _treatmentDetailsConfirmationCollectionViewSource = new TreatmentDetailsConfirmationCollectionViewSource(TreatmentDetailsCollectionView, _viewModel);
            TreatmentDetailsCollectionView.DataSource = _treatmentDetailsConfirmationCollectionViewSource;

            _documentListSource = new DocumentListReadOnlySource<ClaimSubmissionConfirmationViewModel>(DocumentsTableView, _viewModel);
            DocumentsTableView.Source = _documentListSource;

            var boolOppositeValueConverter = new BoolOppositeValueConverter();
            var boolToFloatValueConverter = new BoolToDefaultFloatValueConverter();

            var set = this.CreateBindingSet<ClaimSubmissionConfirmationView, ClaimSubmissionConfirmationViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(PlanInformationHeaderLabel).To(vm => vm.PlanInformation);
            set.Bind(ClaimDetailsLabel).To(vm => vm.ClaimDetails);
            set.Bind(TreatmentDetailsLabel).To(vm => vm.TreatmentDetails);
            set.Bind(AdditionalInformationLabel).To(vm => vm.AdditionalInformation);
            set.Bind(AdditionalInformationValueLabel).To(vm => vm.Comments);
            set.Bind(GSCNumberLabel).To(vm => vm.GscIdNumber);
            set.Bind(ParticipantNameLabel).To(vm => vm.ParticipantName);
            set.Bind(HealthProviderLabel).To(vm => vm.HealthProvider);
            set.Bind(HealthProviderLabel).For(x => x.Hidden).To(vm => vm.IsHealthProviderVisible);

            set.Bind(GSCNumberValueLabel)
                .For(x => x.AttributedText)
                .To(vm => vm.Claim.Participant.PlanMemberID)
                .WithConversion("StringToAttributedString", _labelTextAttributes);
            set.Bind(ParticipantNameValueLabel)
                .For(x => x.AttributedText)
                .To(vm => vm.Claim.Participant.FullName)
                .WithConversion("StringToAttributedString", _labelTextAttributes);
            set.Bind(HealthProviderValueLabel)
                .For(x => x.AttributedText)
                .To(vm => vm.Claim.Provider.ProviderNameAndContactInfo)
                .WithConversion("StringToAttributedString", _labelTextAttributes);

            set.Bind(_claimDetailsConfirmationCollectionViewSource).To(vm => vm.ClaimDetailInput);
            set.Bind(_treatmentDetailsConfirmationCollectionViewSource).To(vm => vm.ClaimTreatmentInput);
            set.Bind(_documentListSource).To(vm => vm.Attachments);

            set.Bind(TreatmentDetailsStackViewTopConstraint).For(x => x.Constant).To(vm => vm.IsTreatmentDetailsVisible).WithConversion(boolToFloatValueConverter, TreatmentDetailsStackViewTopConstraint.Constant);
            set.Bind(TreatmentDetailsStackView).For(x => x.Hidden).To(vm => vm.IsTreatmentDetailsVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(TreatmentDetailsLabel).For(x => x.Hidden).To(vm => vm.IsTreatmentDetailsVisible).WithConversion(boolOppositeValueConverter, null);

            set.Bind(TreatmentDetailsStackViewBottomConstraint).For(x => x.Constant).To(vm => vm.IsAttachmentsVisible).WithConversion(boolToFloatValueConverter, TreatmentDetailsStackViewBottomConstraint.Constant);
            set.Bind(AttachmentsStackView).For(x => x.Hidden).To(vm => vm.IsAttachmentsVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(DocumentsLabel).To(vm => vm.DocumentsToUpload);
            set.Bind(DocumentsLabel).For(x => x.Hidden).To(vm => vm.IsAttachmentsVisible).WithConversion(boolOppositeValueConverter, null);

            set.Bind(AttachmentStackViewBottomConstraint).For(x => x.Constant).To(vm => vm.IsCommentVisible).WithConversion(boolToFloatValueConverter, AttachmentStackViewBottomConstraint.Constant);
            set.Bind(AdditionalInformationStackView).For(x => x.Hidden).To(vm => vm.IsCommentVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(AdditionalInformationLabel).For(x => x.Hidden).To(vm => vm.IsCommentVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(AdditionalInformationValueLabel).For(x => x.Hidden).To(vm => vm.IsCommentVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(SubmitClaimButton).To(vm => vm.SubmitClaimCommand);
            set.Bind(SubmitClaimButton).For("Title").To(vm => vm.SubmitButtonText);

            set.Bind(FooterNoteLabel).For(x => x.AttributedText)
                .To(vm => vm.FooterNotes)
                .WithConversion("StringToAttributedString", _footerNoteAttributes);
            set.Apply();

            ClaimDetailsCollectionView.ReloadData();
            TreatmentDetailsCollectionView.ReloadData();
            DocumentsTableView.ReloadData();

            SetClaimDetailsCollectionHeightConstraint();
            SetTreatmentDetailsCollectionHeightConstraint();
            SetDocumentTableViewHeightConstraint();
        }

        private void SetDocumentTableViewHeightConstraint()
        {
            float listHeight = 0;
            if (_viewModel.Attachments.Any())
            {
                listHeight = _viewModel.Attachments.Count * (float)Constants.DocumentListCellHeight;
            }
            DocumentsTableViewHeightConstraint.Constant = listHeight;
        }

        private void SetClaimDetailsCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (_viewModel == null || _viewModel.ClaimDetailInput == null)
            {
                return;
            }

            var rowCount = _viewModel.ClaimDetailInput.Count;
            for (int i = 0; i < rowCount; i++)
            {
                var size = _claimDetailsConfirmationCollectionFlowLayout.GetSizeForItem(ClaimDetailsCollectionView, _layout, NSIndexPath.FromRowSection(i, 0));
                height += size.Height;
            }

            if (rowCount > 0)
            {
                height += (rowCount - 1) *
                    _claimDetailsConfirmationCollectionFlowLayout.GetMinimumLineSpacingForSection(ClaimDetailsCollectionView, _layout, 0);
            }
            ClaimDetailsCollectionViewHeightConstraint.Constant = height;
        }

        private void SetTreatmentDetailsCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (_viewModel == null || _viewModel.ClaimTreatmentInput == null)
            {
                return;
            }

            var sectionCount = _viewModel.ClaimTreatmentInput.Count;
            for (int sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
            {
                var headerSize = _treatmentDetailsConfirmationCollectionFlowLayout.GetReferenceSizeForHeader(TreatmentDetailsCollectionView, _treatmentLayout, sectionIndex);
                var sectionInset = _treatmentDetailsConfirmationCollectionFlowLayout.GetInsetForSection(TreatmentDetailsCollectionView, _treatmentLayout, sectionIndex);
                height += headerSize.Height + sectionInset.Bottom + sectionInset.Top;

                var rowCount = _viewModel.ClaimTreatmentInput[sectionIndex].Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var size = _treatmentDetailsConfirmationCollectionFlowLayout.GetSizeForItem(TreatmentDetailsCollectionView, _treatmentLayout, NSIndexPath.FromRowSection(rowIndex, sectionIndex));
                    height += size.Height;
                }

                var lineSpacing = _treatmentDetailsConfirmationCollectionFlowLayout.GetMinimumLineSpacingForSection(TreatmentDetailsCollectionView, _treatmentLayout, sectionIndex);
                height += ((rowCount - 1) * lineSpacing);
            }

            TreatmentDetailsCollectionViewHeightConstraint.Constant = height;
        }

        private void SetFont()
        {
            _footerNoteAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.LightGrayColorForLabelValuePair,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSubmitConfirmationFooterNoteFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListInstructionLineSpacing,
                    LineHeightMultiple = Constants.AuditListInstructionLineSpacing
                }
            };
        }

        private UIStringAttributes GetLabelStringFontAttributes(string fontName, float fontSize, UIColor fontColor)
        {
            var attributes = new UIStringAttributes
            {
                ForegroundColor = fontColor,
                Font = UIFont.FromName(fontName, fontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListInstructionLineSpacing,
                    LineHeightMultiple = Constants.AuditListInstructionLineSpacing
                }
            };
            return attributes;
        }

        private void _viewModel_ListsCreated(object sender, EventArgs e)
        {
            SetClaimDetailsCollectionHeightConstraint();
            SetTreatmentDetailsCollectionHeightConstraint();
            SetDocumentTableViewHeightConstraint();
            ClaimDetailsCollectionView.ReloadData();
        }

        private void HandleCancelButton(object sender, EventArgs e)
        {
            int backTo = 2;

            if (base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo].GetType() == typeof(ClaimSubmitTermsAndConditionsView))
            {
                backTo++;
            }
            base.NavigationController.PopToViewController(base.NavigationController.ViewControllers[base.NavigationController.ViewControllers.Length - backTo], true);
        }
    }
}