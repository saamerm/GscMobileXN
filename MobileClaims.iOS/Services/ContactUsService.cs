using Foundation;
using MobileClaims.Core.Services;
using UIKit;

namespace MobileClaims.iOS.Services
{
    public class ContactUsService : IContactUsService
    {
        public void DisplayContactUsPage()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("https://sel.ccq.org/GestionSel/zjweb020/aide.aspx"));
        }
    }
}