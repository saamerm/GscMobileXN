using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimSubmissionTypesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimSubmissionTypesComplete(object sender) : base(sender)
        {

        }
    }
}
