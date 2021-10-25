using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetPreviousServiceProvidersComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetPreviousServiceProvidersComplete(object sender) : base(sender)
        {

        }
    }
}
