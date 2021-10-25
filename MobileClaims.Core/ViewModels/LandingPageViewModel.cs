using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.Requests;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels
{
    public class LandingPageViewModel : ViewModelBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IMenuService _menuService;
        private readonly IContactUsService _contactUsService;
        private readonly IUserDialogs _dialogService;
        private readonly IMvxMessenger _messenger;

        private readonly object _sync = new object();

        private Dictionary<MenuItemRel, IMvxAsyncCommand> _menuToCommandsMapping;

        private readonly MvxBundle _requestedByMenuBundle = new MvxBundle(new Dictionary<string, string>
        {
            { NavigationRequestTypes.RequestedBy, NavigationRequestTypes.Menu }
        });

        public IMvxAsyncCommand ShowMyAccountCommand { get; }
        public IMvxAsyncCommand ShowBalancesCommand { get; }
        public IMvxAsyncCommand ShowFindServiceProviderCommand { get; }
        public IMvxAsyncCommand ShowFindPharmaciesProviderCommand { get; }
        public IMvxAsyncCommand ShowDrugLookupCommand { get; }
        public IMvxAsyncCommand ShowTermsAndConditionsCommand { get; }
        public IMvxAsyncCommand ShowSettingsCommand { get; }
        public IMvxAsyncCommand ShowClaimsCommand { get; }
        public IMvxAsyncCommand ShowBenefitsCommand { get; }
        public IMvxAsyncCommand ShowAlertsCommand { get; }
        public IMvxAsyncCommand ShowChangeForLifeCommand { get; }
        public IMvxAsyncCommand ContactUsCommand { get; }
        public IMvxAsyncCommand SureHealthCommand { get; }
        public IMvxAsyncCommand SupportCenterCommand { get; }
        public IMvxAsyncCommand ShowDirectDepositCommand { get; }

        public IMvxAsyncCommand LogoutCommand { get; }

        public IList<MenuItem> DynamicMenuItems { get; set; }

        public MvxAsyncCommand<MenuItem> OpenMenuItemCommand { get; }

        public Action OnMapLocationRequest { get; set; }

        public bool HasMapLocationPermission { get; set; }

        public LandingPageViewModel(
            ILoginService loginService,
            IDeviceService deviceService,
            IMenuService menuService,
            IContactUsService contactUsService,
            IMvxMessenger messenger,
            IUserDialogs dialogService)
        {
            _loginservice = loginService;
            _messenger = messenger;
            _deviceService = deviceService;
            _menuService = menuService;
            _contactUsService = contactUsService;
            _dialogService = dialogService;

            DynamicMenuItems = _menuService.MenuItems.OrderBy(x => x.SortOrder).ToList();
            var shouldShowCounter = DynamicMenuItems.Any(x => x.Count > 0);

            foreach (var item in DynamicMenuItems)
            {
                item.ShouldShowCounter = shouldShowCounter;
            }

            ShowClaimsCommand = new MvxAsyncCommand(ExecuteShowClaims);
            ShowMyAccountCommand = new MvxAsyncCommand(ExecuteShowMyAccount);
            ShowBalancesCommand = new MvxAsyncCommand(() => ExecuteShowMenu<ChooseSpendingAccountTypeViewModel>());
            ShowFindServiceProviderCommand = new MvxAsyncCommand(ExecuteShowHealthProvider);
            ShowFindPharmaciesProviderCommand = new MvxAsyncCommand(ExecuteShowFindPharmaciesProvider);
            ShowDrugLookupCommand = new MvxAsyncCommand(() => ExecuteShowMenu<DrugLookupModelSelectionViewModel>());
            ShowTermsAndConditionsCommand = new MvxAsyncCommand(() => ExecuteShowMenu<TermsAndConditionsViewModel>());
            ShowSettingsCommand = new MvxAsyncCommand(() => ExecuteShowMenu<SettingsViewModel>());
            ShowBenefitsCommand = new MvxAsyncCommand(() => ExecuteShowMenu<EligibilityCheckTypesViewModel>());
            ShowAlertsCommand = new MvxAsyncCommand(() => ExecuteShowMenu<AuditListViewModel>());
            ShowChangeForLifeCommand = new MvxAsyncCommand(ExecuteShowChangeForLife);
            ContactUsCommand = new MvxAsyncCommand(ExecuteContactUs);
            SureHealthCommand = new MvxAsyncCommand(ExecuteSureHealth);
            SupportCenterCommand = new MvxAsyncCommand(ExecuteSupportCenter);
            LogoutCommand = new MvxAsyncCommand(ExecuteLogoutCommand);
            OpenMenuItemCommand = new MvxAsyncCommand<MenuItem>(ExecuteOpenMenu);
            ShowDirectDepositCommand = new MvxAsyncCommand(() => ExecuteShowMenu<DirectDepositViewModel>());
            PrepareMapping();
        }

        private async Task ExecuteShowMenu<T>()
            where T : ViewModelBase
        {
            await ShowMainNavViewModelForIos();
            await ShowViewModel<T>(_requestedByMenuBundle);
        }
        
        public async Task ExecuteShowFindPharmaciesProvider()
        {
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                ///If user has not permission to access location then request for permission and return
                ///else continue
                if (!HasMapLocationPermission)
                {
                    OnMapLocationRequest?.Invoke();
                    return;
                }
            }
            await ShowMainNavViewModelForIos();
            await ShowViewModel<FindHealthProviderViewModel, FindHealthProviderViewModelParameter>(new FindHealthProviderViewModelParameter(ProvidersId.Pharmacy), _requestedByMenuBundle);
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
                _messenger.Publish(new UpdateMapMessage("Pharmacy"));
        }

        private async Task ExecuteShowHealthProvider()
        {
            await ShowMainNavViewModelForIos();
            await ShowViewModel<FindHealthProviderViewModel, FindHealthProviderViewModelParameter>(new FindHealthProviderViewModelParameter(ProvidersId.Favourites), _requestedByMenuBundle);
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
                _messenger.Publish(new UpdateMapMessage("Favourites"));
        }

        private async Task ExecuteShowMyAccount()
        {
            await ShowMainNavViewModelForIos();
            await ShowViewModel<CardViewModel, CardViewModelParameter>(new CardViewModelParameter(false), _requestedByMenuBundle);
        }

        private async Task ExecuteShowClaims()
        {
            await ShowViewModel<ChooseClaimOrHistoryViewModel>(_requestedByMenuBundle);
        }

        private async Task ExecuteShowChangeForLife()
        {
            await ShowViewModel<ChangeForLifeViewModel>(_requestedByMenuBundle);
        }

        private async Task ExecuteContactUs()
        {
            _contactUsService.DisplayContactUsPage();
        }

        private async Task ExecuteSureHealth()
        {
            await ShowViewModel<SureHealthViewModel>(_requestedByMenuBundle);
        }

        private async Task ExecuteSupportCenter()
        {
            await ShowViewModel<SupportCenterViewModel>(_requestedByMenuBundle);
        }

        private async Task ExecuteLogoutCommand()
        {
            await Logout();
        }

        private async Task ShowMainNavViewModelForIos()
        {
            if (_deviceService.CurrentDevice == GSCHelper.OS.iOS)
            {
                await ShowViewModel<MainNavigationViewModel>();
            }
        }

        private void PrepareMapping()
        {
            _menuToCommandsMapping = new Dictionary<MenuItemRel, IMvxAsyncCommand>()
            {
                {MenuItemRel.MyClaimsPage, ShowClaimsCommand},
                {MenuItemRel.IdCardPage , ShowMyAccountCommand},
                {MenuItemRel.DirectDepositPage , ShowDirectDepositCommand},
                {MenuItemRel.DrugsOnTheGo, ShowDrugLookupCommand},
                {MenuItemRel.MyBenefitsPage, ShowBenefitsCommand},
                {MenuItemRel.FindAProviderPage, ShowFindServiceProviderCommand},
                {MenuItemRel.FindAPharmacyPage, ShowFindPharmaciesProviderCommand},
                {MenuItemRel.MyAlertsPage, ShowAlertsCommand},
                {MenuItemRel.MyBalancesPage, ShowBalancesCommand},
                {MenuItemRel.Change4lifePage, ShowChangeForLifeCommand},
                {MenuItemRel.TermAndConditionsPage, ShowTermsAndConditionsCommand},
                {MenuItemRel.MySettingsPage, ShowSettingsCommand},
                {MenuItemRel.ContactUsExternal, ContactUsCommand },
                {MenuItemRel.SureHealthPage, SureHealthCommand },
                {MenuItemRel.SupportCentreAccessPage, SupportCenterCommand },
                {MenuItemRel.LogoutPage, LogoutCommand },
            };
        }

        private async Task ExecuteOpenMenu(MenuItem selectedMenuItem)
        {
            var rel = selectedMenuItem.Links.FirstOrDefault()?.MenuItemRel;
            if (rel == MenuItemRel.NotDefined)
            {
                _dialogService.Alert(Resource.GenericErrorDialogMessage);
            }
            else
            {
                await _menuToCommandsMapping[rel.Value]?.ExecuteAsync();
            }
        }
    }
}