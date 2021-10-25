using MobileClaims.Core.Constants;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels
{
    public class ConfirmationOfPaymentCompletedViewModel : ViewModelBase<ConfirmationOfPaymentCompletedViewModelParameters>, IClaimCompletedProperties
    {

        private readonly IClaimService _claimService;
        private bool _isNoteVisible;

        public string Title { get; private set; }
        public string UploadSuccess { get; private set; }
        public string UploadCompletedNote { get; private set; }
        public string BackToMyClaimsText { get; private set; }
        public string BackToViewModelType { get; private set; }

        public bool IsNoteVisible
        {
            get => _isNoteVisible;
            private set => SetProperty(ref _isNoteVisible, value);
        }

        public IMvxCommand BackToMyClaimsCommand { get; }

        public ConfirmationOfPaymentCompletedViewModel(IClaimService claimService)
        {
            _claimService = claimService;
            BackToMyClaimsCommand = new MvxCommand(BackToMyClaims);
        }

        private void BackToMyClaims()
        {
            GoHomeCommand.Execute(this);
            if (BackToViewModelType.Equals(nameof(ChooseClaimOrHistoryViewModel)))
            {
                ShowViewModel<ChooseClaimOrHistoryViewModel>();
            }
            else if (BackToViewModelType.Equals(nameof(DashboardViewModel)))
            {
                ShowViewModel<DashboardViewModel>();
            }
        }

        public override void Prepare(ConfirmationOfPaymentCompletedViewModelParameters parameter)
        {
            var claimCompletedPropeties = parameter.ClaimCompletedProperties;
            if (claimCompletedPropeties.GetType() != typeof(AuditClaimCompletedProperties) && claimCompletedPropeties.GetType() != typeof(CopClaimCompletedProperties))
            {
                _claimService?.ClearClaimDetails();
            }
            Title = parameter.ClaimCompletedProperties.Title;
            UploadSuccess = parameter.ClaimCompletedProperties.UploadSuccess;
            UploadCompletedNote = parameter.ClaimCompletedProperties.UploadCompletedNote;
            BackToMyClaimsText = parameter.ClaimCompletedProperties.BackToMyClaimsText;
            BackToViewModelType = parameter.ClaimCompletedProperties.BackToViewModelType;
            IsNoteVisible = parameter.ClaimCompletedProperties.UploadCompletedNote != string.Empty;
        }
    }
}