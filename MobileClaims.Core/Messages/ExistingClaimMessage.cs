using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class ExistingClaimMessage : MvxMessage
    {
        public string Message { get; set; }
        public ExistingClaimMessage(object sender) : base(sender)
        {

        }
    }
}
