using System.Collections.ObjectModel;
using Acr.UserDialogs;
using MobileClaims.Core.Constants;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Services;
using MobileClaims.Core.Util;
using MobileClaims.Core.ViewModelParameters;
using MobileClaims.Core.ViewModels.Interfaces;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using Newtonsoft.Json;

namespace MobileClaims.Core.ViewModels
{
    public class ConfirmationOfPaymentSubmitViewModel : ViewModelBase<ConfirmationOfPaymentSubmitViewModelParameters>, IFileNamesContainer, IClaimSubmitProperties
    {
        private readonly IUserDialogs _userDialogService;
        private readonly IClaimService _claimService;
        private UploadDocumentsFormData _uploadDocumentsFormData;
        private bool _isCommentVisible;

        public TopCardViewData TopCardViewData { get; set; }
        public ObservableCollection<DocumentInfo> Attachments { get; set; } = new ObservableCollection<DocumentInfo>();
        
        public bool IsDisclaimerChecked { get; set; }
        public string Title { get; private set; }
        public string Comments { get; set; }
        public string DocumentsToUpload => Resource.DocumentsToUpload;
        public string AdditionalInformation => $"{Resource.AdditionalInformation}:";
        public string HaveReadAndAcceptThe => Resource.HaveReadAndAcceptThe;
        public string Submit => Resource.Submit;
        public string UploadDocumentType { get; private set; }

        public bool IsCommentVisible
        {
            get => _isCommentVisible;
            set => SetProperty(ref _isCommentVisible, value);
        }

        public IMvxCommand SubmitDocumentsCommand { get; }
        public IMvxCommand OpenDisclaimerCommand { get; }

        public ConfirmationOfPaymentSubmitViewModel(IUserDialogs userDialogService, IClaimService claimService)
        {
            _userDialogService = userDialogService;
            _claimService = claimService;

            OpenDisclaimerCommand = new MvxCommand(OpenDisclaimer);
            SubmitDocumentsCommand = new MvxCommand(SubmitDocuments);
        }

        private void OpenDisclaimer()
        {
            IDisclaimerProperties uploadable = (IDisclaimerProperties)UploadFactory.Create(TopCardViewData.ClaimActionState, nameof(DisclaimerViewModel));

            ShowViewModel<DisclaimerViewModel, DisclaimerViewModelParameters>(new DisclaimerViewModelParameters(uploadable));
        }

        private async void SubmitDocuments()
        {
            if (!IsDisclaimerChecked)
            {
                await _userDialogService.AlertAsync(Resource.AcceptDisclaimerFirst);
                return;
            }

            try
            {
                _userDialogService.ShowLoading(Resource.Loading);
                var isSuccessful = await _claimService.UploadDocumentsAsync(Comments,
                        _uploadDocumentsFormData.ClaimFormId.ToString(),
                        UploadDocumentType,
                        Attachments,
                        _uploadDocumentsFormData.ParticipantNumber);

                if (isSuccessful)
                {
                    IClaimCompletedProperties uploadable = (IClaimCompletedProperties)UploadFactory.Create(TopCardViewData.ClaimActionState, nameof(ConfirmationOfPaymentCompletedViewModel));
                    await ShowViewModel<ConfirmationOfPaymentCompletedViewModel, ConfirmationOfPaymentCompletedViewModelParameters>(new ConfirmationOfPaymentCompletedViewModelParameters(uploadable));
                }
                else
                {
                    _userDialogService.HideLoading();
                    await _userDialogService.AlertAsync(Resource.GenericErrorDialogMessage);
                }
            }
            catch (NullResponseException)
            {
                _userDialogService.HideLoading();
                await _userDialogService.AlertAsync(Resource.ServerError_Description, Resource.ServerError_Title);
            }
            finally
            {
                _userDialogService.HideLoading();
            }
        }

        public override void Prepare(ConfirmationOfPaymentSubmitViewModelParameters parameter)
        {
            _uploadDocumentsFormData = parameter.PlanMemberData;
            TopCardViewData = parameter.TopCardViewData;

            Title = parameter.ClaimSubmitProperties.Title;
            IsCommentVisible = parameter.ClaimSubmitProperties.IsCommentVisible;
            UploadDocumentType = parameter.ClaimSubmitProperties.UploadDocumentType;

            Attachments = parameter.Attachments;
        }
    }
}