using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimHistoryPayeesError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimHistoryPayeesError(object sender) : base(sender)
        {

        }
    }
}
