using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.Services;
using MobileClaims.iOS.Presenters;
using MobileClaims.iOS.Services;
using MobileClaims.Services;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using System.Globalization;
using UIKit;

namespace MobileClaims.iOS
{
    public class Setup : MvxIosSetup<App>
    {
        private UIWindow _window;
        private MvxApplicationDelegate _applicationDelegate;

        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _iscurrentlybusy;

        protected override IMvxIosViewPresenter CreateViewPresenter()
        {
            object presenter = new PhonePresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
            Mvx.IoCProvider.RegisterSingleton((INavViewHeightProvider)presenter);
            return presenter as IMvxIosViewPresenter;
        }

        // TODO: conflicts in the namespaces
        protected override IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions()
            {
                PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject,
            };
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            string lang = NSLocale.PreferredLanguages[0].ToString() + "-CA";

            if (lang.Contains("-CA-CA"))
            {
                lang = lang.Replace("-CA-CA", "-CA");
            }

            var langService = Mvx.IoCProvider.Resolve<ILanguageService>();
            langService.SetCurrentLanguage(lang);

            var deviceService = Mvx.IoCProvider.Resolve<IHttpClientPlatformService>();
            _iscurrentlybusy = _messenger.Subscribe<Core.Messages.BusyIndicator>((message) =>
                {
                    if (message.Busy)
                    {
                        System.Console.WriteLine("BUSY");
                    }
                    else
                    {
                        System.Console.WriteLine("NOT BUSY");
                    }
                });
        }

        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.RegisterSingleton<IBiometricsService>(new BiometricsService());
            Mvx.IoCProvider.RegisterSingleton<IGpsAvailableService>(new GpsAvailableService());
            Mvx.IoCProvider.RegisterSingleton<IConfigurationService>(new ConfigurationService());
            Mvx.IoCProvider.RegisterSingleton<IDirectionsService>(new DirectionsService());
            Mvx.IoCProvider.RegisterSingleton<INativeUrlService>(new IosNativeUrlService());
            Mvx.IoCProvider.RegisterSingleton<IAddToContactsService>(new AddToContactsService());
            Mvx.IoCProvider.RegisterSingleton<ILoadFileService>(new LoadFileService());
            Mvx.IoCProvider.RegisterSingleton<IPlayCoreService>(new PlayCoreService());
            Mvx.IoCProvider.RegisterSingleton<IContactUsService>(new ContactUsService());
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IHttpClientPlatformService>(() => new HttpClientiOSService());
            base.InitializeFirstChance();
        }

        // TODO: Uncomment the following method and fix it
        //public override void Initialize()
        //{
            //    base.Initialize();
            //    string lang = NSLocale.PreferredLanguages[0].ToString() + "-CA";

            //    if (lang.Contains("-CA-CA"))
            //    {
            //        lang = lang.Replace("-CA-CA", "-CA");
            //    }

            //    var langService = Mvx.IoCProvider.Resolve<ILanguageService>();
            //    langService.SetCurrentLanguage(lang);

            //    var registry = Mvx.IoCProvider.Resolve<IMvxTargetBindingFactoryRegistry>();
            //    registry.RegisterPropertyInfoBindingFactory(typeof(CustomUIDatePickerDateTargetBinding),
            //        typeof(UIDatePicker),
            //        "Date");

            //    var key = Mvx.IoCProvider.Resolve<IConfigurationService>().GetMapsApiKey();
            //    MapServices.ProvideAPIKey(key);
        //}

        protected override IMvxApplication CreateApp()
        {
            // Init app and set OS.
            var app = new Core.App();
            app.DeviceOS = GSCHelper.OS.iOS;
            return app;
        }

        // TODO: Uncomment and fix following method
        //protected override IMvxTrace CreateDebugTrace()
        //{
        //    return new DebugTrace();
        //}

    }
}
