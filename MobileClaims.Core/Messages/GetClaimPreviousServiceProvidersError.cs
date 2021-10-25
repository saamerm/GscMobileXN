using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimPreviousServiceProvidersError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimPreviousServiceProvidersError(object sender)
            : base(sender)
        {

        }
    }
}
