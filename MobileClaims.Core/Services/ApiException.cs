﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services
{
    public class ApiException : NullResponseException
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string ReasonPhrase { get; private set; }
        public HttpResponseHeaders Headers { get; private set; }
        public HttpMethod HttpMethod { get; private set; }
        public Uri Uri { get; private set; }

        public HttpContentHeaders ContentHeaders { get; private set; }

        public string Content { get; private set; }

        public bool HasContent => !string.IsNullOrWhiteSpace(Content);

        ApiException(Uri uri, HttpMethod httpMethod, HttpStatusCode statusCode, string reasonPhrase, HttpResponseHeaders headers) :
            base(createMessage(statusCode, reasonPhrase))
        {
            Uri = uri;
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Headers = headers;
        }

        public T GetContentAs<T>()
        {
            return HasContent ?
                JsonConvert.DeserializeObject<T>(Content) :
                default(T);
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public static async Task<ApiException> Create(Uri uri, HttpMethod httpMethod, HttpResponseMessage response)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var exception = new ApiException(uri, httpMethod, response.StatusCode, response.ReasonPhrase, response.Headers);

            if (response.Content == null)
                return exception;

            try
            {
                exception.ContentHeaders = response.Content.Headers;
                exception.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                response.Content.Dispose();
            }
            catch
            {
                // NB: We're already handling an exception at this point, 
                // so we want to make sure we don't throw another one 
                // that hides the real error.
            }

            return exception;
        }

        static string createMessage(HttpStatusCode statusCode, string reasonPhrase)
        {
            return string.Format("Response status code does not indicate success: {0} ({1}).", (int)statusCode, reasonPhrase);
        }
    }
}
