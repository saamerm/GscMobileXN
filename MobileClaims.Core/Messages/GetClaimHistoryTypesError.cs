using MvvmCross.Plugin.Messenger;

namespace MobileClaims.Core.Messages
{
    public class GetClaimHistoryTypesError : MvxMessage
    {
        public string Message { get; set; }
        public GetClaimHistoryTypesError(object sender) : base(sender)
        {

        }
    }


	public class ClaimHistoryResultsListViewOnLoad : MvxMessage
	{
		public string Message { get; set; }
		public ClaimHistoryResultsListViewOnLoad(object sender) : base(sender)
		{

		}
	}



	public class ClaimHistoryResultDetailViewOnLoad : MvxMessage
	{
		public string Message { get; set; }
		public ClaimHistoryResultDetailViewOnLoad(object sender) : base(sender)
		{

		}
	}
}
