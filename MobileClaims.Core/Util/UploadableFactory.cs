using MobileClaims.Core.Entities;
using MobileClaims.Core.Models.Upload;
using MobileClaims.Core.Models.Upload.Specialized;
using MobileClaims.Core.ViewModels;

namespace MobileClaims.Core.Util
{
    public interface IUploadPropertiesFactory
    {
        IClaimPropertiesBase Create(string uploadStateName);
    }

    public class AuditUploadPropertiesFactory : IUploadPropertiesFactory
    {
        public IClaimPropertiesBase Create(string uploadStateName)
        {
            if (uploadStateName.Equals(nameof(ClaimSummaryViewModel)))
            {
                return new AuditClaimSummaryProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentUploadViewModel)))
            {
                return new AuditClaimUploadProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentSubmitViewModel)))
            {
                return new AuditClaimSubmitProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentCompletedViewModel)))
            {
                return new AuditClaimCompletedProperties();
            }

            if (uploadStateName.Equals(nameof(DisclaimerViewModel)))
            {
                return new AuditDisclaimerProperties();
            }

            return new AuditClaimPropertiesBase();
        }
    }

    public class CopUploadPropertiesFactory : IUploadPropertiesFactory
    {
        public IClaimPropertiesBase Create(string uploadStateName)
        {
            if (uploadStateName.Equals(nameof(ClaimSummaryViewModel)) ||
                uploadStateName.Equals(nameof(ActiveClaimDetailViewModel)))
            {
                return new CopClaimSummaryProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentUploadViewModel)))
            {
                return new CopClaimUploadProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentSubmitViewModel)))
            {
                return new CopClaimSubmitProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentCompletedViewModel)))
            {
                return new CopClaimCompletedProperties();
            }

            if (uploadStateName.Equals(nameof(DisclaimerViewModel)))
            {
                return new CopDisclaimerProperties();
            }

            return new CopClaimPropertiesBase();
        }
    }

    public class NonUploadPropertiesFactory : IUploadPropertiesFactory
    {
        public IClaimPropertiesBase Create(string uploadStateName)
        {
            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentUploadViewModel)))
            {
                return new CopClaimUploadProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentSubmitViewModel)))
            {
                return new CopClaimSubmitProperties();
            }

            if (uploadStateName.Equals(nameof(ConfirmationOfPaymentCompletedViewModel)))
            {
                return new CopClaimCompletedProperties();
            }

            if (uploadStateName.Equals(nameof(DisclaimerViewModel)))
            {
                return new CopDisclaimerProperties();
            }

            return new NonActionClaimSummaryProperties();
        }
    }

    public static class UploadFactory
    {
        private static IUploadPropertiesFactory _uploadPropertyFactory;

        public static IClaimPropertiesBase Create(ClaimActionState claimActionState, string uploadStateName)
        {
            switch (claimActionState)
            {
                case ClaimActionState.Audit:
                    _uploadPropertyFactory = new AuditUploadPropertiesFactory();
                    break;
                case ClaimActionState.Cop:
                    _uploadPropertyFactory = new CopUploadPropertiesFactory();
                    break;
                case ClaimActionState.None:
                    _uploadPropertyFactory = new NonUploadPropertiesFactory();
                    break;
            }

            return _uploadPropertyFactory.Create(uploadStateName);
        }
    }
   
}