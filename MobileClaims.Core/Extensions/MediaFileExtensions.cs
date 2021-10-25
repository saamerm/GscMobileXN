using System.IO;
using Plugin.Media.Abstractions;

namespace MobileClaims.Core.Extensions
{
    public static class MediaFileExtensions
    {
        public static byte[] GetBytes(this MediaFile mediaFile)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                mediaFile.GetStream().CopyTo(memoryStream);
                mediaFile.Dispose();
                bytes = memoryStream.ToArray();
            }
            return bytes;
        }
    }
}