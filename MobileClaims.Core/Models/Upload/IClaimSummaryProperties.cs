namespace MobileClaims.Core.Models.Upload
{
    public interface IClaimSummaryProperties : IClaimPropertiesBase
    {
        bool IsUploadSectionVisible { get; }
        string UploadButtonText { get; }
    }
}
