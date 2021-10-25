using System.IO;

namespace MobileClaims.Core.Services
{
    public class FileTypeNotSupportedException : FileNotFoundException
    {
        public FileTypeNotSupportedException()
        {
        }

        public FileTypeNotSupportedException(string v) : base(v)
        {
        }
    }
}