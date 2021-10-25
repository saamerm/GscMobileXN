using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class CheckDrugEligibilityError : MvxMessage
    {
        public string Message { get; set; }
        public CheckDrugEligibilityError(object sender) : base(sender)
        {

        }
    }
}
