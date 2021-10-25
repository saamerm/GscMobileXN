using System;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public class TreatmentDetailsConfirmationCollectionViewSource : MvxCollectionViewSource
    {
        internal readonly ClaimSubmissionConfirmationViewModel ViewModel;

        public TreatmentDetailsConfirmationCollectionViewSource(UICollectionView collectionView, ClaimSubmissionConfirmationViewModel viewModel)
            : base(collectionView)
        {
            ViewModel = viewModel;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return ViewModel.ClaimTreatmentInput.Count;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ViewModel.ClaimTreatmentInput[(int)section].Count;
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
            if (elementKind == "UICollectionElementKindSectionHeader")
            {
                var header = (CliamSubmissionConfirmationHeaderCellView)collectionView.DequeueReusableSupplementaryView(elementKind, CliamSubmissionConfirmationHeaderCellView.Key, indexPath);
                header.HeaderText = string.Format(Resource.ClaimCountOutOf, indexPath.Section + 1, ViewModel.ClaimTreatmentInput.Count);
                return header;
            }
            return new UICollectionReusableView();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ViewModel.ClaimTreatmentInput[indexPath.Section][indexPath.Row];
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
