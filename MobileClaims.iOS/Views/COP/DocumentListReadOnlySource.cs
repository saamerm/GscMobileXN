using System;
using Foundation;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.ViewModels.Interfaces;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public class DocumentListReadOnlySource<T> : MvxSimpleTableViewSource
        where T: IFileNamesContainer, 
         IClaimSubmitProperties
    {
        private readonly T _viewModel;

        public DocumentListReadOnlySource(UITableView tableView, T viewModel)
            : base(tableView, DocumentListCellView.Key, DocumentListCellView.Key)
        {
            _viewModel = viewModel;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(tableView, indexPath, item);

            var bindableCell = cell as IMvxDataConsumer;
            if (bindableCell != null)
            {
                bindableCell.DataContext = item;
            }
            
            return cell;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var existingCell = (DocumentListCellView)tableView.DequeueReusableCell(DocumentListCellView.Key);
            if (existingCell != null)
            {
                if (!_viewModel.IsCommentVisible && _viewModel.GetType() == typeof(ClaimSubmissionConfirmationViewModel))
                {
                    if (indexPath.Row == _viewModel.Attachments.Count - 1)
                    {
                        existingCell.SeparatorInset = new UIEdgeInsets(0, tableView.Frame.Size.Width, 0, 0);
                    }
                }
                return existingCell;
            }

            return new DocumentListCellView();
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 55;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return _viewModel.Attachments[indexPath.Row];
        }
    }
}