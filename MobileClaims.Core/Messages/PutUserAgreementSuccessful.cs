using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class PutUserAgreementSuccessful : MvxMessage
    {
        public string Message { get; set; }
        public PutUserAgreementSuccessful(object sender) : base(sender)
        {
        }
    }
}
