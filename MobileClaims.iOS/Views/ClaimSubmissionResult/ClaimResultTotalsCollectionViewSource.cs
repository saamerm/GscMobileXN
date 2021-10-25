using System;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.iOS.Views.ClaimConfirmation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimSubmissionResult
{
    public class ClaimResultTotalsCollectionViewSource : MvxCollectionViewSource
    {
        internal bool ShowFooter;

        internal readonly ClaimResultDetailQAndAList DataSourceList;

        public ClaimResultTotalsCollectionViewSource(UICollectionView collectionView, ClaimResultDetailQAndAList viewModel, bool showFooter = false)
            : base(collectionView)
        {
            ShowFooter = showFooter;
            DataSourceList = viewModel;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return DataSourceList.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath) as ClaimDetailConfirmationCellView;

            cell.DataContext = item;
            cell.NeedsUpdateConstraints();
            cell.InvalidateIntrinsicContentSize();
            cell.LayoutSubviews();
            return cell;
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            if (ShowFooter && string.Equals(elementKind, "UICollectionElementKindSectionFooter"))
            {
                var footerCellView = collectionView.DequeueReusableSupplementaryView(elementKind,
                    ClaimDetailsFooterCellView.Key, indexPath) as ClaimDetailsFooterCellView;

                footerCellView.FooterText = string.Join(Environment.NewLine + Environment.NewLine,
                    DataSourceList.EOB);
                return footerCellView;
            }
            return new UICollectionReusableView();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return DataSourceList[indexPath.Row];
        }

        private UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var existingCell = collectionView.DequeueReusableCell(ClaimDetailConfirmationCellView.Key, indexPath)
                as ClaimDetailConfirmationCellView;
            if (existingCell != null)
            {
                return existingCell;
            }
            return new UICollectionViewCell();
        }
    }
}
