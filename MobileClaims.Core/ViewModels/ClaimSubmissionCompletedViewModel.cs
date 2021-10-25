using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MobileClaims.Core.ViewModels
{
    public class ClaimSubmissionCompletedViewModel : ViewModelBase
    {
        public string SubmissionConfirmationTitle => Resource.SubmissionConfirmation;
        public string UploadSuccess => string.Empty;
        public string UploadCompletedNote => string.Empty;
        public string BackToMyClaimsText => Resource.BackToMyClaims;

        public IMvxCommand BackToMyClaimsCommand { get; }

        private bool _isNoteVisible;

        public bool IsNoteVisible
        {
            get => _isNoteVisible;
            private set => SetProperty(ref _isNoteVisible, value);
        }

        public ClaimSubmissionCompletedViewModel()
        {
            BackToMyClaimsCommand = new MvxCommand(BackToMyClaims);
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);

            /*IsNoteVisible =
                SerializationUtils.TryDeserializeMessageStatus<bool>(parameters.Data[BundleKeys.UploadableKey]);*/
        }

        private void BackToMyClaims()
        {
            ShowViewModel<ChooseClaimOrHistoryViewModel>();
        }
    }
}