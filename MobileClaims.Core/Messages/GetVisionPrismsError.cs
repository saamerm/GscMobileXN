using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetVisionPrismsError : MvxMessage
    {
        public string Message { get; set; }
        public GetVisionPrismsError(object sender) : base(sender)
        {

        }
    }
}
