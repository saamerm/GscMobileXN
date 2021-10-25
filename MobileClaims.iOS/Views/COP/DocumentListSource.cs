using System;
using Foundation;
using MobileClaims.Core.ViewModels.Interfaces;
using MvvmCross.Base;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public class DocumentListSource<T> : MvxSimpleTableViewSource
        where T : ICanDeleteFile, IFileNamesContainer
    {
        private readonly T _viewModel;
       
        public DocumentListSource(UITableView tableView, T viewModel)
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

            var documentListCell = (DocumentListCellView)cell;
            documentListCell.BindRemoveButtonHandler(indexPath.Row, HandleRemoveDocumentEventHandler);
            return cell;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var existingCell = (DocumentListCellView)tableView.DequeueReusableCell(DocumentListCellView.Key);
            if (existingCell != null)
            {
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

        private void HandleRemoveDocumentEventHandler(DocumentListCellView sender, RemoveDocumentEventArgs args)
        {
            if (args != null)
            {
                _viewModel.DeleteCommand.Execute(args.Index);
            }
        }
    }
}