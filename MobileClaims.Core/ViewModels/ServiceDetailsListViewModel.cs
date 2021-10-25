using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MobileClaims.Core.Helpers;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ServiceDetailsListViewModel : ViewModelBase<IList<HealthProviderSummaryModel>>
    {
        private bool _shoulCenterToSelectedProvider = false;
        private readonly IMvxMessenger _messenger;
        private readonly IHealthProviderService _healthProviderService;
        private readonly IUserDialogs _userDialogs;
        private readonly ISearchHealthProviderService _searchHealthProviderService;
        private readonly MvxSubscriptionToken _refreshProvidersSubscriptionToken;
        private readonly MvxSubscriptionToken _favouriteProviderSubscriptionToken;

        public event EventHandler ServiceProvidersChanged;
        public event EventHandler<int> FavouriteProviderRefreshed;

        public MvxCommand CenterMapToProvider => new MvxCommand(ExecuteCenterMapToProvider, CanExecuteCenterMapToProvider);

        public HealthProviderSummaryModel SelectedServiceProvider { get; set; }

        public ServiceDetailsListViewModel(IMvxMessenger messenger,
            IHealthProviderService healthProviderService,
            IUserDialogs userDialogs,
            ISearchHealthProviderService searchHealthProviderService)
        {
            _messenger = messenger;
            _healthProviderService = healthProviderService;
            _userDialogs = userDialogs;
            _searchHealthProviderService = searchHealthProviderService;
            _refreshProvidersSubscriptionToken = _messenger.Subscribe<HealthProvidersRefreshedMessage>(OnHealthProvidersRefreshed);
            _favouriteProviderSubscriptionToken = _messenger.Subscribe<FavouriteHealthProviderChange>(OnFavouriteHealthProviderChanged);
        }

        public override void Prepare(IList<HealthProviderSummaryModel> parameters)
        {
            base.Prepare(parameters);
        }

        private void OnFavouriteHealthProviderChanged(FavouriteHealthProviderChange msg)
        {
            var provider = ViewModelParameter?.SingleOrDefault(p => p.Model.ProviderId == msg.ProviderId);

            if (provider != null)
            {
                provider.IsFavourite = msg.IsFavourite;
                FavouriteProviderRefreshed?.Invoke(this, ViewModelParameter.IndexOf(provider));
            }
        }

        private void OnHealthProvidersRefreshed(HealthProvidersRefreshedMessage msg)
        {
            ViewModelParameter = msg.ServiceProviders;
            ServiceProvidersChanged?.Invoke(this, EventArgs.Empty);
        }

        public MvxCommand CloseCommand => new MvxCommand(Close);
        public MvxCommand CloseAndCenterMapToProviderCommand => new MvxCommand(ExecuteCloseAndCenterMapToProvider);

        private void Close()
        {
            Close(this);
            _messenger.Unsubscribe<HealthProvidersRefreshedMessage>(_refreshProvidersSubscriptionToken);
            _messenger.Unsubscribe<FavouriteHealthProviderChange>(_favouriteProviderSubscriptionToken);
        }

        private void ExecuteCloseAndCenterMapToProvider()
        {
            CloseCommand.Execute(null);
            if (CenterMapToProvider.CanExecute(null))
            {
                CenterMapToProvider.Execute(null);
            }
        }

        private void ExecuteCenterMapToProvider()
        {
            _messenger.Publish(new CenterMapToProviderArgs(this)
            {
                ProviderId = this.SelectedServiceProvider.Model.ProviderId
            });
        }

        private bool CanExecuteCenterMapToProvider()
        {
            return this.SelectedServiceProvider != null && this._shoulCenterToSelectedProvider;
        }

        public async Task ShowDetails(HealthProviderSummaryModel provider)
        {
            try
            {
                var detailedProvider = await _healthProviderService.GetProviderDetails(provider.Model.ProviderId);
                if (detailedProvider == null)
                {
                    throw new NullResponseException();
                }
                var detailedProviderViewModel = new HealthProviderSummaryModel(detailedProvider, _searchHealthProviderService);
                // var bundle = detailedProviderViewModel.PrepareShowDetailsCommand();
                await ShowViewModel<ProviderDetailsInformationViewModel, HealthProviderSummaryModel>(detailedProviderViewModel);
            }
            catch (NullResponseException)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
        }

        public async Task ToggleFavouriteDetails(HealthProviderSummaryModel provider)
        {
            try
            {
                await _healthProviderService.ToggleFavouriteProvider(provider.Model.ProviderId);
                provider.IsFavourite = !provider.IsFavourite;
                _messenger.Publish(new FavouriteHealthProviderChange(this)
                {
                    IsFavourite = provider.IsFavourite,
                    ProviderId = provider.Model.ProviderId
                });
            }
            catch (ApiException)
            {
                await _userDialogs.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
        }

        public async Task SelectAndCenterServiceProvider(HealthProviderSummaryModel serviceProvider)
        {
            this._shoulCenterToSelectedProvider = true;
            this.SelectedServiceProvider = serviceProvider;
            if (CenterMapToProvider.CanExecute(null))
            {
                CenterMapToProvider.Execute(null);
            }
            this._shoulCenterToSelectedProvider = false;
        }

        public async Task SelectServiceProvider(HealthProviderSummaryModel serviceProvider)
        {
            this._shoulCenterToSelectedProvider = true;
            this.SelectedServiceProvider = serviceProvider;
        }
    }
}
