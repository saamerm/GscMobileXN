using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.ViewModels;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Dashboard
{
    public class RecentClaimsCollectionViewSource : MvxCollectionViewSource
    {
        private readonly DashboardViewModel _viewModel;

        public RecentClaimsCollectionViewSource(UICollectionView collectionView, DashboardViewModel viewModel)
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
            return _viewModel.RecentClaims.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath);

            var bindableCell = cell as IMvxDataConsumer;
            if (bindableCell != null)
            {
                bindableCell.DataContext = item;
            }
            return cell;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return _viewModel.RecentClaims[indexPath.Row];
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
            var item = _viewModel.RecentClaims[indexPath.Row];

            nfloat width = 0, height = 0;
            if (Constants.IsPhone())
            {
                width = collectionView.Bounds.Size.Width;
                height = item.ActionRequired ? Constants.RecentClaimsWhenActionRequiredCellHeight : Constants.RecentClaimsCellHeightOniPhone;
            }
            else
            {
                width = collectionView.Bounds.Size.Width / 2 - Constants.RecentClaimsCollectionViewCellSpacing;
                height = Constants.RecentClaimsCellHeightOniPad;
            }
            return new CGSize(width, height);
        }

        private UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var existingCell = collectionView.DequeueReusableCell(RecentClaimCellView.Key, indexPath) as RecentClaimCellView;
            if (existingCell != null)
            {
                return existingCell;
            }
            return new UICollectionViewCell();
        }
    }
}