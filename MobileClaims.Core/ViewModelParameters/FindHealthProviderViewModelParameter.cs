using System;
using MobileClaims.Core.Services.Requests;

namespace MobileClaims.Core.ViewModelParameters
{
    public class FindHealthProviderViewModelParameter
    {
        public readonly ProvidersId ProviderSearchKey;
        public FindHealthProviderViewModelParameter(ProvidersId providerSearchKey)
        {
            ProviderSearchKey = providerSearchKey;
        }
    }
}
