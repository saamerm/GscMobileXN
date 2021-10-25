using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetLocationError : MvxMessage
    {
        public string Message { get; set; }
        public GetLocationError(object sender) : base(sender)
        {

        }
    }
}
