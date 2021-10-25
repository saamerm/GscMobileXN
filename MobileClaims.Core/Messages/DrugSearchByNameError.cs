using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class DrugSearchByNameError : MvxMessage
    {
        public string Message { get; set; }
        public DrugSearchByNameError(object sender) : base(sender)
        {

        }
    }
}
