using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using System;
using Acr.UserDialogs;
using MvvmCross;
using System.Threading.Tasks;

namespace MobileClaims.Core.ViewModels
{
    public class ChangeForLifeViewModel : ViewModelBase
    {
        private readonly ILoginService _loginservice;
        private readonly IParticipantService _participantService;
        private readonly IUserDialogs _userDialog;

        public ChangeForLifeViewModel(ILoginService loginservice, IParticipantService participantService, IUserDialogs userDialog)
        {
            _loginservice = loginservice;
            _participantService = participantService;
            _userDialog = userDialog;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            try
            {
                Busy = true;

                var success = await _loginservice.AuthorizeC4LCredentialsAsync();

                if (success)
                {
                    var ua = _participantService.UserAgreement;
                    if (ua == null)
                    {
                        ua = await _participantService.GetUserAgreement();
                    }

                    if (ua != null && ua.IsAccepted)
                    {
                        C4LOAuth = _loginservice.C4LOAuth;
                        Uri = C4LOAuth.RS_URI;
                    }
                    else
                    {
                        await ShowViewModel<ChangeForLifeTermsAndConditionsViewModel>();
                    }
                }
                else
                {
                    if (_loginservice.C4LOAuth != null
                        && (_loginservice.C4LOAuth.ErrorCode == "1"
                            || _loginservice.C4LOAuth.ErrorDescription.StartsWith("1:",
                                StringComparison.OrdinalIgnoreCase)))
                    {
                        await ShowViewModel<ChangeForLifeTermsAndConditionsViewModel>();
                    }
                    else //don't have access to C4L
                    {
                        await ShowViewModel<ChangeForLifeNoAccessViewModel>();
                    }
                }
            }
            catch (Exception)
            {
                Busy = false;
                Mvx.IoCProvider.Resolve<IUserDialogs>().HideLoading();
                //TODO what should happen if it fails?
                _userDialog.Alert(Resource.GenericErrorDialogMessage);
            }
            finally
            {
                Busy = false;
            }
        }

        public C4LOAuth C4LOAuth { get; set; }

        public string Username { get; } = "mobile@demosites.ca";

        public string Password { get; } = "ct6wgcPp";

        private bool _busy;

        public bool Busy
        {
            get => _busy;

            set
            {
                _busy = value;
                RaisePropertyChanged(() => Busy);
            }
        }

        private string _uri;
        public string Uri
        {
            get => _uri;
            set
            {
                _uri = value;
                RaisePropertyChanged(() => Uri);
            }
        }
    }
}
