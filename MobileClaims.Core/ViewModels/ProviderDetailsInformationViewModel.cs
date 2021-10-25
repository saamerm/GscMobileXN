using MobileClaims.Core.Messages;
using MobileClaims.Core.Models;
using MobileClaims.Core.Services;
using Newtonsoft.Json;
using System.Net;
using Acr.UserDialogs;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace MobileClaims.Core.ViewModels
{
    public class ProviderDetailsInformationViewModel : ViewModelBase<HealthProviderSummaryModel>
    {
        private readonly IHealthProviderService _healthProviderService;
        private readonly IMvxMessenger _messenger;
        private readonly IDirectionsService _directionsService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAddToContactsService _addToContactsService;

        public ProviderDetailsInformationViewModel(
            IHealthProviderService healthProviderService, 
            IMvxMessenger messenger, 
            IDirectionsService directionsService,
            IUserDialogs userDialogs,
            IAddToContactsService addToContactsService)
        {
            _healthProviderService = healthProviderService;
            _directionsService = directionsService;
            _userDialogs = userDialogs;
            _messenger = messenger;
            _addToContactsService = addToContactsService;

            HideProviderDetailsInformationCommand = new MvxCommand(HideProviderDetailsInformation);
            ToggleFavouriteProviderCommand = new MvxCommand(OnToggleFavouriteProvider);
            AddToContactsCommand = new MvxCommand(AddToContacts);
            ToggleOpeningHoursCommand = new MvxCommand(ToggleOpeningHours);
        }

        public override void Prepare(HealthProviderSummaryModel parameter)
        {
            base.Prepare(parameter);            
        }

        private bool _isOpeningHoursExpanded;
        public bool IsOpeningHoursExpanded
        {
            get => _isOpeningHoursExpanded;
            set => SetProperty(ref _isOpeningHoursExpanded, value);
        }

        public IMvxCommand HideProviderDetailsInformationCommand { get; }

        public IMvxCommand ToggleFavouriteProviderCommand { get; }

        public IMvxCommand AddToContactsCommand { get; }

        public IMvxCommand ToggleOpeningHoursCommand { get; }

        public MvxCommand ShowDirectionsCommand => new MvxCommand(ShowDirections);

        public string AddToFavouritiesText => Resource.ADDFAVOURITIES;
        public string AddToContactsText => Resource.ADDCONTACT;
        public string ShowDirectionsText => Resource.DIRECTIONS;

        private void HideProviderDetailsInformation()
        {
            Close(this);
        }

        private void ShowDirections()
        {
            _directionsService.ShowDirectionsInMaps(ViewModelParameter.FullAddress,
                new Coordinates(ViewModelParameter.Model.Latitude, ViewModelParameter.Model.Longitude));
        }

        private async void OnToggleFavouriteProvider()
        {
            try
            {
                var response = await _healthProviderService.ToggleFavouriteProvider(ViewModelParameter.Model.ProviderId);
                if (response == null)
                {
                    throw new NullResponseException();
                }
                var result = response.StatusCode == HttpStatusCode.OK;
                if (result)
                {
                    ViewModelParameter.IsFavourite = !ViewModelParameter.IsFavourite;
                    _messenger.Publish(new FavouriteHealthProviderChange(this)
                        {
                            IsFavourite = ViewModelParameter.IsFavourite,
                            ProviderId = ViewModelParameter.Model.ProviderId
                        });
                }
            }
            catch (NullResponseException)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
        }

        private async void AddToContacts()
        {
            if (string.IsNullOrEmpty(ViewModelParameter.Model.Phone))
            {
                return;
            }

            var perms = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);
            if (perms[Permission.Contacts] == PermissionStatus.Granted)
            {
                var addingMessage = _addToContactsService.SaveContact(ViewModelParameter.Model.ProviderTradingName,
                                                                      ViewModelParameter.Model.Phone);

                _userDialogs.Alert(addingMessage);
            }
            else
            {
                _userDialogs.Alert(Resource.GrantPermissionToContacts);
            }
        }

        private void ToggleOpeningHours()
        {
            IsOpeningHoursExpanded = !IsOpeningHoursExpanded;
        }
    }
}
