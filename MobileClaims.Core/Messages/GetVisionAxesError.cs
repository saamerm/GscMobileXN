using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionAxesError : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionAxesError(object sender) : base(sender)
        {

        }
    }
}
