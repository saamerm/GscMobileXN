using System.IO;
using System.IO.Compression;

namespace MobileClaims.Core.Extensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] Compress(this byte[] byteArray)
        {
            var output = new MemoryStream();
            using (var dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(byteArray, 0, byteArray.Length);
            }
            return output.ToArray();
        }
    }
}