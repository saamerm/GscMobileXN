using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
  public   class BusyIndicatorForClaimSubmitResultFeedBack: MvxMessage
    {
      public BusyIndicatorForClaimSubmitResultFeedBack(object sender)
            : base(sender)
        {
        }
        public bool Busy { get; set; }
    }
}
