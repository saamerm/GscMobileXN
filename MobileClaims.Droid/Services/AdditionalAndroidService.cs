using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Services;
using ModernHttpClient;

namespace MobileClaims.Droid.Services
{
    public class AdditionalAndroidService : IAdditionalPlatformService
    {
        public HttpClient GetHttpClient()
        {
            HttpClient httpClient;
#if CLAC && (PROD || PROD1 || PROD2 || QA2 || QA6)
            httpClient = new HttpClient(new HttpClientHandler()
            {
                UseProxy = true
            });
#else
            httpClient = new HttpClient(new NativeMessageHandler()
            {
                UseProxy = true
            });
#endif
            return httpClient;
        }
    }
}