using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetEligibilityCheckTypesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetEligibilityCheckTypesComplete(object sender) : base(sender)
        {

        }
    }
}
