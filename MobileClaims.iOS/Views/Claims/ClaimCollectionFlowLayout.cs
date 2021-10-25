using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Models;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public class ClaimCollectionFlowLayout : UICollectionViewDelegateFlowLayout
    {
        private double _approximateWidthForLabel = 0;
        private double _approximateWidthForValue = 0;
        
        private CGSize _approximateSizeForLabel;
        private CGSize _approximateSizeForValue;

        private UIStringAttributes _labelStringAttributes;
        private UIStringAttributes _valueStringAttributes;

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            nfloat width = 0, height = 0;

            var claimsCollectionSource = collectionView.DataSource as ClaimsCollectionViewSource;
            var item = claimsCollectionSource.ViewModel.Claims[indexPath.Row] as ClaimFormClaimSummary;

            var approximateWidthCountValue = collectionView.Frame.Width - 40;
            var sizeCountValue = new CGSize(approximateWidthCountValue, 1000);
            var attributesCount = new UIStringAttributes()
            {
                Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryClaimCounterFontSize)
            };

            var estimatedFrameCountvalue = new NSString(item.CountValue).
                GetBoundingRect(sizeCountValue, NSStringDrawingOptions.UsesLineFragmentOrigin, attributesCount, null);

            _approximateWidthForLabel = CalculateApproximateWidthForLabel(collectionView);
            _approximateSizeForLabel = GetApproximateSize(_approximateWidthForLabel);
            _labelStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);

            var estimatedFrameServiceDate = GetEstimatedFrameSize(item.ServiceDate, _approximateSizeForLabel, _labelStringAttributes);

            var estimatedFrameServiceDescription = GetEstimatedFrameSize(item.ServiceDescription, _approximateSizeForLabel, _labelStringAttributes);

            var estimatedFrameClaimedAmount = GetEstimatedFrameSize(item.ClaimedAmount, _approximateSizeForLabel, _labelStringAttributes);

            var estimatedFrameOtherPaidAmount = GetEstimatedFrameSize(item.OtherPaidAmount, _approximateSizeForLabel, _labelStringAttributes);

            var estimatedFramePaidAmount = GetEstimatedFrameSize(item.PaidAmount, _approximateSizeForLabel, _labelStringAttributes);

            var estimatedFrameCopay = GetEstimatedFrameSize(item.Copay, _approximateSizeForLabel, _labelStringAttributes);

            var estimatedFrameClaimStatus = GetEstimatedFrameSize(item.ClaimStatus, _approximateSizeForLabel, _labelStringAttributes);

            _approximateWidthForValue = CalculateApproximateWidthForValue(collectionView);
            _approximateSizeForValue = GetApproximateSize(_approximateWidthForValue);
            _valueStringAttributes = GetLabelStringFontAttributes(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize);

            var estimatedFrameServiceDateValue = GetEstimatedFrameSize(item.ServiceDateValue, _approximateSizeForValue, _valueStringAttributes);

            var estimatedFrameServiceDescriptionValue = GetEstimatedFrameSize(item.ServiceDescriptionValue, _approximateSizeForValue, _valueStringAttributes);

            var estimatedFrameClaimedAmountValue = GetEstimatedFrameSize(item.ClaimedAmountValue, _approximateSizeForValue, _valueStringAttributes);

            var estimatedFrameOtherPaidAmountValue = GetEstimatedFrameSize(item.OtherPaidAmountValue, _approximateSizeForValue, _valueStringAttributes);

            var estimatedFramePaidAmountValue = GetEstimatedFrameSize(item.PaidAmountValue, _approximateSizeForValue, _valueStringAttributes);

            var estimatedFrameCopayValue = GetEstimatedFrameSize(item.CopayValue, _approximateSizeForValue, _valueStringAttributes);

            var estimatedFrameClaimStatusValue = GetEstimatedFrameSize(item.ClaimStatusValue, _approximateSizeForValue, _valueStringAttributes);

            width = collectionView.Bounds.Size.Width;

            nfloat otherPaidAmountHeight = GetHeight(estimatedFrameOtherPaidAmount, estimatedFrameOtherPaidAmountValue);
            nfloat paidAmountHeight = GetHeight(estimatedFramePaidAmount, estimatedFramePaidAmountValue);
            nfloat copayHeight = GetHeight(estimatedFrameCopay, estimatedFrameCopayValue);

            height = estimatedFrameCountvalue.Height
                + (Constants.IsPhone() ? 6 : 20)
                + ((estimatedFrameServiceDateValue.Height > estimatedFrameServiceDate.Height) ? estimatedFrameServiceDateValue.Height : estimatedFrameServiceDate.Height)
                + (Constants.IsPhone() ? 9 : 20)
                + ((estimatedFrameServiceDescriptionValue.Height > estimatedFrameServiceDescription.Height) ? estimatedFrameServiceDescriptionValue.Height : estimatedFrameServiceDescription.Height)
                + (Constants.IsPhone() ? 9 : 20)
                + ((estimatedFrameClaimedAmountValue.Height > estimatedFrameClaimedAmount.Height) ? estimatedFrameClaimedAmountValue.Height : estimatedFrameClaimedAmount.Height)
                + (Constants.IsPhone() ? 9 : 20)
                + otherPaidAmountHeight
                + paidAmountHeight
                + copayHeight
                + ((estimatedFrameClaimStatusValue.Height > estimatedFrameClaimStatus.Height) ? estimatedFrameClaimStatusValue.Height : estimatedFrameClaimStatus.Height)
                + (Constants.IsPhone() ? 30 : 40);

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

        private static nfloat GetHeight(CGRect estimatedFrameOtherPaidAmount, CGRect estimatedFrameOtherPaidAmountValue)
        {
            nfloat height = 0;
            if (estimatedFrameOtherPaidAmountValue.Height == 0 && estimatedFrameOtherPaidAmount.Height == 0)
            {
                height = 0;
            }
            else
            {
                height = estimatedFrameOtherPaidAmountValue.Height > estimatedFrameOtherPaidAmount.Height
                    ? estimatedFrameOtherPaidAmountValue.Height : estimatedFrameOtherPaidAmount.Height;
                height += (Constants.IsPhone() ? 9 : 20);
            }

            return height;
        }

        private double CalculateApproximateWidthForLabel(UICollectionView collectionView)
        {
            return (collectionView.Frame.Width * (Constants.IsPhone() ? 0.55 : 0.45)) - 40 - 16;
        }

        private double CalculateApproximateWidthForValue(UICollectionView collectionView)
        {
            return (collectionView.Frame.Width * (Constants.IsPhone() ? 0.45 : 0.55)) - 20 - 16;
        }

        private CGSize GetApproximateSize(double width)
        {
            return new CGSize(width, 1000);
        }

        private UIStringAttributes GetLabelStringFontAttributes(string fontName, float fontSize)
        {
            return new UIStringAttributes()
            {
                Font = UIFont.FromName(fontName, fontSize)
            };
        }

        private CGRect GetEstimatedFrameSize(string value, CGSize approximateSizeForLabel, UIStringAttributes attributes)
        {
            return string.IsNullOrWhiteSpace(value) ? new CGRect(0, 0, 0, 0) :
                new NSString(value).
                GetBoundingRect(approximateSizeForLabel, NSStringDrawingOptions.UsesLineFragmentOrigin, attributes, null);
        }
    }
}