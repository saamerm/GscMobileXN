using System;
using Foundation;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public class ClaimDetailsConfirmationCollectionViewSource : MvxCollectionViewSource
    {
        internal readonly ClaimSubmissionConfirmationViewModel ViewModel;

        public ClaimDetailsConfirmationCollectionViewSource(UICollectionView collectionView, ClaimSubmissionConfirmationViewModel viewModel)
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
            return ViewModel.ClaimDetailInput.Count;
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

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ViewModel.ClaimDetailInput[indexPath.Row];
        }

        private UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var existingCell = collectionView.DequeueReusableCell(ClaimDetailConfirmationCellView.Key, indexPath) as ClaimDetailConfirmationCellView;
            if (existingCell != null)
            {
                return existingCell;
            }
            return new UICollectionViewCell();
        }
    }
}
