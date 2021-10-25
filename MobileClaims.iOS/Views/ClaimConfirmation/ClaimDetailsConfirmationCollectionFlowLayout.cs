using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public class ClaimDetailsConfirmationCollectionFlowLayout : UICollectionViewDelegateFlowLayout
    {
        private double _approximateWidthForLabel = 0;
        private double _approximateWidthForValue = 0;

        private CGSize _approximateSizeForLabel;
        private CGSize _approximateSizeForValue;

        private UIStringAttributes _labelStringAttributes;
        private UIStringAttributes _valueStringAttributes;

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var claimsCollectionSource = collectionView.DataSource as ClaimDetailsConfirmationCollectionViewSource;
            var item = claimsCollectionSource.ViewModel.ClaimDetailInput[indexPath.Row] as ClaimQuestionAnswerPair;

            _approximateWidthForLabel = CalculateApproximateWidthForLabel(collectionView);
            _approximateSizeForLabel = GetApproximateSize(_approximateWidthForLabel);
            _labelStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);

            var estimatedFrameQuestion = GetEstimatedFrameSize(item.Question, _approximateSizeForLabel, _labelStringAttributes);

            _approximateWidthForValue = CalculateApproximateWidthForValue(collectionView);
            _approximateSizeForValue = GetApproximateSize(_approximateWidthForValue);
            _valueStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize, Colors.Black);

            var estimatedFrameAnswer = GetEstimatedFrameSize(item.Answer, _approximateSizeForValue, _valueStringAttributes);

            var width = collectionView.Bounds.Size.Width;

            var height = estimatedFrameAnswer.Height > estimatedFrameQuestion.Height ?
                estimatedFrameAnswer.Height : estimatedFrameQuestion.Height;
            return new CGSize(width, height);
        }


        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new UIEdgeInsets(0, 0, 0, 0);
        }

        public override nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return Constants.RecentClaimsCollectionViewCellSpacing;
        }

        private double CalculateApproximateWidthForLabel(UICollectionView collectionView)
        {
            return (collectionView.Frame.Width * (Constants.IsPhone() ? 0.55 : 0.45)) - 20 - 16;
        }

        private double CalculateApproximateWidthForValue(UICollectionView collectionView)
        {
            return (collectionView.Frame.Width * (Constants.IsPhone() ? 0.45 : 0.55)) - 16;
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
                GetBoundingRect(approximateSizeForLabel, NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, attributes, null);
        }
    }
}