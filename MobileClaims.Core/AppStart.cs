using System;
using System.Threading.Tasks;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MobileClaims.Core
{
    public class AppStart : MvxAppStart
    {
        private readonly IDataService _dataService;
        private readonly ILoginService _loginService;
        private readonly IParticipantService _participantService;

        public AppStart(
            IMvxApplication mvxApplication,
            IMvxNavigationService navigationService,
            IDataService dataService,
            ILoginService loginService,
            IParticipantService participantService)
            : base(mvxApplication, navigationService)
        {
            _dataService = dataService;
            _loginService = loginService;
            _participantService = participantService;
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            bool tcAccepted = _dataService.GetAcceptedTC();

            if (!tcAccepted)
            {
                return NavigationService.Navigate<TermsAndConditionsViewModel>();
            }

            DateTime lastLogin = _loginService.LastLogin;
            if (!_loginService.IsLoggedIn)
            {
                return NavigationService.Navigate<LoginViewModel>();
            }

            if (_loginService.IsLoggedIn)
            {
                if (DateTime.Now <= lastLogin.AddMinutes(15))
                {
                    _loginService.LastLogin = DateTime.Now;
                    _participantService.ReloadPersistedParticipant();

                    NavigationService.Navigate<MainNavigationViewModel>().GetAwaiter().GetResult();
                    return NavigationService.Navigate<DashboardViewModel>();
                }
                else
                {
                    _loginService.Logout();
                    return NavigationService.Navigate<LoginViewModel>();
                }
            }
            else
            {
                return NavigationService.Navigate<LoginViewModel>();
            }
        }
    }
}