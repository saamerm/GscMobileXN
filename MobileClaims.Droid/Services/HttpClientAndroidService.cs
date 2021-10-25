using System;
using System.Collections.Generic;
using System.Net.Http;
using Java.Interop;
using Javax.Net.Ssl;
using MobileClaims.Core.Services;
using ModernHttpClient;

namespace MobileClaims.Droid.Services
{
    public class HttpClientAndroidService : IHttpClientPlatformService
    {
        public HttpClient GetHttpClient(bool isAuth = false)
        {
            HttpClient httpClient;
#if FPPM && (PROD || PROD1 || PROD2)
            if (isAuth)
            {
               httpClient = GetClientForAuth();
            }
            else
            {
               httpClient = new HttpClient(new NativeMessageHandler()
               {
                   UseProxy = true
               });
            }
#else
            httpClient = new HttpClient(new HttpClientHandler()
            {
                UseProxy = true
            });
#endif
            return httpClient;
        }

        // The Auth token generator API Call requires certificate pinning to work
        private HttpClient GetClientForAuth()
        {
            var pin = new Pin();
            pin.Hostname = "idpsso.fppm.qc.ca";

            pin.PublicKeys = new string[] { "sha256/9+LrVep4maepB3QbicKwG/Wg53zmmSXklIJkJ9UC4Dw=" };
            var config = new TLSConfig();
            config.Pins = new List<Pin>();
            config.Pins.Add(pin);
            var httpClient = new HttpClient(new NativeMessageHandler(true, config)
            {
                UseProxy = true
            });
            return httpClient;
        }
    }
}