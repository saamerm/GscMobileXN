using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Graphics;

namespace MobileClaims.Droid
{		
	public class TitleTextView : TextView
	{


		protected TextView headerText;


		public TitleTextView (Context context) :
		base (context)
		{
			Initialize ();
		}

		public TitleTextView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public TitleTextView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{

			float textSize;

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				textSize = 29.0f;
			} else {
				textSize = 29.0f;
			}

			Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

			SetTypeface (leagueFont, TypefaceStyle.Normal);
			SetTextSize (ComplexUnitType.Sp, textSize);

			SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
			//this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
		}


	}
}

