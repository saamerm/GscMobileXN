using System;
using System.Collections.ObjectModel;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;

namespace MobileClaims.Core.ViewModelParameters
{
    public class ClaimSubmitTermsAndConditionsViewModelParameters
    {
        public NonRealTimeClaimType NonRealTimeClaimType { get; set; }
		public IDisclaimerProperties Uploadable { get; set; }
        public ObservableCollection<DocumentInfo> Attachments { get; set; }
        public string Comment { get; set; }

        public ClaimSubmitTermsAndConditionsViewModelParameters(NonRealTimeClaimType nonRealTimeClaimType, IDisclaimerProperties uploadable, ObservableCollection<DocumentInfo> attachments, string comment)
        {
            NonRealTimeClaimType = nonRealTimeClaimType;
            Uploadable = uploadable;
            Attachments = attachments;
            Comment = comment;
        }
    }
}
