using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class PutUserAgreementError : MvxMessage
    {
        public string Message { get; set; }
        public PutUserAgreementError(object sender)
            : base(sender)
        {

        }
    }
}