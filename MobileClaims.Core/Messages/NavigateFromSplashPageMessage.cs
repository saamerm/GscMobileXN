using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class NavigateFromSplashPageMessage : MvxMessage
    {
        public string Message { get; set; }
        public NavigateFromSplashPageMessage(object sender) : base(sender)
        {

        }
    }
}
