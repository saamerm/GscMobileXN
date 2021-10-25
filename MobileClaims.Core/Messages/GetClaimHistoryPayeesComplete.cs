using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimHistoryPayeesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimHistoryPayeesComplete(object sender) : base(sender)
        {

        }
    }
}
