using System;
using Android.Widget;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Services;
using MobileClaims.Droid.Binding;
using MobileClaims.Droid.Services;
using MobileClaims.Services;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using MobileClaims.Core;
using MobileClaims.Droid.Presenter;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MobileClaims.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        private MvxAndroidLifetimeMonitor _lifeTimeMonitor;

        protected override IDictionary<string, string> ViewNamespaceAbbreviations
        {
            get
            {
                var toReturn = base.ViewNamespaceAbbreviations;
                toReturn["gsc"] = "MobileClaims.Droid";
                return toReturn;
            }
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies as IList;
                toReturn.Add(typeof(AmountZeroToBlankStringValueConverter).Assembly);
                toReturn.Add(typeof(ObjectNullToVisibleValueConverter).Assembly);
                toReturn.Add(typeof(ObjectToHideValueConverter).Assembly);
                toReturn.Add(typeof(HCSATitleValueConverter).Assembly);
                toReturn.Add(typeof(FullNameToCaptializeValueConverter).Assembly);

                return (IEnumerable<Assembly>)toReturn;
            }
        }

        protected override IMvxIocOptions CreateIocOptions()
        {
            return new MvxIocOptions()
            {
                PropertyInjectorOptions = MvxPropertyInjectorOptions.MvxInject,
            };
        }

        protected override IMvxIoCProvider CreateIocProvider()
        {
            var options = CreateIocOptions();
            var provider = MvxIoCProvider.Initialize(options);
            return provider;
        }

        protected override IMvxApplication CreateApp()
        {
            _lifeTimeMonitor = CreateLifetimeMonitor();
            _lifeTimeMonitor.LifetimeChanged += LifeTimeMonitorOnLifetimeChanged;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Init app and set OS.
            var app = new Core.App();
            app.DeviceOS = GSCHelper.OS.Droid;
            return app;
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();
            Mvx.IoCProvider.RegisterSingleton<IBiometricsService>(new BiometricsService());
            Mvx.IoCProvider.RegisterSingleton<IGpsAvailableService>(new GpsAvailableService());
            Mvx.IoCProvider.RegisterSingleton<IConfigurationService>(new ConfigurationService());
            Mvx.IoCProvider.RegisterSingleton<IDirectionsService>(new DirectionsService());
            Mvx.IoCProvider.RegisterSingleton<INativeUrlService>(new DroidNativeUrlService());
            Mvx.IoCProvider.RegisterSingleton<IAddToContactsService>(new AddToContactsService());
            Mvx.IoCProvider.RegisterSingleton<ILoadFileService>(new LoadFileService());
            Mvx.IoCProvider.RegisterSingleton<IPlayCoreService>(new PlayCoreService());
            Mvx.IoCProvider.RegisterSingleton<IContactUsService>(new ContactUsService());
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IHttpClientPlatformService>(() => new HttpClientAndroidService());
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            // Register the multi region presenter presenter
            var regionPresenter = new MultiRegionPresenter(AndroidViewAssemblies);
            Mvx.IoCProvider.RegisterSingleton<IMultiRegionPresenter>(regionPresenter);
            return regionPresenter;
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            var langService = Mvx.IoCProvider.Resolve<ILanguageService>();
            string currLang = System.Globalization.CultureInfo.CurrentUICulture.Name.ToString();

            if (currLang.StartsWith("fr"))
            {
                langService.SetCurrentLanguage("fr-CA");
            }
            else
            {
                langService.SetCurrentLanguage(currLang);
            }
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxCustomBindingFactory<TextView>(
                "Striked", (textView) => new StrikedTextViewBinding(textView)));
        }

        private void LifeTimeMonitorOnLifetimeChanged(object sender, MvxLifetimeEventArgs e)
        {
            //MvxTrace.Trace(MvxTraceLevel.Diagnostic, e.LifetimeEvent.ToString());
            if (e.LifetimeEvent.Equals(MvxLifetimeEvent.ActivatedFromDisk) || e.LifetimeEvent.Equals(MvxLifetimeEvent.Launching) || e.LifetimeEvent.Equals(MvxLifetimeEvent.Closing))
            {
                var loginService = Mvx.IoCProvider.Resolve<ILoginService>();
                loginService.Logout();
            }
        }
    }
}