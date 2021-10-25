using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetUserAgreementError : MvxMessage
    {
        public string Message { get; set; }
        public GetUserAgreementError(object sender)
            : base(sender)
        {

        }
    }
}
