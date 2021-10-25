using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class DrugSearchComplete : MvxMessage
    {
        public DrugSearchComplete(object sender)
            : base(sender)
        { }
    }
}
