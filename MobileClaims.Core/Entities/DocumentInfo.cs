using System;
using System.IO;
using System.Linq;
using MobileClaims.Core.Constants;

namespace MobileClaims.Core.Entities
{
    public enum DocumentType
    {
        None,
        TextBasedDocument,
        Image
    }

    public class DocumentInfo
    {
        public string Name { get; set; }
        public byte[] ByteContent { get; set; }
        public DocumentType Type
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    return StringConstants.MediaExtensions.Contains(Path.GetExtension(Name), StringComparer.OrdinalIgnoreCase) ?
                        DocumentType.Image : 
                        DocumentType.TextBasedDocument;
                }
                return DocumentType.None;
            }
        }
    }
}
