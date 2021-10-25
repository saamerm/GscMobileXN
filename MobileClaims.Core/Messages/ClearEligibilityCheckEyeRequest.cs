using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
  public  class ClearEligibilityCheckEyeRequest : MvxMessage
    {
      public ClearEligibilityCheckEyeRequest(object sender)
            : base(sender)
        { }
      public bool isClose { get; set; }

    }
}
