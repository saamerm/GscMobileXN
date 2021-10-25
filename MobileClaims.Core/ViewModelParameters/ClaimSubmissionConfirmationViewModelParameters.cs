using System;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ClaimSubmissionConfirmationViewModelParameters
    {
        public NonRealTimeClaimType ClaimType { get; private set; }
        public IClaimSubmitProperties Uploadable { get; set; }
        public ObservableCollection<DocumentInfo> Attachments { get; set; }
        public string Comment { get; set; }

        public ClaimSubmissionConfirmationViewModelParameters(NonRealTimeClaimType claimType, IClaimSubmitProperties uploadable, ObservableCollection<DocumentInfo> attachments, string comment)
        {
            ClaimType = claimType;
            Uploadable = uploadable;
            Attachments = attachments;
            Comment = comment;
        }
    }
}
