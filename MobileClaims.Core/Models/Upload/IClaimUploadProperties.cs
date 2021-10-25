using MobileClaims.Core.Models.Upload.Specialized.PerType;

namespace MobileClaims.Core.Models.Upload
{
    public interface IClaimUploadProperties : IClaimPropertiesBase
    {
        string ActionSheetTitle { get; }
    }

    public interface INonRealTimeClaimUploadProperties : IClaimUploadProperties
    {
        NonRealTimeClaimType ClaimType { get; }
    }
}