using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetLensTypesError : MvxMessage
    {
        public string Message { get; set; }
        public GetLensTypesError(object sender) : base(sender)
        {

        }
    }
}
