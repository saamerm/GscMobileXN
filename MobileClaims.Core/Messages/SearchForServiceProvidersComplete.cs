using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class SearchForServiceProvidersComplete : MvxMessage
    {
        public SearchForServiceProvidersComplete(object sender)
            : base(sender)
        { }
        public bool NoResultsFound { get; set; }
    }
}
