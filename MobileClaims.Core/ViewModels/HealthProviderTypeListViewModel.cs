using System.Collections.Generic;
using MobileClaims.Core.Messages;
using MobileClaims.Core.Services;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class HealthProviderTypeListViewModel : ViewModelBase<HealthProviderTypeViewModel>
    {
        private readonly IMvxMessenger _messenger;

        public HealthProviderTypeListViewModel(ISearchHealthProviderService searchHealthProviderService,
            IMvxMessenger messenger)
        {
            _messenger = messenger;
            ProviderTypeSelectedCommand = new MvxCommand<HealthProviderTypeViewModel>(ProviderTypeSelected);
            HealthProviderTypeList = searchHealthProviderService.ProviderTypes;
            if (HealthProviderTypeList != null)
            {
                foreach (var parent in HealthProviderTypeList)
                {
                    parent.SelectHealthProviderTypeCommand = ProviderTypeSelectedCommand;

                    foreach (var child in parent.ChildItems)
                    {
                        child.SelectHealthProviderTypeCommand = ProviderTypeSelectedCommand;
                    }
                }
            }
        }

        private List<HealthProviderTypeViewModel> _healthProviderTypeList;

        public List<HealthProviderTypeViewModel> HealthProviderTypeList
        {
            get => _healthProviderTypeList;
            set => SetProperty(ref _healthProviderTypeList, value);
        }

        public override void Prepare(HealthProviderTypeViewModel parameter)
        {
            if (HealthProviderTypeList != null)
            {
                foreach (var parent in HealthProviderTypeList)
                {
                    if (parent.Id == parameter.Id)
                    {
                        parent.IsSelected = true;
                        break;
                    }

                    foreach (var child in parent.ChildItems)
                    {
                        if (child.Id == parameter.Id)
                        {
                            child.IsSelected = true;
                            break;
                        }
                    }
                }
            }

            ViewModelParameter = parameter;
            _messenger.Publish(new ProviderTypeSelectedMessage(this, ViewModelParameter));
        }

        public MvxCommand<HealthProviderTypeViewModel> ProviderTypeSelectedCommand { get; }

        private void ProviderTypeSelected(HealthProviderTypeViewModel healthProviderType)
        {
            ViewModelParameter = healthProviderType;
            _messenger.Publish(new ProviderTypeSelectedMessage(this, ViewModelParameter));
            UnselectAllButThis(healthProviderType);
            Close(this);
        }

        private void UnselectAllButThis(HealthProviderTypeViewModel selectedProvider)
        {
            foreach (var healthProviderType in HealthProviderTypeList)
            {
                foreach (var child in healthProviderType.ChildItems)
                {
                    if (child.Id == selectedProvider.Id)
                    {
                        continue;
                    }

                    child.IsSelected = false;
                }

                if (healthProviderType.Id == selectedProvider.Id)
                {
                    continue;
                }

                healthProviderType.IsSelected = false;
            }
        }
    }
}