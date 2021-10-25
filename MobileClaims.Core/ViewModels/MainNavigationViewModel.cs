using System.Collections.Generic;
using System.Threading.Tasks;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using MobileClaims.Core.Messages;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class MainNavigationViewModel : ViewModelBase
    {
        public string Dashboard => Resource.BottomNavBarDashboardLabel;
        public string Claims => Resource.BottomNavBarClaimsLabel;
        public string FindProvider => Resource.BottomNavBarHealthProviderLabel;
        public string Menu => Resource.BottomNavBarMenuLabel;

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowClaimsCommand { get; }
        public ICommand ShowFindAProviderCommand { get; }
        public ICommand ShowLandingPageCommand { get; }

        private readonly IMvxMessenger _messenger;
        private readonly IDeviceService _deviceService;

        private readonly MvxBundle _requestedByBundle = new MvxBundle(new Dictionary<string, string>
        {
            { NavigationRequestTypes.RequestedBy, NavigationRequestTypes.BottomNavigationBar }
        });

        public MainNavigationViewModel(IDeviceService deviceService, IMvxMessenger messenger)
        {
            _deviceService = deviceService;
            _messenger = messenger;

            ShowDashboardCommand = new MvxAsyncCommand(ExecuteShowDashboard);
            ShowClaimsCommand = new MvxAsyncCommand(ExecuteShowClaims);
            ShowFindAProviderCommand = new MvxAsyncCommand(ExecuteShowFindAProvider);
            ShowLandingPageCommand = new MvxAsyncCommand(ExecuteShowLandingPage);
        }

        private async Task ExecuteShowDashboard()
        {
            await ShowViewModel<DashboardViewModel>(_requestedByBundle);
        }

        private async Task ExecuteShowClaims()
        {
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                await ShowViewModel<MainNavigationViewModel>();
            }
            await ShowViewModel<ChooseClaimOrHistoryViewModel>(_requestedByBundle);
        }

        private async Task ExecuteShowFindAProvider()
        {
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                await ShowViewModel<MainNavigationViewModel>();
                _messenger.Publish(new UpdateMapMessage("Favourites"));
            }
            await ShowViewModel<FindHealthProviderViewModel>(_requestedByBundle);
        }

        private async Task ExecuteShowLandingPage()
        {
            await ShowViewModel<LandingPageViewModel>(_requestedByBundle);
        }
    }
}