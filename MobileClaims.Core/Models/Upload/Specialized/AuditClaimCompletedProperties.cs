using System;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditClaimCompletedProperties : IClaimCompletedProperties
    {
        public string Title => Resource.SubmissionConfirmation;

        public string UploadSuccess => Resource.UploadSuccessful;

        public string UploadCompletedNote => Resource.UploadSuccessfulAdditionalMessage;

        public string BackToMyClaimsText => Resource.BackToDashboard;

        public string BackToViewModelType => nameof(DashboardViewModel);
    }
}