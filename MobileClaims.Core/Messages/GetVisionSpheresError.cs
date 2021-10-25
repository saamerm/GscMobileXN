using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionSpheresError : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionSpheresError(object sender) : base(sender)
        {

        }
    }
}
