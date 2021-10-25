using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using MobileClaims.Core.Services;

namespace MobileClaims.Core.Util
{
    public class ApiHelper
    {
        public static async Task<T> TryWithRetry<T>(Func<Task<T>> func, ILoginService loginService)
        {
            T result = default(T);
            try
            {
                result =  await func();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode != HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine($"Request - error: {ex.HttpMethod}, {ex.Uri}, {ex.Message}, {ex.Content}");
                    throw;
                }

                var isSuccessful = await loginService.RefreshAccessTokenAsync();

                if (isSuccessful)
                {
                    result = await func();
                }
                else
                {
                    loginService.Logout();
                }
            }
            return result;
        }
    }
}