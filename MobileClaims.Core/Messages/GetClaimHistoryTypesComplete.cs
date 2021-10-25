using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimHistoryTypesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimHistoryTypesComplete(object sender) : base(sender)
        {

        }
    }
}
