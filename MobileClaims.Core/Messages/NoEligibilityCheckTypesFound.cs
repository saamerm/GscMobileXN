using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NoEligibilityCheckTypesFound : MvxMessage
    {
        public string Message { get; set; }
        public NoEligibilityCheckTypesFound(object sender) : base(sender)
        {

        }
    }
}
