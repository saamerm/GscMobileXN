using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimParticipants
{
    public class ClaimParticipantCollectionFlowLayout : UICollectionViewDelegateFlowLayout
    {
        private double _approximateWidthForLabel;

        private CGSize _approximateSizeForLabel;

        private UIStringAttributes _labelStringAttributes;

        public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            //var claimsCollectionSource = collectionView.DataSource as ClaimParticipantCollectionViewSource;
            //var item = claimsCollectionSource.Participants[indexPath.Row] as Participant;

            //_approximateWidthForLabel = CalculateApproximateWidthForLabel(collectionView);
            //_approximateSizeForLabel = GetApproximateSize(_approximateWidthForLabel);
            //_labelStringAttributes = GetLabelStringFontAttributes(Constants.LEAGUE_GOTHIC, Constants.LIST_ITEM_FONT_SIZE, Colors.LightGrayColorForLabelValuePair);

            //var estimatedFrameQuestion = GetEstimatedFrameSize(item.FullName, _approximateSizeForLabel, _labelStringAttributes);

            var width = collectionView.Bounds.Size.Width;

            return new CGSize(width, 55);
        }

        public override UIEdgeInsets GetInsetForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return new UIEdgeInsets(0, 0, 0, 0);
        }

        public override nfloat GetMinimumLineSpacingForSection(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            return 20; // Constants.ClaimSubmitTreatmentDetailsCollectionViewLineSpacing;
        }

        private double CalculateApproximateWidthForLabel(UICollectionView collectionView)
        {
            return collectionView.Frame.Width - 10;
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