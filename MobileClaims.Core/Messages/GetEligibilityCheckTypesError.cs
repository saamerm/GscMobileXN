using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityCheckTypesError : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityCheckTypesError(object sender) : base(sender)
        {

        }
    }
}
