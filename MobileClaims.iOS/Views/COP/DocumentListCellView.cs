using System;
using Foundation;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public delegate void RemoveDocumentEventHandler(DocumentListCellView sender, RemoveDocumentEventArgs args);

    public partial class DocumentListCellView : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("DocumentListCellView");

        public RemoveDocumentEventHandler RemoveDocumentButtonHandler;

        public int CellIndex;

        public DocumentListCellView()
        {
            InitializeBinding();
        }

        protected DocumentListCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            FileNameLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_FONT_SIZE);
            FileNameLabel.TextColor = Colors.Black;
            RemoveDocumentButton.Hidden = true;
            RemoveDocumentButton.SetImage(UIImage.FromBundle("Delete"), UIControlState.Normal);
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();

            if (RemoveDocumentButtonHandler != null)
            {
                RemoveDocumentButton.TouchUpInside -= ButtonPressed;
            }
            CellIndex = -1;
            RemoveDocumentButtonHandler = null;
        }

        public void BindRemoveButtonHandler(int index, RemoveDocumentEventHandler handler)
        {
            CellIndex = index;
            RemoveDocumentButton.Hidden = false;
            RemoveDocumentButtonHandler = handler;
            RemoveDocumentButton.TouchUpInside += ButtonPressed;
        }

        private void ButtonPressed(object sender, EventArgs e)
        {
            var eventArgs = new RemoveDocumentEventArgs(this, CellIndex);
            RemoveDocumentButtonHandler?.Invoke(this, eventArgs);
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DocumentListCellView, DocumentInfo>();
                set.Bind(FileNameLabel).To(x => x.Name);
                set.Bind(ThumbnailImageView).To(x => x.Type).WithConversion("DocumentTypeToImage");
                set.Apply();
           });
        }
    }
}