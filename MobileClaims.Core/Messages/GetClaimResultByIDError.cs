using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimResultByIDError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimResultByIDError(object sender) : base(sender)
        {

        }
    }
}
