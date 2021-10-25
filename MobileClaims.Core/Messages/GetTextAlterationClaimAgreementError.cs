using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTextAlterationClaimAgreementError : MvxMessage
    {
        public string Message { get; set; }
        public GetTextAlterationClaimAgreementError(object sender) : base(sender)
        {

        }
    }
}
