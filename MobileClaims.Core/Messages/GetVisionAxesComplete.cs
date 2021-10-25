using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionAxesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionAxesComplete(object sender) : base(sender)
        {

        }
    }
}
