using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimServiceProviderSearchResultsViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _providerservice;
        private readonly IClaimService _claimservice;
        private readonly IRehydrationService _rehydrationservice;
        private readonly MvxSubscriptionToken _shouldcloseself;

        public ClaimServiceProviderSearchResultsViewModel(IMvxMessenger messenger, IProviderLookupService providerservice, IClaimService claimservice)
        {
            _providerservice = providerservice;
            _messenger = messenger;
            _claimservice = claimservice;
           
            ClaimServiceProviderProvideInformationViewModel = new ClaimServiceProviderProvideInformationViewModel(messenger, claimservice);
            _rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (_rehydrationservice.Rehydrating && _providerservice.ClaimSearchResults == null)
            {
                _providerservice.ClaimSearchResults = claimservice.Claim.ServiceProviderSearchResults != null ? claimservice.Claim.ServiceProviderSearchResults : null;
            }
            ServiceProviderSearchResults = _providerservice.ClaimSearchResults;
            if (_claimservice.Claim.Provider != null)
            {
                SelectedServiceProvider = _claimservice.Claim.Provider;
            }

            _shouldcloseself = _messenger.Subscribe<ClearClaimServiceProviderSearchResultsViewRequested>((message) =>
            {
                _messenger.Unsubscribe<ClearClaimServiceProviderSearchResultsViewRequested>(_shouldcloseself);
                Close(this);
            });

           
        }

        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimservice.SelectedClaimSubmissionType;
            }
        }

        private ClaimServiceProviderProvideInformationViewModel _claimServiceProviderProvideInformationViewModel;
        public ClaimServiceProviderProvideInformationViewModel ClaimServiceProviderProvideInformationViewModel
        {
            get
            {
                return _claimServiceProviderProvideInformationViewModel;
            }
            set
            {
                _claimServiceProviderProvideInformationViewModel = value;
                RaisePropertyChanged(() => ClaimServiceProviderProvideInformationViewModel);
            }
        }

        private ServiceProvider _selectedServiceProvider;
        public ServiceProvider SelectedServiceProvider
        {
            get { return _selectedServiceProvider; }
            set
            {
                if (_selectedServiceProvider != value)
                {
                    _selectedServiceProvider = value;
                    _claimservice.Claim.Provider = _selectedServiceProvider;
                    RaisePropertyChanged(() => SelectedServiceProvider);
                }
            }
        }

        private List<ServiceProvider> _serviceProviderSearchResults;
        public List<ServiceProvider> ServiceProviderSearchResults
        {
            get
            {
                return _serviceProviderSearchResults;
            }
            set
            {
                _serviceProviderSearchResults = value;
                _claimservice.Claim.ServiceProviderSearchResults = _serviceProviderSearchResults;
                RaisePropertyChanged(() => ServiceProviderSearchResults);
            }
        }

        public override Task RaisePropertyChanged(System.ComponentModel.PropertyChangedEventArgs changedArgs)
        {
            _claimservice.PersistClaim();
            return base.RaisePropertyChanged(changedArgs);
        }

        public override Task RaiseAllPropertiesChanged()
        {
            _claimservice.PersistClaim();
            return base.RaiseAllPropertiesChanged();
        }

        private void PublishMessages()
        {
            _messenger.Publish<Messages.ClearClaimTreatmentDetailsViewRequested>(new MobileClaims.Core.Messages.ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish<Messages.ClearClaimServiceProviderEntryViewRequested>(new MobileClaims.Core.Messages.ClearClaimServiceProviderEntryViewRequested(this));
            _messenger.Publish<Messages.ClearClaimParticipantsViewRequested>(new MobileClaims.Core.Messages.ClearClaimParticipantsViewRequested(this));
            _messenger.Publish<Messages.ClearClaimDetailsViewRequested>(new MobileClaims.Core.Messages.ClearClaimDetailsViewRequested(this));
            _messenger.Publish<Messages.ClearClaimTreatmentDetailsListViewRequested>(new MobileClaims.Core.Messages.ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish<Messages.ClearClaimSubmitTermsAndConditionsViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish<Messages.ClearClaimSubmissionConfirmationViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish<Messages.ClearClaimSubmissionResultViewRequested>(new MobileClaims.Core.Messages.ClearClaimSubmissionResultViewRequested(this));
        }

        public ICommand ServiceProviderSelectedCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((selectedServiceProvider) =>
                {
                    SelectedServiceProvider = selectedServiceProvider;
                    PublishMessages();
                    this.ShowViewModel<ClaimParticipantsViewModel>();
                });
            }
        }

        public ICommand ServiceProviderSelectedGotoWindowsDetailViewModelCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>((selectedServiceProvider) =>
                {
                    SelectedServiceProvider = selectedServiceProvider;

                    PublishMessages();
                    this.ShowViewModel<ClaimDetailsViewModel>();
                });
            }
        }

        public ICommand NavigateToViewModelCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    PublishMessages();
                    this.ShowViewModel<ClaimServiceProviderProvideInformationViewModel>();
                });
            }
        }

        // Created for windows to skip ClaimServiceProviderProvideInformationViewModel and go directly to input
        public ICommand EnterServiceProviderCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    PublishMessages();
                    this.ShowViewModel<ClaimServiceProviderEntryViewModel>();
                });
            }
        }
    }
}
