using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimServiceProviderProvideInformationViewModel : ViewModelBase
    {
        private readonly IMvxMessenger _messenger;
        private readonly IClaimService _claimservice;

        public ClaimServiceProviderProvideInformationViewModel(IMvxMessenger messenger, IClaimService claimservice)
        {
            _messenger = messenger;
            _claimservice = claimservice;
        }

        public ClaimSubmissionType ClaimSubmissionType
        {
            get
            {
                return _claimservice.SelectedClaimSubmissionType;
            }
        }

        public string ProviderType
        {
            get
            {
                return ClaimSubmissionType.ID;
            }
        }

        public ICommand EnterServiceProviderCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    this.ShowViewModel<ClaimServiceProviderEntryViewModel>();
                });
            }
        }
    }
}
