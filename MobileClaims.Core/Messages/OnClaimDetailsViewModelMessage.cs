using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
	public class OnClaimDetailsViewModelMessage : MvxMessage
    {
        public string Message { get; set; }
		public OnClaimDetailsViewModelMessage(object sender) : base(sender)
        {

        }
    }
}
