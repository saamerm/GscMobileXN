using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class UpdateHostNavigation : MvxMessage
    {
        public UpdateHostNavigation(object sender)
            : base(sender)
        {
        }
        public int NavigationIndex { get; set; }
        public bool GoHome { get; set; }
    }
}
