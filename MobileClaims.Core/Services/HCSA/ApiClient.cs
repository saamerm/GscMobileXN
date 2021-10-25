using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using System.Net.Http.Headers;

namespace MobileClaims.Core.Services.HCSA
{
    public class ApiClient<T> : IApiClient
    {
        public string ApiPath { get; set; }
        public Uri BaseUri { get; set; }
        public HttpMethod Method { get; set; }
        public bool UseDefaultHeaders { get; set; }
        public object ApiBody { get; set; }

        private HttpClient client;
        private ILoginService loginservice;
        private ILanguageService languageservice;
        public ApiClient(
            Uri baseAddress, 
            HttpMethod method, 
            string apiPath, 
            Dictionary<string, string> apiParameters=null,
            object apiBody = null, 
            bool useDefaultHeaders = true)
        {
            BaseUri = baseAddress;
            Method = method;
            UseDefaultHeaders = useDefaultHeaders;
            ApiPath = apiPath;
            ApiBody = apiBody;

            var platformHttpClient = Mvx.IoCProvider.Resolve<IHttpClientPlatformService>();
            client = platformHttpClient.GetHttpClient();
            client.BaseAddress = baseAddress;
            client.Timeout = TimeSpan.FromSeconds(5 * 60);

            loginservice = Mvx.IoCProvider.Resolve<ILoginService>();
            languageservice = Mvx.IoCProvider.Resolve<ILanguageService>();
            if (apiParameters != null)
            {
                foreach (var parameter in apiParameters)
                {
                    apiPath = apiPath.Replace(string.Format("{{{0}}}", parameter.Key), parameter.Value);
                }
                ApiPath = apiPath;
            }
        }

        public async Task<T>ExecuteRequestWithUrlEncodedBody(Dictionary<string,string> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(Method, string.Format("{0}{1}", this.BaseUri, ApiPath));
            if(Method==HttpMethod.Post)
            {
                if(ApiBody != null)
                {
                    string json = ApiBody.ToString();
#if !FPPM
                    request.Content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
#else
                    request.Content = new StringContent(json);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
#endif
                }
                else
                {
                    request.Content = new StringContent(string.Empty);
                }
            }
            if(headers != null)
            {
                foreach(var h in headers)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }
            HttpResponseMessage result = null;
            try
            {
                result = await client.SendAsync(request);
                if (typeof(T).Name == "HttpResponseMessage")
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Request Exception: {ex}");
#if DEBUG
                Crashes.TrackError(ex);
#endif
                throw;
            }

        }


        public async Task<T>ExecuteRequest(Dictionary<string,string>headers = null)
        {

            HttpRequestMessage request = new HttpRequestMessage(Method, ApiPath);
            if(Method == HttpMethod.Post || Method==HttpMethod.Put)
            {
                if(ApiBody != null)
                {
                    string json = JsonConvert.SerializeObject(ApiBody, new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.IsoDateFormat });
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    request.Content = new StringContent(string.Empty);
                }
            }
            if (UseDefaultHeaders)
            {
                request.Headers.Add("Authorization", string.Format("Bearer {0}", loginservice.AuthInfo.AccessToken));
                request.Headers.Add("Accept-Language", languageservice.CurrentLanguage);
                request.Headers.Add("Accept", "application/json, application/xml, text/json, text/x-json, text/javascript, text/xml");
            }
            if(headers != null)
            {
                foreach (var h in headers)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }
            var result = await client.SendAsync(request);
            if(result.IsSuccessStatusCode)
            {
                if (typeof(T).Name == "HttpResponseMessage")
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
            }
            else if(result.StatusCode==HttpStatusCode.Unauthorized)
            {
                var exception = await ApiException.Create(request.RequestUri, request.Method, result);
                throw exception;
            }
            else
            {
                var exception = await ApiException.Create(request.RequestUri, request.Method, result);
                throw exception;
            }
        }
    }
}
