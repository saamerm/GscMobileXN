using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class LanguageUpdatedMessage : MvxMessage
    {
        public string Message { get; set; }
        public LanguageUpdatedMessage(object sender) : base(sender)
        {

        }
    }
}
