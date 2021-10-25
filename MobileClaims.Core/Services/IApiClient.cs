using System;
using System.Net.Http;

namespace MobileClaims.Core.Services
{
    public interface IApiClient
    {
        Uri BaseUri { get; set; }
        HttpMethod Method { get; set; }
        bool UseDefaultHeaders { get; set; }
    }
}
