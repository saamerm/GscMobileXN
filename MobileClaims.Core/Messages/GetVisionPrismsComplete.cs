using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionPrismsComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionPrismsComplete(object sender) : base(sender)
        {

        }
    }
}
