using System.ComponentModel;
using System.Threading.Tasks;
using MobileClaims.Core.Services;

namespace MobileClaims.Core.ViewModels
{
    public class DentalClaimDetailViewModel : ViewModelBase
    {
        private readonly IClaimService _claimService;

        private bool _isOtherTypeOfAccident;
        private bool _isOtherTypeOfAccidentVisible;

        public bool IsOtherTypeOfAccident
        {
            get => _isOtherTypeOfAccident;
            set
            {
                SetProperty(ref _isOtherTypeOfAccident, value);
                _claimService.Claim.IsOtherTypeOfAccident = _isOtherTypeOfAccident;
            }
        }

        public bool IsOtherTypeOfAccidentVisible
        {
            get => _isOtherTypeOfAccidentVisible;
            set
            {
                SetProperty(ref _isOtherTypeOfAccidentVisible, value);
            }
        }

        public DentalClaimDetailViewModel(IClaimService claimService)
        {
            _claimService = claimService;

            IsOtherTypeOfAccidentVisible = _claimService.Claim.IsOtherTypeOfAccidentVisible;
            IsOtherTypeOfAccident = _claimService.Claim.IsOtherTypeOfAccident;
        }

        public override Task RaisePropertyChanged(PropertyChangedEventArgs changedArgs)
        {
            _claimService.PersistClaim();
            return base.RaisePropertyChanged(changedArgs);
        }

        public override Task RaiseAllPropertiesChanged()
        {
            _claimService.PersistClaim();
            return base.RaiseAllPropertiesChanged();
        }

        public void ClearData()
        {
            IsOtherTypeOfAccident = false;
            IsOtherTypeOfAccidentVisible = false;
        }
    }
}