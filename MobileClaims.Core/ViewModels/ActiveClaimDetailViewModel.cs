using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ActiveClaimDetailViewModel : ViewModelBase<ActiveClaimDetailViewModelParameters>, IClaimSummaryProperties
    {
        private UploadDocumentsFormData _uploadDocumentsFormData;
       
        public string TwentyFourMb => Resource.TwentyFourMb;
        public string Title { get; private set; }
        public bool IsUploadSectionVisible { get; private set; }
        public string UploadButtonText { get; private set; }

        public string DocumentsToUpload => Resource.DocumentsToUpload;
        public string ExplanationOfBenefitsLabel => Resource.ExplanationOfBenefits;
        public string CombinedSizeOfFilesMustBe => Resource.CombinedSizeOfFilesMustBe;
        public string SubmitAdditionalInformationLabel => Resource.ClaimHistoryDetailRequriesCopTrue;

        public TopCardViewData TopCardViewData { get; private set; }

        public IMvxCommand OpenUploadDocumentsCommand { get; }

        public ActiveClaimDetailViewModel()
        {
            OpenUploadDocumentsCommand = new MvxCommand(OpenUploadDocuments);
        }

        private void OpenUploadDocuments()
        {
            IClaimUploadProperties uploadable = (IClaimUploadProperties)UploadFactory.Create(TopCardViewData.ClaimActionState, nameof(ConfirmationOfPaymentUploadViewModel));

            ShowViewModel<ConfirmationOfPaymentUploadViewModel, IViewModelParameters>(new ConfirmationOfPaymentUploadViewModelParameters(TopCardViewData, _uploadDocumentsFormData, uploadable));
        }

        public override void Prepare(ActiveClaimDetailViewModelParameters parameter)
        {
            TopCardViewData = parameter.TopCardWithSummary;
            _uploadDocumentsFormData = parameter.CopPlanData;

            Title = parameter.ClaimSummary.Title;
            UploadButtonText = parameter.ClaimSummary.UploadButtonText;
            IsUploadSectionVisible = parameter.ClaimSummary.IsUploadSectionVisible;
        }
    }
}