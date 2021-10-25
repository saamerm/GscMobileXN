using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.AuditClaims
{
    public class AuditClaimsCollectionViewSource : MvxCollectionViewSource
    {
        private readonly AuditListViewModel _viewModel;

        public AuditClaimsCollectionViewSource(UICollectionView collectionView, AuditListViewModel viewModel)
            : base(collectionView)
        {
            _viewModel = viewModel;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _viewModel.AuditClaims.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath);

            var bindableCell = cell as AuditClaimCellView;
            if (bindableCell != null)
            {
                bindableCell.DataContext = item;
                bindableCell.DueDateLabelText = _viewModel.ClaimDueDateLabel;
                bindableCell.SubmissionDateLabelText = _viewModel.ClaimSubmissionDateLabel;
            }
            return cell;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return _viewModel.AuditClaims[indexPath.Row];
        }

        [Export("collectionView:shouldHighlightItemAtIndexPath:")]
        public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        [Export("collectionView:shouldSelectItemAtIndexPath:")]
        public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {            
            nfloat width = 0, height = 0;
            if (Constants.IsPhone())
            {
                width = collectionView.Bounds.Size.Width;
                height = Constants.AuditClaimsCellHeight;
            }
            else
            {
                width = collectionView.Bounds.Size.Width / 2 - Constants.RecentClaimsCollectionViewCellSpacing;
                height = Constants.AuditClaimsCellHeight;
            }
            return new CGSize(width, height);
        }

        private UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var existingCell = collectionView.DequeueReusableCell(AuditClaimCellView.Key, indexPath) as AuditClaimCellView;
            if (existingCell != null)
            {
                return existingCell;
            }
            return new UICollectionViewCell();
        }
    }
}
