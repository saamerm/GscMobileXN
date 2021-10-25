using Android.Content;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Runtime;

namespace MobileClaims.Droid
{
	[Register("gsc.HeaderTextView")]
	public class HeaderTextView : TextView
	{


		protected TextView headerText;


		public HeaderTextView (Context context) :
		base (context)
		{
			Initialize ();
		}

		public HeaderTextView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public HeaderTextView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{

			float textSize;

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				textSize = 27.0f;
			} else {
				textSize = 27.0f;
			}

			Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

			SetTypeface (leagueFont, TypefaceStyle.Normal);
			SetTextSize (ComplexUnitType.Sp, textSize);

			SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
			//this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
		}

	}
}

