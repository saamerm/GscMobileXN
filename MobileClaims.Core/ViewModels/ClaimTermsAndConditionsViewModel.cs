using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using MobileClaims.Core.Messages;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    // TODO: Not used? Is it safe to remove it?
    public class ClaimTermsAndConditionsViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private readonly MvxSubscriptionToken _gettextalterationclaim;
        private readonly MvxSubscriptionToken _gettextalterationclaimerror;
        private readonly MvxSubscriptionToken _notextalterationclaim;

        public ClaimTermsAndConditionsViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;
            _shouldcloseself = _messenger.Subscribe<ClearClaimTermsAndConditionsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimTermsAndConditionsViewRequested>(_shouldcloseself);
                Close(this);
            });
            if (_claimservice.ClaimDisclaimerTextAlterations == null)
            {
                ClaimBusy = true;
                _claimservice.GetClaimDisclaimer(_loginservice.CurrentPlanMemberID);
            }
            else
            {
                ClaimDisclaimer = _claimservice.ClaimDisclaimerTextAlterations;
                ClaimBusy = false;
            }
            _gettextalterationclaim = _messenger.Subscribe<GetTextAlterationClaimDisclaimerComplete>((message) =>
            {
                ClaimDisclaimer = _claimservice.ClaimDisclaimerTextAlterations;
                ClaimBusy = false;
            });

            _gettextalterationclaimerror = _messenger.Subscribe<GetTextAlterationClaimDisclaimerError>((message) =>
            {
                RaiseClaimError(new EventArgs());
                ClaimBusy = false;
            });

            _notextalterationclaim = _messenger.Subscribe<NoTextAlterationClaimDisclaimerFound>((message) =>
            {
                NoClaimAlteration = true;
                ClaimBusy = false;
            });
        }

        public event EventHandler OnClaimError;
        protected virtual void RaiseClaimError(EventArgs e)
        {
            if (this.OnClaimError != null)
            {
                OnClaimError(this, e);
            }
        }


        private bool _noClaimAlteration = false;
        public bool NoClaimAlteration
        {
            get
            {
                return _noClaimAlteration;
            }
            set
            {
                _noClaimAlteration = value;
                RaisePropertyChanged(() => NoClaimAlteration);
            }
        }

        private List<TextAlteration> _claimDisclaimer;
        public List<TextAlteration> ClaimDisclaimer
        {
            get
            {
                return _claimDisclaimer;
            }
            set
            {
                _claimDisclaimer = value;
                RaisePropertyChanged(() => ClaimDisclaimer);
            }
        }
        private bool _claimBusy = false;
        public bool ClaimBusy
        {
            get
            {
                return _claimBusy;
            }
            set
            {
                if (_claimBusy != value)
                {
                    _claimBusy = value;
                    RaisePropertyChanged(() => ClaimBusy);
                }
            }
        }
        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimservice.SelectedClaimSubmissionType;
            }
        }

        private string _termsAndConditions;
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

        public ICommand AcceptTermsAndConditionsCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _messenger.Publish<Messages.ClearClaimTermsAndConditionsViewRequested>(new MobileClaims.Core.Messages.ClearClaimTermsAndConditionsViewRequested(this));
                    Close(this);
                    _messenger.Unsubscribe<GetTextAlterationClaimAgreementComplete>(_gettextalterationclaim);
                    _messenger.Unsubscribe<GetTextAlterationClaimAgreementError>(_gettextalterationclaimerror);
                    _messenger.Unsubscribe<NoTextAlterationClaimAgreementFound>(_notextalterationclaim);
                    this.ShowViewModel<ClaimServiceProvidersViewModel>();
                });
            }
        }
    }
}
