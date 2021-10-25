using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionCylindersComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionCylindersComplete(object sender) : base(sender)
        {

        }
    }
}
