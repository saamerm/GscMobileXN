using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetPreviousServiceProvidersError : MvxMessage
    {
        public string Message { get; set; }
        public GetPreviousServiceProvidersError(object sender) : base(sender)
        {

        }
    }
}
