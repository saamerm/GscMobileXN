using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimSubmissionResult
{
    public class ClaimResultDetailsCollectionFlowLayout : UICollectionViewDelegateFlowLayout
    {
        private double _approximateWidthForLabel;
        private double _approximateWidthForValue;

        private CGSize _approximateSizeForHeader;
        private CGSize _approximateSizeForFooter;
        private CGSize _approximateSizeForLabel;
        private CGSize _approximateSizeForValue;

        private UIStringAttributes _labelStringAttributes;
        private UIStringAttributes _valueStringAttributes;

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var treatmentCollectionSource = collectionView.DataSource as ClaimResultDetailsCollectionViewSource;
            var item = treatmentCollectionSource.ViewModel.ClaimSubmissionResultDetails[indexPath.Section][indexPath.Row];

            _approximateWidthForLabel = CalculateApproximateWidthForLabel(collectionView);
            _approximateSizeForLabel = GetApproximateSize(_approximateWidthForLabel);
            _labelStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);

            var estimatedFrameQuestion = GetEstimatedFrameSize(item.Question, _approximateSizeForLabel, _labelStringAttributes);

            _approximateWidthForValue = CalculateApproximateWidthForValue(collectionView);
            _approximateSizeForValue = GetApproximateSize(_approximateWidthForValue);
            _valueStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize, Colors.Black);

            var estimatedFrameAnswer = GetEstimatedFrameSize(item.Answer, _approximateSizeForValue, _valueStringAttributes);

            var width = collectionView.Bounds.Size.Width;
            var labelFont = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            var valueFont = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize);
            var height = estimatedFrameAnswer.Height > estimatedFrameQuestion.Height
                ? estimatedFrameAnswer.Height
                : estimatedFrameQuestion.Height;
            return new CGSize(width, height);
        }

        public override CGSize GetReferenceSizeForHeader(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var claimDetailsCollectionSource = collectionView.DataSource as ClaimResultDetailsCollectionViewSource;

            var width = collectionView.Bounds.Size.Width;
            nfloat height;

            if (claimDetailsCollectionSource.ViewModel.ClaimSubmissionResultDetails.Count == 1)
            {
                height = 0;
            }
            else
            {
                _approximateSizeForHeader = GetApproximateSize(collectionView.Bounds.Size.Width);

                var labelStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_BOLD, Constants.ClaimSummaryClaimCounterFontSize, Colors.Black);
                var estimatedFrameHeader = GetEstimatedFrameSize($"CLAIM {section} of {claimDetailsCollectionSource.ViewModel.ClaimSubmissionResultDetails.Count}",
                    _approximateSizeForHeader, labelStringAttributes);

                height = estimatedFrameHeader.Height;
            }

            return new CGSize(width, height);
        }

        public override CGSize GetReferenceSizeForFooter(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var claimDetailsCollectionSource = collectionView.DataSource as ClaimResultDetailsCollectionViewSource;

            _approximateSizeForFooter = GetApproximateSize(collectionView.Bounds.Size.Width);

            var labelStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_REGULAR,
                Constants.ClaimSummaryFooterFontSize,
                Colors.LightGrayColorForLabelValuePair);

            var footerText = string.Join(Environment.NewLine,
                    claimDetailsCollectionSource.ViewModel.ClaimSubmissionResultDetails[(int)section].EOB);

            if (claimDetailsCollectionSource.ViewModel.ClaimSubmissionResultDetails.Count > 0
                   && section < claimDetailsCollectionSource.ViewModel.ClaimSubmissionResultDetails.Count - 1)
            {
                footerText += Environment.NewLine;
            }

            var estimatedFrameFooter = GetEstimatedFrameSize(footerText,
                _approximateSizeForFooter, labelStringAttributes);

            var width = collectionView.Bounds.Size.Width;
            var height = estimatedFrameFooter.Height;
            return new CGSize(width, height);
        }

        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var claimResultDetailsCollectionViewSource = collectionView.DataSource as ClaimResultDetailsCollectionViewSource;
            var count = claimResultDetailsCollectionViewSource.ViewModel.ClaimSubmissionResultDetails.Count;

            var isfooterTextNullOrWhiteSpace = string.IsNullOrWhiteSpace(
                string.Join(Environment.NewLine,
                    claimResultDetailsCollectionViewSource.ViewModel.ClaimSubmissionResultDetails[(int)section].EOB));

            if (count == 1)
            {
                return new UIEdgeInsets(0, 0, 0, 0);
            }
            else 
            {
                if (section == count - 1 || !isfooterTextNullOrWhiteSpace)
                {
                    return new UIEdgeInsets(Constants.ClaimSubmitTreatmentDetailsSectionTopInset, 0, 0, 0);
                }
                else
                {
                    return new UIEdgeInsets(Constants.ClaimSubmitTreatmentDetailsSectionTopInset, 0, Constants.ClaimSubmitTreatmentDetailsSectionBottomInset, 0);
                }
            }
        }

        public override nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return Constants.ClaimSubmitTreatmentDetailsCollectionViewLineSpacing;
        }

        private double CalculateApproximateWidthForLabel(UICollectionView collectionView)
        {
            return (collectionView.Frame.Width * (Constants.IsPhone() ? 0.55 : 0.45)) - 10;
        }

        private double CalculateApproximateWidthForValue(UICollectionView collectionView)
        {
            return (collectionView.Frame.Width * (Constants.IsPhone() ? 0.45 : 0.55));
        }

        private CGSize GetApproximateSize(double width)
        {
            return new CGSize(width, 1000);
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

        private CGRect GetEstimatedFrameSize(string value, CGSize approximateSizeForLabel, UIStringAttributes attributes)
        {
            return string.IsNullOrWhiteSpace(value) ? new CGRect(0, 0, 0, 0) :
                new NSString(value).
                GetBoundingRect(approximateSizeForLabel,
                NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, attributes, null);
        }
    }
}
