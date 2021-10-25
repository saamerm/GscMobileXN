using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoTextAlterationClaimAgreementFound : MvxMessage
    {
        public string Message { get; set; }
        public NoTextAlterationClaimAgreementFound(object sender) : base(sender)
        {

        }
    }
}
