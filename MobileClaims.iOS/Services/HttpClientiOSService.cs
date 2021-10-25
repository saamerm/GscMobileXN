using System.Net.Http;
using MobileClaims.Core.Services;
using ModernHttpClient;

namespace MobileClaims.iOS.Services
{
    public class HttpClientiOSService : IHttpClientPlatformService
    {
        public HttpClient GetHttpClient(bool isAuth = false)
        {
            return new HttpClient(new NativeMessageHandler
            {
                UseProxy = true
            });
        }
    }
}