using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimOptionsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimOptionsComplete(object sender) : base(sender)
        {

        }
    }
}
