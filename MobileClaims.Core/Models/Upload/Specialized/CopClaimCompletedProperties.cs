using System;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class CopClaimCompletedProperties : IClaimCompletedProperties
    {
        public string Title => Resource.SubmissionConfirmation;

        public string UploadSuccess => Resource.UploadSuccessful;

        public string UploadCompletedNote => string.Empty;

        public string BackToMyClaimsText => Resource.BackToMyClaims;

        public string BackToViewModelType => nameof(ChooseClaimOrHistoryViewModel);
    }
}