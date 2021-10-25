using System;

namespace MobileClaims.Core.Services
{
    public class NullResponseException : Exception
    {
        public NullResponseException()
        {
        }

        public NullResponseException(string v) : base(v)
        {
        }
    }
}