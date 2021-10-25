using MobileClaims.Core.ViewModels;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ProviderTypeSelectedMessage : MvxMessage
    {
        public HealthProviderTypeViewModel SelectedProviderType { get; }

        public ProviderTypeSelectedMessage(object sender, HealthProviderTypeViewModel selectedProviderType)
            : base(sender)
        {
            SelectedProviderType = selectedProviderType;
        }
    }
}