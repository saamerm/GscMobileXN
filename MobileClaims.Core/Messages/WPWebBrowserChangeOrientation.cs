using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{ 
    public class WPWebBrowserChangeOrientation : MvxMessage
    {
        public WPWebBrowserChangeOrientation(object sender)
            : base(sender)
        {
        }
       
        public bool isLandscape { get; set; }
    }
}
