using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public partial class AuditClaimSummaryView : GSCBaseViewController
    {
        private float _labelFontSize;
        private float _valueFontSize;
        private float _sectionHeaderFontSize;
        private UIStringAttributes _eobMessageAttribute;

        private AuditClaimSummaryViewModel _viewModel;
        private nfloat _copContainerHeight;
        private UICollectionViewFlowLayout _layout;
        private ClaimsCollectionViewSource _claimsCollectionViewSource;
        private ClaimCollectionFlowLayout _claimsCollectionsFlowLayout;

        public AuditClaimSummaryView()
            : base()
        {
            _claimsCollectionsFlowLayout = new ClaimCollectionFlowLayout();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationController.SetNavigationBarHidden(false, false);

            if (_viewModel != null)
            {
                CopContainer.Hidden = !_viewModel.IsUploadSectionVisible;
            }
            else
            {
                CopContainer.Hidden = true;
            }
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
            ClaimsCollectionView.ReloadData();            
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _viewModel.ClaimsFetched -= OnClaimsFetched;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = false;
            NavigationItem.SetHidesBackButton(false, false);

            _viewModel = (AuditClaimSummaryViewModel)ViewModel;
            _viewModel.ClaimsFetched += OnClaimsFetched;

            SetFonts();

            _layout = new UICollectionViewFlowLayout()
            {
                ItemSize = UICollectionViewFlowLayout.AutomaticSize,
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing,
            };

            ClaimsCollectionView.AllowsSelection = true;
            ClaimsCollectionView.AllowsMultipleSelection = false;
            ClaimsCollectionView.RegisterNibForCell(ClaimSummaryCellView.Nib, ClaimSummaryCellView.Key);
            ClaimsCollectionView.Delegate = _claimsCollectionsFlowLayout;
            ClaimsCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            SetBorder();
            SetBindings();
        }

        private void SetFonts()
        {
            _labelFontSize = Constants.ClaimSummaryInfoLabelFontSize;
            _valueFontSize = Constants.ClaimSummaryInfoValueFontSize;
            _sectionHeaderFontSize = Constants.ClaimSummarySectionHeaderFontSize;

            var labelColor = Colors.LightGrayColorForLabelValuePair;

            ParticipantLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummaryParticipantLabelFontSize, Colors.Black);

            PlanInformationTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, _sectionHeaderFontSize, Colors.Black);
            GSCNumberLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            ClaimFormNumberLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            SubmissionDateLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);

            GSCNumberValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, Colors.Black);
            ClaimFormNumberValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, Colors.Black);
            SubmissionDateValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, Colors.Black);

            ClaimDetailsTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, _sectionHeaderFontSize, Colors.Black);
            FooterTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, _sectionHeaderFontSize, Colors.Black);
            AuditStatusLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            AuditDueDateLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            AuditInformationLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);

            AuditValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, Colors.DARK_RED);
            AuditDueDateValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, Colors.DARK_RED);
            UploadDocMessageLabel.SetLabel(Constants.NUNITO_REGULAR, Constants.HEADING_HCSA_FONT_SIZE, Colors.DARK_RED);

            _eobMessageAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.FromName(Constants.NUNITO_REGULAR, _valueFontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.IsPhone() ? (nfloat)1.33 : (nfloat)1.38,
                    LineHeightMultiple = Constants.IsPhone() ? (nfloat)1.33 : (nfloat)1.38,
                }
            };

            var highlightFontAttribute = new UIStringAttributes
            {
                ForegroundColor = Colors.DARK_RED
            };
            var footNoteString = new NSMutableAttributedString(string.Format("{0} {1}", _viewModel.CombinedSizeOfFilesMustBe, _viewModel.TwentyFourMb));
            footNoteString.SetAttributes(highlightFontAttribute, new NSRange(_viewModel.CombinedSizeOfFilesMustBe.Length + 1, _viewModel.TwentyFourMb.Length));

            FooterNoteLabel.Lines = 0;
            FooterNoteLabel.TextColor = Colors.DarkGrayColor;
            FooterNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
            FooterNoteLabel.AttributedText = footNoteString;
        }

        private void SetBorder()
        {
            ClaimSummaryDetailView.Layer.BorderColor = Colors.DARK_GREY_COLOR.CGColor;
            ClaimSummaryDetailView.Layer.BorderWidth = Constants.TopCardBorderWidth;
        }

        private void SetBindings()
        {
            BoolOppositeValueConverter boolOppositeValueConverter = new BoolOppositeValueConverter();
            var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter();

            _claimsCollectionViewSource = new ClaimsCollectionViewSource(ClaimsCollectionView, _viewModel);
            ClaimsCollectionView.DataSource = _claimsCollectionViewSource;

            var set = this.CreateBindingSet<AuditClaimSummaryView, AuditClaimSummaryViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);
            set.Bind(ParticipantLabel).To(vm => vm.UserName);

            set.Bind(PlanInformationTitleLabel).To(vm => vm.AuditClaimSummaryHeader.PlanInformationLabel);
            set.Bind(GSCNumberLabel).To(vm => vm.AuditClaimSummaryHeader.GscNumberLabel);
            set.Bind(GSCNumberValueLabel).To(vm => vm.AuditClaimSummaryHeader.GscNumber);
            set.Bind(ClaimFormNumberLabel).To(vm => vm.AuditClaimSummaryHeader.ClaimFormNumberLabel);
            set.Bind(ClaimFormNumberValueLabel).To(vm => vm.AuditClaimSummaryHeader.ClaimFormNumber);
            set.Bind(SubmissionDateLabel).To(vm => vm.AuditClaimSummaryHeader.SubmissionDateLabel);
            set.Bind(SubmissionDateValueLabel).To(vm => vm.AuditClaimSummaryHeader.SubmissionDate);
            set.Bind(ClaimDetailsTitleLabel).To(vm => vm.ClaimDetailsSectionTitle);
            set.Bind(_claimsCollectionViewSource).To(vm => vm.Claims);
            set.Bind(FooterTitleLabel).To(vm => vm.AuditClaimSummaryFooter.AuditDetailsLabel);
            set.Bind(AuditStatusLabel).To(vm => vm.AuditClaimSummaryFooter.ClaimStatusLabel);
            set.Bind(AuditValueLabel).To(vm => vm.AuditClaimSummaryFooter.ClaimStatusValue);
            set.Bind(AuditDueDateLabel).To(vm => vm.AuditClaimSummaryFooter.AuditDueDateLabel);
            set.Bind(AuditDueDateValueLabel).To(vm => vm.AuditClaimSummaryFooter.AuditDueDate);
            set.Bind(AuditInformationLabel).To(vm => vm.AuditClaimSummaryFooter.AuditInformationLabel);
            set.Bind(AuditInformationValueLabel).For(x => x.AttributedText)
                .To(vm => vm.AuditClaimSummaryFooter.AuditInformation)
                .WithConversion("StringToAttributedString", _eobMessageAttribute);

            set.Bind(UploadDocMessageLabel).To(vm => vm.SubmitAdditionalInformationLabel);
            set.Bind(UploadDocumentsButton).To(vm => vm.OpenUploadDocumentsCommand);
            set.Bind(UploadDocumentsButton).For("Title").To(vm => vm.UploadDocuments);

            set.Bind(CopContainer).For(x => x.Hidden).To(vm => vm.IsUploadSectionVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(CopContainerHeightConstraint).For(x => x.Constant).To(vm => vm.IsUploadSectionVisible).WithConversion(boolToDefaultFloatValueConverter, _copContainerHeight);

            set.Apply();

            ClaimsCollectionView.ReloadData();
            SetClaimsCollectionHeightConstraint();
        }

        private void SetClaimsCollectionHeightConstraint()
        {
            nfloat height = 0;

            var rowCount = _viewModel.Claims.Count;
            for (int i = 0; i < rowCount; i++)
            {
                var size = _claimsCollectionsFlowLayout.GetSizeForItem(ClaimsCollectionView, _layout, NSIndexPath.FromRowSection(i, 0));
                height += size.Height;
            }

            if (rowCount > 0)
            {
                height += (rowCount - 1) *
                    _claimsCollectionsFlowLayout.GetMinimumLineSpacingForSection(ClaimsCollectionView, _layout, 0);
            }
            ClaimsCollectionViewHeightConstraint.Constant = height;
        }

        private void OnClaimsFetched(object sender, EventArgs e)
        {
            SetClaimsCollectionHeightConstraint();
        }
    }
}