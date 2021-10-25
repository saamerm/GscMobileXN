using System;
using Foundation;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public class ClaimsCollectionViewSource : MvxCollectionViewSource
    {
        internal readonly AuditClaimSummaryViewModel ViewModel;

        public ClaimsCollectionViewSource(UICollectionView collectionView, AuditClaimSummaryViewModel viewModel)
            : base(collectionView)
        {
            ViewModel = viewModel;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ViewModel.Claims.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath) as ClaimSummaryCellView;

            cell.DataContext = item;
            cell.SetTopConstraints();
            cell.NeedsUpdateConstraints();
            cell.InvalidateIntrinsicContentSize();
            cell.LayoutSubviews();
            return cell;    
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ViewModel.Claims[indexPath.Row];
        }

        private UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var existingCell = collectionView.DequeueReusableCell(ClaimSummaryCellView.Key, indexPath) as ClaimSummaryCellView;
            if (existingCell != null)
            {
                return existingCell;
            }
            return new UICollectionViewCell();
        }
    }
}