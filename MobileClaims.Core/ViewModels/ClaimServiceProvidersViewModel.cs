using MobileClaims.Core.Entities;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.Services.HCSA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimServiceProvidersViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IProviderLookupService _providerservice;
        private readonly IParticipantService _participantservice;
        private readonly IClaimService _claimservice;
        private readonly IHCSAClaimService _hcsaservice;
        private readonly MvxSubscriptionToken _serviceprovidersretrieved;
		private readonly MvxSubscriptionToken _serviceproviderserror;
        private readonly MvxSubscriptionToken _shouldcloseself;
        private MvxSubscriptionToken _searchcomplete;
        private MvxSubscriptionToken _searcherror;

        private string _initial;
        public string Initial
        {
            get => _initial;
            set
            {
                if (_initial != value)
                {
                    _initial = value;
                    _claimservice.Claim.ServiceProviderSearchInitial = _initial;
                    RaisePropertyChanged(() => Initial);
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    _claimservice.Claim.ServiceProviderSearchLastName = _lastName;
                    RaisePropertyChanged(() => LastName);
                }
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    _claimservice.Claim.ServiceProviderSearchPhoneNumber = _phoneNumber;
                    RaisePropertyChanged(() => PhoneNumber);
                }
            }
        }

        private bool _searching;
        public bool Searching
        {
            get => _searching;
            set
            {
                if (_searching != value)
                {
                    _searching = value;
                    RaisePropertyChanged(() => Searching);

                    _messenger.Publish(new BusyIndicator(this)
                    {
                        Busy = _searching
                    });
                    if (_searchByNameCommand != null)
                        InvokeOnMainThread(() =>
                            (_searchByNameCommand as MvxCommandBase).RaiseCanExecuteChanged());
                    if (_searchByPhoneCommand != null)
                        InvokeOnMainThread(() =>
                            (_searchByPhoneCommand as MvxCommandBase).RaiseCanExecuteChanged());
                }
            }
        }

        public ClaimServiceProvidersViewModel(IMvxMessenger messenger, 
                                              IProviderLookupService providerservice, 
                                              IParticipantService participantservice, 
                                              IClaimService claimservice,
                                              IHCSAClaimService hcsaservice)
        {
            _providerservice = providerservice;
            _messenger = messenger;
            _participantservice = participantservice;
            _claimservice = claimservice;
            _hcsaservice = hcsaservice;

            _serviceprovidersretrieved = _messenger.Subscribe<GetClaimPreviousServiceProvidersComplete>(message =>
            {
                Busy = false;
                ServiceProviders = _providerservice.PreviousClaimServiceProviders;
                if (_claimservice.Claim.Provider != null)
                {
                    SelectedServiceProvider = _claimservice.Claim.Provider;
                }
            });

			_serviceproviderserror = _messenger.Subscribe<GetClaimPreviousServiceProvidersError> (message => {
				Busy = false;
			});

            _shouldcloseself = _messenger.Subscribe<ClearClaimServiceProviderViewRequested>(message =>
            {
                _messenger.Unsubscribe<ClearClaimServiceProviderViewRequested>(_shouldcloseself);
                _messenger.Unsubscribe<GetClaimPreviousServiceProvidersError>(_serviceprovidersretrieved);
                _messenger.Unsubscribe<GetClaimPreviousServiceProvidersComplete>(_serviceproviderserror);
                
                Close(this);
            });

            if (_participantservice.PlanMember != null && !string.IsNullOrEmpty(_participantservice.PlanMember.PlanMemberID) && _providerservice.PreviousClaimServiceProviders == null)
            {
                Busy = true;
                _providerservice.GetClaimPreviousServiceProviders(_participantservice.PlanMember.PlanMemberID, ClaimSubmissionType.ID);
            }
            else
            {
                ServiceProviders = _providerservice.PreviousClaimServiceProviders;
                if (_claimservice.Claim.Provider != null)
                {
                    SelectedServiceProvider = _claimservice.Claim.Provider;
                }
            }
            Busy = false;
        }

        public ClaimSubmissionType ClaimSubmissionType => _claimservice.SelectedClaimSubmissionType;

        private ServiceProvider _selectedServiceProvider;
        public ServiceProvider SelectedServiceProvider
        {
            get => _selectedServiceProvider;
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

        private List<ServiceProvider> _serviceProviders;
        public List<ServiceProvider> ServiceProviders
        {
            get => _serviceProviders;
            set
            {
                _serviceProviders = value;
                _claimservice.Claim.PreviousServiceProviders = _serviceProviders;
                RaisePropertyChanged(() => ServiceProviders);
            }
        }

        private bool _busy;
        public bool Busy
        {
            get => _busy;
            set
            {
                if (_busy != value)
                {
                    _busy = value;
                    _messenger.Publish(new BusyIndicator(this)
                    {
                        Busy = _busy
                    });
                    RaisePropertyChanged(() => Busy);
                }
            }
        }

        public override Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
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
            _messenger.Publish(new ClearClaimTreatmentDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderSearchResultsViewRequested(this));
            _messenger.Publish(new ClearClaimServiceProviderEntryViewRequested(this));
            _messenger.Publish(new ClearClaimParticipantsViewRequested(this));
            _messenger.Publish(new ClearClaimDetailsViewRequested(this));
            _messenger.Publish(new ClearClaimTreatmentDetailsListViewRequested(this));
            _messenger.Publish(new ClearClaimSubmitTermsAndConditionsViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionConfirmationViewRequested(this));
            _messenger.Publish(new ClearClaimSubmissionResultViewRequested(this));
        }

        public ICommand ServiceProviderSelectedCommand
        {
            get
            {
                return new MvxCommand<ServiceProvider>(selectedServiceProvider =>
                {
                    SelectedServiceProvider = selectedServiceProvider;
                    Unsubscribe();
                    PublishMessages();
                    ShowViewModel<ClaimParticipantsViewModel>();
                }, selectedServiceProvider => !Busy);
            }
        }

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler OnNoResults;

        protected virtual void RaiseNoResults(EventArgs e)
        {
            OnNoResults?.Invoke(this, e);
        }

        private ICommand _searchByNameCommand;
        public ICommand SearchByNameCommand
        {
            get
            {
                if (_searchByNameCommand == null)
                {
                    _searchByNameCommand = new MvxCommand(() =>
                    {
                        Searching = true;

                        _searchcomplete = _messenger.Subscribe<SearchForServiceProvidersComplete>(message =>
                        {
                            Searching = false;
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);

                            PublishMessages();
                            ShowViewModel<ClaimServiceProviderSearchResultsViewModel>();
                        });

                        _searcherror = _messenger.Subscribe<SearchForServiceProvidersError>(messsage =>
                        {
                            Searching = false;
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);

                            PublishMessages();
                            ShowViewModel<ClaimServiceProviderSearchResultsViewModel>();
                        });

                        _providerservice.GetServiceProviderByName(ClaimSubmissionType.ID, LastName, Initial);
                    },
                    () => { return !Searching; }
                    );
                }
                return _searchByNameCommand;
            }
        }
        private ICommand _searchByPhoneCommand;
        public ICommand SearchByPhoneCommand
        {
            get
            {
                if (_searchByPhoneCommand == null)
                {
                    _searchByPhoneCommand = new MvxCommand(() =>
                    {
                        Searching = true;

                        _searchcomplete = _messenger.Subscribe<SearchForServiceProvidersComplete>(message =>
                        {
                            Searching = false;
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);

                            PublishMessages();
                            ShowViewModel<ClaimServiceProviderSearchResultsViewModel>();
                        });

                        _searcherror = _messenger.Subscribe<SearchForServiceProvidersError>(messsage =>
                        {
                            Searching = false;
                            _messenger.Unsubscribe<SearchForServiceProvidersComplete>(_searchcomplete);
                            _messenger.Unsubscribe<SearchForServiceProvidersError>(_searcherror);

                            PublishMessages();
                            ShowViewModel<ClaimServiceProviderSearchResultsViewModel>();
                        });

                        _providerservice.GetServiceProviderByPhoneNumber(ClaimSubmissionType.ID, PhoneNumber);
                    },
                    () => { return !Searching; }
                    );
                }
                return _searchByPhoneCommand;
            }
        }

        private void Unsubscribe()
        {
            _messenger.Unsubscribe<GetPreviousServiceProvidersComplete>(_serviceprovidersretrieved);
			_messenger.Unsubscribe<GetPreviousServiceProvidersError>(_serviceproviderserror);
        }
    }
}