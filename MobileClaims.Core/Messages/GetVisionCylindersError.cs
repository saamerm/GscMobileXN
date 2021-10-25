using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionCylindersError : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionCylindersError(object sender) : base(sender)
        {

        }
    }
}
