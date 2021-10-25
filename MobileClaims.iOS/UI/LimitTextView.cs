using System;
using UIKit;
using Foundation;

namespace MobileClaims.iOS
{
	public class LimitTextView : UITextView
	{

		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler LimitReached;
		public int MaxCharacters { get; set; }

		public LimitTextView ()
		{
			this.BackgroundColor = Colors.LightGrayColor;
			this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
			this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
			this.TextColor = Colors.DARK_GREY_COLOR;

			MaxCharacters = 1000;
			ShouldChangeText = ShouldLimit;

		}

		static bool ShouldLimit (UITextView view, NSRange range, string text)
		{

			if (text == "\n") {
				view.ResignFirstResponder ();
				return false;
			}

			var textView = (LimitTextView)view;
 			var limit = textView.MaxCharacters;

			int newLength = ((int)view.Text.Length - (int)range.Length) + (int)text.Length;
			if (newLength <= limit)
				return true;

			var emptySpace = Math.Max (0, limit - (view.Text.Length - range.Length));
			var beforeCaret = view.Text.Substring (0, (int)range.Location) + text.Substring (0, (int)emptySpace);
			var afterCaret = view.Text.Substring ((int)range.Location + (int)range.Length);

			view.Text = beforeCaret + afterCaret;
			view.SelectedRange = new NSRange (beforeCaret.Length, 0);

			if (((LimitTextView)view).LimitReached != null)
			{
				((LimitTextView)view).LimitReached(view, EventArgs.Empty);
			}

			return false;
		}

	}
}

