using System;
using Foundation;
using SafariServices;

namespace MobileClaims.iOS
{
	public class SFViewController : SFSafariViewController, ISFSafariViewControllerDelegate
	{

		public SFSafariViewController SFwebviewController { get; set; }
		public SFViewControllerDelegate ViewControllerDelegate;
		protected Uri openUrl;
		public SFViewController(Uri u) : base(u)
		{
			openUrl = u;
			ViewControllerDelegate = new SFViewControllerDelegate();
			ViewControllerDelegate.SFViewController = this; ;
			this.Delegate = ViewControllerDelegate;
		}


	}


	public class SFViewControllerDelegate : SFSafariViewControllerDelegate
	{
		public SFSafariViewController SFViewController { get; set; }

	    public delegate void EventHandler(object sender, EventArgs args);

        public event EventHandler RefreshEvent;
		protected virtual void RaiseRefreshEvent(EventArgs e)
		{
			if (this.RefreshEvent != null)
			{
				RefreshEvent(this, e);
			}
		}

		public IntPtr Handle
		{
			get;
		}
		[Export("safariViewControllerDidFinish:")]
		public override void DidFinish(SFSafariViewController controller)
		{

			RaiseRefreshEvent(new EventArgs());


		}

		public void Dispose(bool isDispose)
		{


		}
	}
}