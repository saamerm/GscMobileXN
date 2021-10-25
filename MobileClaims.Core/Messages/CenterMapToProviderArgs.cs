using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class CenterMapToProviderArgs : MvxMessage
    {
        public int ProviderId { get; set; }

        public CenterMapToProviderArgs(object sender) 
            : base(sender)
        {
        }
    }
}