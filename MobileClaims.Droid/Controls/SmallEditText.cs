using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Graphics;

namespace MobileClaims.Droid
{		
	public class SmallEditText : EditText
	{


		public SmallEditText (Context context) :
		base (context)
		{
			Initialize ();
		}

		public SmallEditText (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public SmallEditText (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{


			Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");

			SetTypeface (leagueFont, TypefaceStyle.Normal);

		}

	}
}

