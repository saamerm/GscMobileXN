using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    // TODO: Not used.
    public class AuthenticationErrorMessage : MvxMessage
    {
        public AuthenticationErrorMessage(object sender, string ErrorMessage)
            : base(sender)
        {
        }
    }
}
