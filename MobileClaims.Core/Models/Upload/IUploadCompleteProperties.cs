using System;
namespace MobileClaims.Core.Models.Upload
{
    public interface IUploadCompleteProperties : IUploadProperties
    {
        string UploadCompletedNote { get; }
        string BackToMyClaimsText { get; }
    }
}
