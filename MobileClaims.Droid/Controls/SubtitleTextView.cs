using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Graphics;

namespace MobileClaims.Droid
{		
	public class SubtitleTextView : TextView
	{


		protected TextView headerText;


		public SubtitleTextView (Context context) :
		base (context)
		{
			Initialize ();
		}

		public SubtitleTextView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public SubtitleTextView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{

			float textSize;

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				textSize = 18.0f;
			} else {
				textSize = 18.0f;
			}

			Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");

			SetTypeface (leagueFont, TypefaceStyle.Normal);
			SetTextSize (ComplexUnitType.Sp, textSize);

			SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
			//this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
		}


	}
}

