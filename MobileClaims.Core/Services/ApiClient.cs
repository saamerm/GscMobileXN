using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MobileClaims.Core.Entities;
using MvvmCross;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services
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
            Dictionary<string, string> apiParameters = null, 
            object apiBody = null, 
            bool useDefaultHeaders = true)
        {
            BaseUri = baseAddress;
            Method = method;
            UseDefaultHeaders = useDefaultHeaders;
            ApiPath = apiPath;
            ApiBody = apiBody;

            var platformHttpClient = Mvx.IoCProvider.Resolve<IHttpClientPlatformService>();
#if FPPM && (PROD || PROD1 || PROD2)
            if (BaseUri.AbsoluteUri.Contains(GSCHelper.GSC_AUTH_BASE_URL))
                client = platformHttpClient.GetHttpClient(true);
            else
                client = platformHttpClient.GetHttpClient();
#else
            client = platformHttpClient.GetHttpClient();
#endif
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

        public async Task<T> ExecuteRequestWithUrlEncodedBody(Dictionary<string, string> headers = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(Method, string.Format("{0}{1}", this.BaseUri, ApiPath));
            if (Method == HttpMethod.Post)
            {
                if (ApiBody != null)
                {
                    string json = ApiBody.ToString();
                    request.Content = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else
                {
                    request.Content = new StringContent(string.Empty);
                }
            }
            if (headers != null)
            {
                foreach (var h in headers)
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Request Exception: {ex}");
#if DEBUG
                Crashes.TrackError(ex);
#endif
                throw;
            }
        }

        public async Task<T> ExecuteRequest(Dictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpRequestMessage request = new HttpRequestMessage(Method, ApiPath);
            if (Method == HttpMethod.Post || Method == HttpMethod.Put)
            {
                if (ApiBody != null)
                {
                    string json = JsonConvert.SerializeObject(ApiBody, new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.IsoDateFormat });
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    Debug.WriteLine($"Request - making with content:{Environment.NewLine}{json}{Environment.NewLine}ApiPath:{Environment.NewLine}{ApiPath}");
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
            if (headers != null)
            {
                foreach (var h in headers)
                {
                    request.Headers.Add(h.Key, h.Value);
                }
            }
            var result = await client.SendAsync(request, cancellationToken);
            Debug.WriteLine($"Request - received: {result}{Environment.NewLine}ApiPath:{Environment.NewLine}{ApiPath}");
            if (result.IsSuccessStatusCode)
            {
                if (typeof(T).Name == "HttpResponseMessage")
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }

                string contentString = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"Request - received with content:{Environment.NewLine}{contentString}{Environment.NewLine}ApiPath:{Environment.NewLine}{ApiPath}");

                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                return JsonConvert.DeserializeObject<T>(contentString, jsonSettings);
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
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

        public async Task<T> UploadDocumentsAsync(IEnumerable<DocumentInfo> attachments, Dictionary<string, string> uploadParameters = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(Method, ApiPath);

            if (Method == HttpMethod.Post && attachments != null)
            {
                var form = new MultipartFormDataContent();

                foreach (var attachment in attachments)
                {
                    var byteContent = new ByteArrayContent(attachment.ByteContent);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(byteContent, "file", attachment.Name);
                }
                request.Content = form;
            }

            if (UseDefaultHeaders)
            {
                request.Headers.Add("Authorization", string.Format("Bearer {0}", loginservice.AuthInfo.AccessToken));
                request.Headers.Add("Accept-Language", languageservice.CurrentLanguage);
                request.Headers.Add("Accept", "multipart/form-data");
            }

            if (uploadParameters != null)
            {
                var uploadParameterString = JsonConvert.SerializeObject(uploadParameters);
                var uploadParameterBytes = Encoding.UTF8.GetBytes(uploadParameterString);
                string base64 = Convert.ToBase64String(uploadParameterBytes);
                request.Headers.Add("UploadParameters", base64);
            }

            var result = await client.SendAsync(request);
            Debug.WriteLine($"Request - received: {result}{Environment.NewLine}ApiPath:{Environment.NewLine}{ApiPath}");
            if (result.IsSuccessStatusCode)
            {
                if (typeof(T).Name == "HttpResponseMessage")
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }

                string contentString = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"Request - received with content:{Environment.NewLine}{contentString}{Environment.NewLine}ApiPath:{Environment.NewLine}{ApiPath}");

                return JsonConvert.DeserializeObject<T>(contentString);
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
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

        public async Task<T> UploadDocumentsAsync(IEnumerable<DocumentInfo> attachments, string base64EncodedRequestString = null)
        {
            var request = new HttpRequestMessage(Method, ApiPath);

            if (Method == HttpMethod.Post && attachments != null)
            {
                var form = new MultipartFormDataContent();

                foreach (var attachment in attachments)
                {
                    var byteContent = new ByteArrayContent(attachment.ByteContent);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(byteContent, "file", attachment.Name);
                }
                request.Content = form;
            }
            
            if (UseDefaultHeaders)
            {
                request.Headers.Add("Authorization", string.Format("Bearer {0}", loginservice.AuthInfo.AccessToken));
                request.Headers.Add("Accept-Language", languageservice.CurrentLanguage);
                request.Headers.Add("Accept", "multipart/form-data");
            }

            if (!string.IsNullOrWhiteSpace(base64EncodedRequestString))
            {
                request.Headers.Add("UploadParameters", base64EncodedRequestString);
            }

            var result = await client.SendAsync(request);
            Debug.WriteLine($"Request - received: {result}");
            if (result.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(HttpResponseMessage))
                {
                    return (T)Convert.ChangeType(result, typeof(T));
                }

                string contentString = await result.Content.ReadAsStringAsync();
                Debug.WriteLine($"Request - received with content:{Environment.NewLine}{contentString}");

                return JsonConvert.DeserializeObject<T>(contentString);
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
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
