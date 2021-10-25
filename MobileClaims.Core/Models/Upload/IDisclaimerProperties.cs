using System;

namespace MobileClaims.Core.Models.Upload
{
    public interface IDisclaimerProperties : IClaimPropertiesBase
    {
        string FirstParagraph { get; }
        string SecondParagraph { get; }
        string ThirdParagraph { get; }
    }
}