using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;

namespace MobileClaims.Core.Services
{
    public class ApiClientHelper
    {
        public async Task<T> ExecuteRequestWithRetry<T>(ApiClient<T> apiClient, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                return await apiClient.ExecuteRequest(cancellationToken: cancellationToken);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var canRetry = await CanRetryWithRefresh();
                    if (canRetry)
                    {
                        return await RetryWithRefresh(async () => await apiClient.ExecuteRequest(cancellationToken: cancellationToken));
                    }
                }
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> ExecuteRequestWithCustomHeaderParameters<T>(ApiClient<T> apiClient, Dictionary<string, string> headers)
        {
            try
            {
                return await apiClient.ExecuteRequestWithUrlEncodedBody(headers);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> CanRetryWithRefresh()
        {
            var loginService = Mvx.IoCProvider.Resolve<ILoginService>();
            return await loginService.RefreshAccessTokenAsync();
        }

        private async Task<T> RetryWithRefresh<T>(Func<Task<T>> refreshFunc)
        {
            return await refreshFunc.Invoke();
        }
    }
}