using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetTextAlterationClaimAgreementComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetTextAlterationClaimAgreementComplete(object sender) : base(sender)
        {

        }
    }
}
