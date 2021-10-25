using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using Acr.UserDialogs;

namespace MobileClaims.Core.ViewModels
{
    public class ChangeForLifeTermsAndConditionsViewModel : ViewModelBase
    {
        private readonly IMvxLog _log;
        private readonly IMvxMessenger _messenger;
        //private readonly ParticipantService _participantservice;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly IUserDialogs _userDialog;

        public string Security => BrandResource.Security;
        public string Privacy => BrandResource.Privacy;
        public string Legal => BrandResource.Legal;

        public ChangeForLifeTermsAndConditionsViewModel(IMvxMessenger messenger, 
            IMvxLog log, IUserDialogs userDialog)
        {
            _messenger = messenger;
            _log = log;
            //_participantservice = participantservice;
            _userDialog = userDialog;

            TCContents = new ObservableCollection<string>();
            _shouldcloseself = _messenger.Subscribe<ClearChangeForLifeTermsAndConditionsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimSubmitTermsAndConditionsViewRequested>(_shouldcloseself);
                Close(this);
            });
        }

        private bool _isNotBusy = true;
        public bool IsNotBusy
        {
            get => _isNotBusy;
            set
            {
                _isNotBusy = value;
                RaisePropertyChanged(() => IsNotBusy);
            }
        }

        private string _termsAndConditions = "Terms and Conditions go here";
        public string TermsAndConditions
        {
            get
            {
                return _termsAndConditions;
            }
            set
            {
                _termsAndConditions = value;
                RaisePropertyChanged(() => TermsAndConditions);
            }
        }

        private bool _isAccepted;
        public bool IsAccepted
        {
            get
            {
                return _isAccepted;
            }
            set
            {
                _isAccepted = value;
                RaisePropertyChanged(() => IsAccepted);
            }
        }

        private ObservableCollection<string> _tCContents;
        public ObservableCollection<string> TCContents
        {
            get
            {
                return _tCContents;
            }
            set
            {
                _tCContents = value;
                RaisePropertyChanged(() => TCContents);
            }
        }

        public event EventHandler CloseTermsAndConditionsPopup;
        protected virtual void RaiseCloseTermsAndConditionsPopup(EventArgs e)
        {
            if (this.CloseTermsAndConditionsPopup != null)
            {
                CloseTermsAndConditionsPopup(this, e);
            }
        }

        private void PublishMessages()
        {
            _messenger.Publish<ClearChangeForLifeTermsAndConditionsViewRequested>(new ClearChangeForLifeTermsAndConditionsViewRequested(this));
            RaiseCloseTermsAndConditionsPopup(new EventArgs());
        }

        ICommand _accepttermsandconditions;
        public ICommand AcceptTermsAndConditionsCommand
        {
            get
            {
                if (_accepttermsandconditions == null)
                {
                    _accepttermsandconditions = new MvxCommand( async() =>
                    {
                        base.InvokeOnMainThread(() => IsNotBusy = false);
                        _log.Trace("Attempting to accept conditions");
                        var _participantService = Mvx.IoCProvider.Resolve<IParticipantService>();

                        await _participantService.PutUserAgreement(new UserAgreement(true));
                        var ua = _participantService.UserAgreement;

                        // Show C4L if UA is accepted. Cancel button is handled by CancelTermsAndConditionsCommand.
                        if ((ua != null) && ua.IsAccepted)
                        {
                            await ShowViewModel<ChangeForLifeViewModel>();
                        }
                        else
                        {
							await _userDialog.AlertAsync(Resource.GenericErrorDialogMessage);
                            PublishMessages();
                            await ShowViewModel<DashboardViewModel>();
                        }

                        base.InvokeOnMainThread(() => IsNotBusy = true);
                    });
                }
                return _accepttermsandconditions;
            }
        }

        ICommand _canceltermsandconditions;
        public ICommand CancelTermsAndConditionsCommand
        {
            get
            {
                if (_canceltermsandconditions == null)
                {
                    _canceltermsandconditions = new MvxCommand(() =>
                    {
                        PublishMessages();
                        this.ShowViewModel<DashboardViewModel>();
                    });
                }
                return _canceltermsandconditions;
            }
        }
    }
}