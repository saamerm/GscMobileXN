namespace MobileClaims.Core.Models.Upload
{
    public interface IClaimSubmitProperties : IClaimPropertiesBase
    {
        bool IsCommentVisible { get; }
        string UploadDocumentType { get; }
    }
}
