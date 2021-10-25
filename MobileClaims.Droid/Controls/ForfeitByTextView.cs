using Android.Content;
using Android.Util;
using Android.Widget;

namespace MobileClaims.Droid
{
	public class ForfeitByTextView : TextView
	{
		public ForfeitByTextView (Context context) :
		base (context)
		{
			Initialize ();
		}

		public ForfeitByTextView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public ForfeitByTextView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		private string _forfeitString;
		public string ForfeitString
		{
			get{
				return _forfeitString;
			}
			set{
				this.SetText (Resources.GetString(Resource.String.forfeited) + " " + value, BufferType.Normal);
			}

		}
	}
}


