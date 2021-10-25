using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimParticipants
{
    public class ClaimParticipantCollectionViewSource : MvxCollectionViewSource
    {
        internal List<Participant> ParticipantsViewModel;

        public ClaimParticipantCollectionViewSource(UICollectionView collectionView, List<Participant> participantsViewModel)
            : base(collectionView)
        {
            this.ParticipantsViewModel = participantsViewModel;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return ParticipantsViewModel.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath) as ClaimParticipantsCollectionViewCell;

            cell.DataContext = item;
            cell.NeedsUpdateConstraints();
            cell.InvalidateIntrinsicContentSize();
            cell.LayoutSubviews();
            return cell;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return ParticipantsViewModel[indexPath.Row];
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
            var width = collectionView.Bounds.Size.Width;
            var height = Constants.SINGLE_SELECTION_CELL_HEIGHT;

            return new CGSize(width, height);
        }

        private UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var existingCell = collectionView.DequeueReusableCell(ClaimParticipantsCollectionViewCell.Key, indexPath)
                as ClaimParticipantsCollectionViewCell;
            if (existingCell != null)
            {
                return existingCell;
            }
            return new UICollectionViewCell();
        }
    }
}