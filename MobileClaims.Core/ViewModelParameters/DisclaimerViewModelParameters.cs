using MobileClaims.Core.Models.Upload;

namespace MobileClaims.Core.ViewModelParameters
{
    public class DisclaimerViewModelParameters
    {
        public IDisclaimerProperties DisclaimerProperties { get; set; }

        public DisclaimerViewModelParameters(IDisclaimerProperties disclaimerProperties)
        {
            DisclaimerProperties = disclaimerProperties;
		}
    }
}
