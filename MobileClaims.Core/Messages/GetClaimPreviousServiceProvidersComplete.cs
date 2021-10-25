using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimPreviousServiceProvidersComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimPreviousServiceProvidersComplete(object sender)
            : base(sender)
        {

        }
    }
}
