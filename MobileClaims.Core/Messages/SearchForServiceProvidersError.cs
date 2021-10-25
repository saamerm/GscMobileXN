using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchForServiceProvidersError : MvxMessage
    {
        public string Message { get; set; }
        public SearchForServiceProvidersError(object sender) : base(sender)
        {

        }
    }
}
