using Android.Content;
using Android.Util;
using Android.Widget;

namespace MobileClaims.Droid
{
	public class DollarSignTextView : TextView
	{
		public DollarSignTextView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public DollarSignTextView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public DollarSignTextView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		private string _dollarString;
		public string DollarString
		{
			get{
				return _dollarString;
			}
			set{
				this.SetText ("$" + value, BufferType.Normal);
			}

		}
	}
}

