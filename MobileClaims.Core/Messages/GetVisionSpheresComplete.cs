using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionSpheresComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionSpheresComplete(object sender) : base(sender)
        {

        }
    }
}
