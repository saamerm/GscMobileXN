using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetSpendingAccountTypesError : MvxMessage
    {
        public string Message { get; set; }
        public GetSpendingAccountTypesError(object sender) : base(sender)
        {

        }
    }
}
