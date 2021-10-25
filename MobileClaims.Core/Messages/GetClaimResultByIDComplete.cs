using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimResultByIDComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimResultByIDComplete(object sender) : base(sender)
        {

        }
    }
}
