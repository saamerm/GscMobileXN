using System;
using UIKit;

namespace MobileClaims.iOS
{
	public class ClaimDetailsSubComponent : UIView
	{
		protected UILabel titleLabel;

		protected UIButton errorButton;

		protected string _titleString;

		public string ErrorString = "";

		public ClaimDetailsSubComponent ()
		{
		}

		float _componentHeight;
		public virtual float ComponentHeight
		{
			get {
				return _componentHeight;
			}
			set{
				_componentHeight = value;
			}

		}

		protected void HandleErrorButton (object sender, EventArgs e)
		{
			UIAlertView _error = new UIAlertView ("", ErrorString, null, "ok".tr(), null);

			_error.Show ();
		}
	}
}

