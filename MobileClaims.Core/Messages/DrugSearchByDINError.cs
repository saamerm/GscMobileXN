using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class DrugSearchByDINError : MvxMessage
    {
        public string Message { get; set; }
        public DrugSearchByDINError(object sender) : base(sender)
        {

        }
    }
}
