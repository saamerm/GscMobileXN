using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized.PerType;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Util
{
    public static class NonRealTimeUploadFactory
    {
        private static IUploadPropertiesFactory _uploadPropertyFactory;

        public static IClaimPropertiesBase Create(NonRealTimeClaimType nonRealTimeClaimType, string targetViewModelName)
        {
            switch (nonRealTimeClaimType)
            {
                case NonRealTimeClaimType.Drug:
                    _uploadPropertyFactory = new DrugUploadPropertiesFactory();
                    break;
                case NonRealTimeClaimType.NotDefined:
                    _uploadPropertyFactory = new RealTimeClaimNonUploadPropertiesFactory();
                    break;
            }

            return _uploadPropertyFactory.Create(targetViewModelName);
        }
    }

    public class DrugUploadPropertiesFactory : IUploadPropertiesFactory
    {
        public IClaimPropertiesBase Create(string targetViewModelName)
        {
            if (targetViewModelName.Equals(nameof(ClaimDocumentsUploadViewModel)))
            {
                return new DrugClaimUploadProperties();
            }

            if (targetViewModelName.Equals(nameof(ClaimSubmitTermsAndConditionsViewModel)))
            {
                return new DrugClaimDisclaimerProperties();
            }

            if (targetViewModelName.Equals(nameof(ClaimSubmissionConfirmationViewModel)))
            {
                return new DrugClaimSubmissionProperties();
            }

            if (targetViewModelName.Equals(nameof(ConfirmationOfPaymentCompletedViewModel)))
            {
                return new DrugClaimSubmissionCompletedProperties();
            }

            return new DrugClaimPropertiesBase();
        }
    }

    public class RealTimeClaimNonUploadPropertiesFactory : IUploadPropertiesFactory
    {
        public IClaimPropertiesBase Create(string targetViewModelName)
        {
            if (targetViewModelName.Equals(nameof(ClaimDocumentsUploadViewModel)))
            {
                return new RealTimeClaimNonUploadProperties();
            }

            if (targetViewModelName.Equals(nameof(ClaimSubmitTermsAndConditionsViewModel)))
            {
                return new DrugClaimDisclaimerProperties();
            }

            if (targetViewModelName.Equals(nameof(ClaimSubmissionConfirmationViewModel)))
            {
                return new RealTimeClaimSubmissionProperties();
            }

            if (targetViewModelName.Equals(nameof(ConfirmationOfPaymentCompletedViewModel)))
            {
                return new RealTimeClaimSubmissionCompletedProperties();
            }

            return new DrugClaimPropertiesBase();
        }
    }
}