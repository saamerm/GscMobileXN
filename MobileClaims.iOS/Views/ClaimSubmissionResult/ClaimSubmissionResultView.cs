using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.Extensions;
using MobileClaims.iOS.UI;
using MobileClaims.iOS.Views.ClaimConfirmation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimSubmissionResult
{
    public partial class ClaimSubmissionResultView : GSCBaseViewController
    {
        private ClaimSubmissionResultViewModel _viewModel;

        private UICollectionViewFlowLayout _claimDetailsFlowLayout;

        private ClaimResultDetailsCollectionViewSource _claimResultDetailsCollectionViewSource;
        private ClaimResultTotalsCollectionViewSource _claimResultPlanLimitationCollectionViewSource;
        private ClaimResultTotalsCollectionViewSource _claimResultTotalsCollectionViewSource;

        private ClaimResultDetailsCollectionFlowLayout _claimResultDetailsCollectionFlowLayout;
        private ClaimResultTotalsCollectionFlowLayout _claimResultTotalsCollectionFlowLayout;
        private ClaimResultPlanLimitationCollectionFlowLayout _claimResultPlanLimitationFlowLayout;
        private bool _showUploadDocuments;
        private bool _showAuditUpload;

        public bool ShowUploadDocuments
        {
            get => _showUploadDocuments;
            set
            {
                _showUploadDocuments = value;
                if (!_showUploadDocuments)
                {
                    CopUploadStackViewHeightConstraint.Active = true;
                    CopUploadStackViewHeightConstraint.Constant = 0;
                }
                CopUploadStackView.LayoutIfNeeded();
            }
        }

        public bool ShowAuditUpload
        {
            get => _showAuditUpload;
            set
            {
                _showAuditUpload = value;
                if (!_showAuditUpload)
                {
                    AuditUploadStackViewHeightConstraint.Active = true;
                    AuditUploadStackViewHeightConstraint.Constant = 0;
                }
                AuditUploadStackView.LayoutIfNeeded();
            }
        }

        public ClaimSubmissionResultView()
            : base()
        {
            _claimResultDetailsCollectionFlowLayout = new ClaimResultDetailsCollectionFlowLayout();
            _claimResultTotalsCollectionFlowLayout = new ClaimResultTotalsCollectionFlowLayout();
            _claimResultPlanLimitationFlowLayout = new ClaimResultPlanLimitationCollectionFlowLayout();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            var bnbHeight = Constants.IsPhone() ? Constants.NAV_BUTTON_SIZE_IPHONE : Constants.NAV_BUTTON_SIZE_IPAD;
            ScrollViewBottomConstraint.Constant = -((Constants.Bottom / 2) + bnbHeight);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            base.NavigationItem.SetHidesBackButton(true, false);

            _viewModel = (ClaimSubmissionResultViewModel)ViewModel;
            _viewModel.ClaimResultFetched += _viewModel_ClaimResultFetched;

            _claimDetailsFlowLayout = new UICollectionViewFlowLayout()
            {
                ItemSize = UICollectionViewFlowLayout.AutomaticSize,
                SectionInset = new UIEdgeInsets(0, 0, 0, 0),
                MinimumLineSpacing = Constants.RecentClaimsCollectionViewCellSpacing,
            };

            ClaimDetailsCollectionView.AllowsSelection = true;
            ClaimDetailsCollectionView.AllowsMultipleSelection = false;
            ClaimDetailsCollectionView.RegisterNibForCell(ClaimDetailConfirmationCellView.Nib, ClaimDetailConfirmationCellView.Key);
            ClaimDetailsCollectionView.RegisterClassForSupplementaryView(typeof(CliamSubmissionConfirmationHeaderCellView), UICollectionElementKindSection.Header, CliamSubmissionConfirmationHeaderCellView.Key);
            ClaimDetailsCollectionView.RegisterClassForSupplementaryView(typeof(ClaimDetailsFooterCellView), UICollectionElementKindSection.Footer, ClaimDetailsFooterCellView.Key);
            ClaimDetailsCollectionView.Delegate = _claimResultDetailsCollectionFlowLayout;
            ClaimDetailsCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            TotalsCollectionView.AllowsSelection = true;
            TotalsCollectionView.AllowsMultipleSelection = false;
            TotalsCollectionView.RegisterNibForCell(ClaimDetailConfirmationCellView.Nib, ClaimDetailConfirmationCellView.Key);
            TotalsCollectionView.RegisterClassForSupplementaryView(typeof(ClaimDetailsFooterCellView), UICollectionElementKindSection.Footer, ClaimDetailsFooterCellView.Key);
            TotalsCollectionView.Delegate = _claimResultTotalsCollectionFlowLayout;
            TotalsCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            PlanLimitationCollectionView.AllowsSelection = true;
            PlanLimitationCollectionView.AllowsMultipleSelection = false;
            PlanLimitationCollectionView.RegisterNibForCell(ClaimDetailConfirmationCellView.Nib, ClaimDetailConfirmationCellView.Key);
            PlanLimitationCollectionView.Delegate = _claimResultPlanLimitationFlowLayout;
            PlanLimitationCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            RequiresCOPLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE, Colors.DARK_RED);
            RequiresAuditLabel.SetLabel(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE, Colors.DARK_RED);
            PlanInformationLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            ClaimDetailsTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            PlanLimitationTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.DARK_GREY_COLOR);
            TotalTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, Constants.ClaimSummarySectionHeaderFontSize, Colors.DARK_GREY_COLOR);

            GscIdLabel.SetLabel(Constants.NUNITO_MEDIUM, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);
            ParticipantNameLabel.SetLabel(Constants.NUNITO_MEDIUM, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);
            SubmissionDateLabel.SetLabel(Constants.NUNITO_MEDIUM, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);

            GscIdValue.SetLabel(Constants.NUNITO_BLACK, Constants.ClaimSummaryInfoLabelFontSize, Colors.Black);
            ParticipantNameValueLabel.SetLabel(Constants.NUNITO_BLACK, Constants.ClaimSummaryInfoLabelFontSize, Colors.Black);
            SubmissionDateValueLabel.SetLabel(Constants.NUNITO_BLACK, Constants.ClaimSummaryInfoLabelFontSize, Colors.Black);

            SetBindings();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _viewModel.ClaimResultFetched -= _viewModel_ClaimResultFetched;
        }

        private void SetBindings()
        {
            _claimResultDetailsCollectionViewSource = new ClaimResultDetailsCollectionViewSource(ClaimDetailsCollectionView, _viewModel);
            ClaimDetailsCollectionView.DataSource = _claimResultDetailsCollectionViewSource;

            _claimResultPlanLimitationCollectionViewSource = new ClaimResultTotalsCollectionViewSource(PlanLimitationCollectionView, _viewModel.PlanLimitation);
            PlanLimitationCollectionView.DataSource = _claimResultPlanLimitationCollectionViewSource;

            _claimResultTotalsCollectionViewSource = new ClaimResultTotalsCollectionViewSource(TotalsCollectionView, _viewModel.Totals, true);
            TotalsCollectionView.DataSource = _claimResultTotalsCollectionViewSource;

            var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter();
            var boolOppositeValueConverter = new BoolOppositeValueConverter();

            var set = this.CreateBindingSet<ClaimSubmissionResultView, ClaimSubmissionResultViewModel>();
            set.Bind(NavigationItem).For(x => x.Title).To(vm => vm.Title);

            set.Bind(RequiresCOPLabel).For(x => x.Hidden).To(vm => vm.ShowUploadDocuments).WithConversion(boolOppositeValueConverter, null);
            set.Bind(RequiresCOPLabel).To(vm => vm.ShowUploadDocuments).WithConversion("RequiresCopToInfo");

            set.Bind(CopButton).For("Title").To(vm => vm.UploadDocuments);
            set.Bind(CopButton).For(x => x.Hidden).To(vm => vm.ShowUploadDocuments).WithConversion(boolOppositeValueConverter, null);
            set.Bind(CopButton).To(vm => vm.OpenConfirmationOfPaymentCommand);
            set.Bind(CopUploadStackViewTopConstraint)
                .For(x => x.Constant)
                .To(vm => vm.ShowUploadDocuments)
                .WithConversion(boolToDefaultFloatValueConverter, CopUploadStackViewTopConstraint.Constant);
            set.Bind(this).For(x => x.ShowUploadDocuments).To(vm => vm.ShowUploadDocuments);

            set.Bind(PlanInformationLabel).To(vm => vm.PlanInformationTitle);
            set.Bind(ClaimDetailsTitleLabel).To(vm => vm.ClaimDetailTitle);
            set.Bind(PlanLimitationTitleLabel).To(vm => vm.PlanLimitationTitle);
            set.Bind(PlanLimitationTitleLabel).For(x => x.Hidden).To(vm => vm.IsPlanLimitationVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(PlanLimitationLabelTopConstraint)
                .For(x => x.Constant)
                .To(x => x.IsPlanLimitationVisible)
                .WithConversion(boolToDefaultFloatValueConverter, PlanLimitationLabelTopConstraint.Constant);
            set.Bind(PlanLimitationCollectionView).For(x => x.Hidden).To(vm => vm.IsPlanLimitationVisible).WithConversion(boolOppositeValueConverter, null);
            set.Bind(PlanLimitationCollectionViewTopConstraint)
              .For(x => x.Constant)
              .To(x => x.IsPlanLimitationVisible)
              .WithConversion(boolToDefaultFloatValueConverter, PlanLimitationCollectionViewTopConstraint.Constant);

            set.Bind(TotalTitleLabel).To(vm => vm.TotalsTitle);

            set.Bind(GscIdLabel).To(vm => vm.GscIdNumberLabel);
            set.Bind(GscIdValue).To(vm => vm.ClaimSubmissionResult.PlanMemberDisplayID);
            set.Bind(ParticipantNameLabel).To(vm => vm.ParticipantNameLabel);
            set.Bind(ParticipantNameValueLabel).To(vm => vm.ClaimSubmissionResult.ParticipantFullName);
            set.Bind(SubmissionDateLabel).To(vm => vm.SubmissionDateLabel);
            set.Bind(SubmissionDateValueLabel).To(vm => vm.ClaimSubmissionResult.SubmissionDate);

            set.Bind(_claimResultDetailsCollectionViewSource).To(vm => vm.ClaimSubmissionResultDetails);
            set.Bind(_claimResultPlanLimitationCollectionViewSource).To(vm => vm.PlanLimitation);
            set.Bind(_claimResultTotalsCollectionViewSource).To(vm => vm.Totals);

            set.Bind(RequiresAuditLabel).For(x => x.Hidden).To(vm => vm.IsSelectedForAudit).WithConversion(boolOppositeValueConverter, null);
            set.Bind(RequiresAuditLabel).To(vm => vm.RequiredAuditLabel);

            set.Bind(AuditUploadButton).For("Title").To(vm => vm.UploadDocuments);
            set.Bind(AuditUploadButton).For(x => x.Hidden).To(vm => vm.IsSelectedForAudit).WithConversion(boolOppositeValueConverter, null);
            set.Bind(AuditUploadButton).To(vm => vm.OpenConfirmationOfPaymentCommand);
            set.Bind(this).For(x => x.ShowAuditUpload).To(vm => vm.IsSelectedForAudit);

            set.Bind(SubmitAnotherClaimButtonTopConstraint)
                .For(x => x.Constant)
                .To(vm => vm.IsSelectedForAudit)
                .WithConversion(boolToDefaultFloatValueConverter, SubmitAnotherClaimButtonTopConstraint.Constant);
            set.Bind(SubmitAnotherClaimButton).For("Title").To(vm => vm.SubmitAnotherClaim);
            set.Bind(SubmitAnotherClaimButton).To(vm => vm.SubmitAnotherClaimCommand);
            set.Apply();

            _claimResultDetailsCollectionViewSource.ReloadData();
        }

        private void SetClaimResultTotalCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (_viewModel == null || _viewModel.Totals == null)
            {
                return;
            }

            var footerSize = _claimResultTotalsCollectionFlowLayout.GetReferenceSizeForFooter(TotalsCollectionView, _claimDetailsFlowLayout, 0);
            height += footerSize.Height;
            var rowCount = _viewModel.Totals.Count;
            for (int i = 0; i < rowCount; i++)
            {
                var size = _claimResultTotalsCollectionFlowLayout.GetSizeForItem(TotalsCollectionView, _claimDetailsFlowLayout, NSIndexPath.FromRowSection(i, 0));
                height += size.Height;
            }

            if (rowCount > 0)
            {
                height += (rowCount - 1) *
                    _claimResultTotalsCollectionFlowLayout.GetMinimumLineSpacingForSection(TotalsCollectionView, _claimDetailsFlowLayout, 0);
            }
            TotalsCollectionViewHeightConstraint.Constant = height;
        }

        private void SetClaimResultPlanLimitationCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (_viewModel == null || _viewModel.PlanLimitation == null)
            {
                return;
            }

            var rowCount = _viewModel.PlanLimitation.Count;
            for (int i = 0; i < rowCount; i++)
            {
                var size = _claimResultPlanLimitationFlowLayout.GetSizeForItem(PlanLimitationCollectionView,
                    _claimDetailsFlowLayout,
                    NSIndexPath.FromRowSection(i, 0));
                height += size.Height;
            }

            if (rowCount > 0)
            {
                height += (rowCount - 1) *
                    _claimResultPlanLimitationFlowLayout.GetMinimumLineSpacingForSection(PlanLimitationCollectionView,
                    _claimDetailsFlowLayout,
                    0);
            }
            PlanLimitationCollectionViewHeightConstraint.Constant = height;
        }

        private void SetClaimResultDetailsCollectionHeightConstraint()
        {
            nfloat height = 0;
            if (_viewModel == null || _viewModel.ClaimSubmissionResultDetails == null)
            {
                return;
            }

            var sectionCount = _viewModel.ClaimSubmissionResultDetails.Count;
            for (int sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
            {
                var headerSize = _claimResultDetailsCollectionFlowLayout.GetReferenceSizeForHeader(ClaimDetailsCollectionView, _claimDetailsFlowLayout, sectionIndex);
                var sectionInset = _claimResultDetailsCollectionFlowLayout.GetInsetForSection(ClaimDetailsCollectionView, _claimDetailsFlowLayout, sectionIndex);
                var footerSize = _claimResultDetailsCollectionFlowLayout.GetReferenceSizeForFooter(ClaimDetailsCollectionView, _claimDetailsFlowLayout, sectionIndex);

                height += headerSize.Height + footerSize.Height + sectionInset.Bottom + sectionInset.Top;

                var rowCount = _viewModel.ClaimSubmissionResultDetails[sectionIndex].Count;
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    var size = _claimResultDetailsCollectionFlowLayout.GetSizeForItem(ClaimDetailsCollectionView, _claimDetailsFlowLayout, NSIndexPath.FromRowSection(rowIndex, sectionIndex));
                    height += size.Height;
                }

                var lineSpacing = _claimResultDetailsCollectionFlowLayout.GetMinimumLineSpacingForSection(ClaimDetailsCollectionView, _claimDetailsFlowLayout, sectionIndex);
                height += ((rowCount - 1) * lineSpacing);
            }

            ClaimDetailsCollectionViewHeightConstraint.Constant = height;
        }

        private void _viewModel_ClaimResultFetched(object sender, EventArgs e)
        {
            SetClaimResultDetailsCollectionHeightConstraint();
            SetClaimResultPlanLimitationCollectionHeightConstraint();
            SetClaimResultTotalCollectionHeightConstraint();
        }
    }
}