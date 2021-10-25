using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class FavouriteHealthProviderChange : MvxMessage
    {
        public int ProviderId { get; set; }

        public bool IsFavourite { get; set; }

        public FavouriteHealthProviderChange(object sender) : base(sender)
        {
        }
    }
}
