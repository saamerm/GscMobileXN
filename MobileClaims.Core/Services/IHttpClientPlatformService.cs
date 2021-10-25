using System.Net.Http;

namespace MobileClaims.Core.Services
{
    public interface IHttpClientPlatformService
    {
        HttpClient GetHttpClient(bool isAuth = false);
    }
}