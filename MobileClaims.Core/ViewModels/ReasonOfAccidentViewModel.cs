using MobileClaims.Core.Services;

namespace MobileClaims.Core.ViewModels
{
    public class ReasonOfAccidentViewModel : ViewModelBase
    {
        private readonly IClaimService _claimService;
        private bool _isClaimDueToAccident;

        public bool IsClaimDueToAccident
        {
            get => _isClaimDueToAccident;
            set
            {
                SetProperty(ref _isClaimDueToAccident, value);
                _claimService.Claim.IsClaimDueToAccident = _isClaimDueToAccident;
            }
        }

        public ReasonOfAccidentViewModel(IClaimService claimService)
        {
            _claimService = claimService;
            IsClaimDueToAccident = _claimService.Claim.IsClaimDueToAccident;
        }
    }
}