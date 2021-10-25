using System;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.ViewModels;
using MobileClaims.iOS.Views.ClaimConfirmation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimSubmissionResult
{
    public class ClaimResultDetailsCollectionViewSource : MvxCollectionViewSource
    {
        internal readonly ClaimSubmissionResultViewModel ViewModel;

        public ClaimResultDetailsCollectionViewSource(UICollectionView collectionView, ClaimSubmissionResultViewModel viewModel)
            : base(collectionView)
        {
            ViewModel = viewModel;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return ViewModel.ClaimSubmissionResultDetails.Count;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ViewModel.ClaimSubmissionResultDetails[(int)section].Count;
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
                var header = (CliamSubmissionConfirmationHeaderCellView)collectionView.DequeueReusableSupplementaryView(elementKind,
                    CliamSubmissionConfirmationHeaderCellView.Key, indexPath);
                header.HeaderText = string.Format(Resource.ClaimCountOutOf, indexPath.Section + 1, ViewModel.ClaimSubmissionResultDetails.Count);
                return header;
            }
            else if (elementKind == "UICollectionElementKindSectionFooter")
            {
                var footerCellView = (ClaimDetailsFooterCellView)collectionView.DequeueReusableSupplementaryView(elementKind,
                    ClaimDetailsFooterCellView.Key, indexPath);

                footerCellView.FooterText = string.Join(Environment.NewLine,
                    ViewModel.ClaimSubmissionResultDetails[indexPath.Section].EOB);

                if (ViewModel.ClaimSubmissionResultDetails.Count > 0 
                    && indexPath.Section < ViewModel.ClaimSubmissionResultDetails.Count - 1)
                {
                    footerCellView.FooterText += Environment.NewLine;
                }
                return footerCellView;
            }
            return new UICollectionReusableView();
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ViewModel.ClaimSubmissionResultDetails[indexPath.Section][indexPath.Row];
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
