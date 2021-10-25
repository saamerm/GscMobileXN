﻿using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public class TreatmentDetailsConfirmationCollectionFlowLayout : UICollectionViewDelegateFlowLayout
    {
        private double _approximateWidthForLabel;
        private double _approximateWidthForValue;

        private CGSize _approximateSizeForLabel;
        private CGSize _approximateSizeForValue;

        private UIStringAttributes _labelStringAttributes;
        private UIStringAttributes _valueStringAttributes;

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var treatmentCollectionSource = collectionView.DataSource as TreatmentDetailsConfirmationCollectionViewSource;
            var item = treatmentCollectionSource.ViewModel.ClaimTreatmentInput[indexPath.Section][indexPath.Row];

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
            var treatmentCollectionSource = collectionView.DataSource as TreatmentDetailsConfirmationCollectionViewSource;

            var labelStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_BOLD, Constants.ClaimSummaryClaimCounterFontSize, Colors.Black);
            var estimatedFrameHeader = GetEstimatedFrameSize($"CLAIM {section} of {treatmentCollectionSource.ViewModel.ClaimTreatmentInput.Count}",
                _approximateSizeForLabel, labelStringAttributes);

            var width = collectionView.Bounds.Size.Width;
            nfloat height;
            if (treatmentCollectionSource.ViewModel.ClaimTreatmentInput.Count == 1)
            {
                height = 0;
            }
            else
            {
                height = estimatedFrameHeader.Height;
            }

            return new CGSize(width, height);
        }
        
        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var treatmentCollectionSource = collectionView.DataSource as TreatmentDetailsConfirmationCollectionViewSource;
            var count = treatmentCollectionSource.ViewModel.ClaimTreatmentInput.Count;

            if (count == 1)
            {
                return new UIEdgeInsets(0, 0, 0, 0); 
            }
            else if (section == count - 1)
            {
                return new UIEdgeInsets(Constants.ClaimSubmitTreatmentDetailsSectionTopInset, 0, 0, 0);
            }
            else
            {
                return new UIEdgeInsets(Constants.ClaimSubmitTreatmentDetailsSectionTopInset, 0, Constants.ClaimSubmitTreatmentDetailsSectionBottomInset, 0);
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
                GetBoundingRect(approximateSizeForLabel, NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, attributes, null);
        }
    }
}