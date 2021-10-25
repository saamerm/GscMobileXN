using Android.Content;
using Android.Util;
using Android.Widget;

namespace MobileClaims.Droid
{
	public class ContributionYearTextView : TextView
	{
		public ContributionYearTextView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public ContributionYearTextView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public ContributionYearTextView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
	}
}

