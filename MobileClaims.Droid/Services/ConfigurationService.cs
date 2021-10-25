using System;


using MobileClaims.Core.Services;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace MobileClaims.Droid.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetMapsApiKey()
        {
            //This is taken from AndroidManifest.xml and see also AssemblyInfo.cs for DEBUG/RELEASE binding
            throw new NotImplementedException();
        }

        public string GetIpStackApiKey()
        {
#if DEBUG
            return Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.ApplicationContext
                .GetString(Resource.String.ipStackApiKeyDebug);
#else
            return Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.ApplicationContext
                .GetString(Resource.String.ipStackApiKeyRelease);
#endif
        }
    }
}