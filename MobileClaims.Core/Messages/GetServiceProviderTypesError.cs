using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetServiceProviderTypesError : MvxMessage
    {
        public string Message { get; set; }
        public GetServiceProviderTypesError(object sender) : base(sender)
        {

        }
    }
}
