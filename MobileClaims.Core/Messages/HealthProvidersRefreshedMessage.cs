using MobileClaims.Core.ViewModelParameters;
using MvvmCross.Plugin.Messenger;
using System.Collections.Generic;

namespace MobileClaims.Core.Messages
{
    public class HealthProvidersRefreshedMessage : MvxMessage
    {
        public HealthProvidersRefreshedMessage(object sender, IList<HealthProviderSummaryModel> serviceProviders) 
            : base(sender)
        {
            ServiceProviders = serviceProviders;
        }

        public IList<HealthProviderSummaryModel> ServiceProviders { get;  }
    }
}
