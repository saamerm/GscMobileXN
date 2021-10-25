using System.Globalization;
using Acr.UserDialogs;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace MobileClaims.Core
{
    public class App : MvxApplication
    {
        private IDeviceService _deviceservice;

        public GSCHelper.OS DeviceOS { get; set; }

        public App()
        {
            //adding comment to test auto build
            string currLang = CultureInfo.CurrentUICulture.Name;
            if (currLang.Contains("fr") || currLang.Contains("Fr"))
            {
                CultureInfo newCultureInfo = new CultureInfo("fr-CA");
                Resource.Culture = newCultureInfo;
            }
        }

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterType(() => new SearchHealthProviderViewModel());
            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            RegisterCustomAppStart<AppStart>();
            _deviceservice = Mvx.IoCProvider.Resolve<IDeviceService>();
            _deviceservice.SetCurrentDevice(DeviceOS);

        }
    }
}