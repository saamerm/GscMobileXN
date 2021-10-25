using System;
namespace MobileClaims.Core.Models.Upload
{
    public interface IClaimCompletedProperties : IClaimPropertiesBase
    {
        string UploadSuccess { get; }
        string UploadCompletedNote { get; }
        string BackToMyClaimsText { get; }
        string BackToViewModelType { get; }
    }
}
