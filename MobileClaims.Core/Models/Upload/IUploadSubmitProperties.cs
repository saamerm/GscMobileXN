using System;
namespace MobileClaims.Core.Models.Upload
{
    public interface IUploadSubmitProperties : IUploadProperties
    {
        bool IsCommentVisible { get; }
    }
}
