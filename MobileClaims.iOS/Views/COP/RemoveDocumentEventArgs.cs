using System;

namespace MobileClaims.iOS.Views.COP
{
    public class RemoveDocumentEventArgs : EventArgs
    {
        public int Index { get; private set; }

        public RemoveDocumentEventArgs(DocumentListCellView cell, int index)
        {
            this.Index = index;
        }
    }
}
