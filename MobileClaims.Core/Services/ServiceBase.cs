using System;
using System.Threading.Tasks;
using MvvmCross;

namespace MobileClaims.Core.Services
{
    [Obsolete("Please use ApiClientHelper instead of ServiceBase.", false)]
    public class ServiceBase
    {
        public Action<Func<Task>> RetryWithRefresh { get; private set; }

        public ServiceBase()
        {
            RetryWithRefresh = (async (retry) =>
            {
                var loginService = Mvx.IoCProvider.Resolve<ILoginService>();
                var result = await loginService.RefreshAccessTokenAsync();

                if (result)
                {
                    await retry();
                }
                else
                {
                    loginService.Logout();
                }
            });
        }
    }
}
