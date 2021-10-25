using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;

namespace MobileClaims.Core.Services
{
    public interface ILoadFileService
    {
        Task<IEnumerable<DocumentInfo>> OpenFilePickerAsync(bool isMediaFileOnly);
    }
}
