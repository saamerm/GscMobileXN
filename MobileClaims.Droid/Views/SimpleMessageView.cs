using Android.OS;
using Android.Widget;

namespace MobileClaims.Droid
{
	public class SimpleMessageView : Android.Support.V4.App.Fragment
	{
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			return inflater.Inflate (Resource.Layout.SimpleMessageView, null);

		}

		public void setMessageText(string messageText)
		{
			TextView messageTextView = this.Activity.FindViewById<TextView> (Resource.Id.simple_message_text);
			messageTextView.Text = messageText;
		}
	}
}

