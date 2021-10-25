using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetLensTypesComplete : MvxMessage
    {
        public string Message { get; set; }
        public GetLensTypesComplete(object sender) : base(sender)
        {

        }
    }
}
