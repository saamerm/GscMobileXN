using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimSubmissionTypesError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimSubmissionTypesError(object sender) : base(sender)
        {

        }
    }
}
