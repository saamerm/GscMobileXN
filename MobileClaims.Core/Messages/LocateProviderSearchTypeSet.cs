using MobileClaims.Core.Entities;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class LocateProviderSearchTypeSet : MvxMessage
    {
        public LocateProviderSearchTypeSet(object sender) : base(sender)
        {

        }
        public SearchType SearchType{ get; set; }
        public LocationType LocationType { get; set; }
        public ServiceProviderType ProviderType { get; set; }
    }
}

