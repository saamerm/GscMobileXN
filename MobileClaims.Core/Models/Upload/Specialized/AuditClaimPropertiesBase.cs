namespace MobileClaims.Core.Models.Upload.Specialized
{
    public class AuditClaimPropertiesBase : IClaimPropertiesBase
    {
        public string Title => "#Audit".ToUpperInvariant();
    }
}