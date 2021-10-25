using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;

namespace MobileClaims.Droid
{
	public class AccountListInvisibleLayout : RelativeLayout
	{
		public AccountListInvisibleLayout (Context context) :
		base (context)
		{
			Initialize ();

		}

		public AccountListInvisibleLayout (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		private List<SpendingAccountType> _accounts;
		public List<SpendingAccountType> accounts
		{
			get{
				return _accounts;
			}
			set{
				_accounts = value;

				if (_accounts !=null && _accounts.Count > 0) {
					this.Visibility = ViewStates.Gone;
				} else {
					this.Visibility = ViewStates.Visible;
				}

			}

		}
	}
}

