using MobileClaims.Core.Entities;
using System.Net;

namespace MobileClaims.Core.Services.FacadeEntities
{

    public interface IApiClientResponse<T>
    {
        T Results { get; set; }

        string Message { get; set; }

        HttpStatusCode ResponseCode { get; set; }
    }

    public class Response<T> : IApiClientResponse<T>
    {
        public T Results { get; set; }

        public string Message { get; set; }

        public HttpStatusCode ResponseCode { get; set; }
    }

    public class PutAgreementWCSResponse : Response<UserAgreement>
    {    
        public PutAgreementWCSResponse()
        {
        }
    }
}