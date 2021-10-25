using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class AuditInformationViewModel : ViewModelBase
    {
        private ClaimResultGSC _claimResult;
        public ClaimResultGSC ClaimResult
        {
            get => _claimResult;
            set => SetProperty(ref _claimResult, value);
        }

        public IMvxCommand DoneCommand { get; }

        public AuditInformationViewModel()
        {
            DoneCommand = new MvxCommand(ExecuteDone);
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
            var serializedClaimResult = parameters.Data[BundleKeys.AuditClaimDetailKey];

            ClaimResult = JsonConvert.DeserializeObject<ClaimResultGSC>(serializedClaimResult);
        }

        private void ExecuteDone()
        {
            Close(this);
        }
    }
}