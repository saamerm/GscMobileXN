using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityProvincesError : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityProvincesError(object sender) : base(sender)
        {

        }
    }
}
