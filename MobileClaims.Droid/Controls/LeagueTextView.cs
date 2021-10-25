using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.Util;
using Android.Graphics;

namespace MobileClaims.Droid
{		
    [Register("gsc.LeagueTextView")]
	public class LeagueTextView : TextView
	{


		protected TextView headerText;


		public LeagueTextView (Context context) :
		base (context)
		{
			Initialize ();
		}

		public LeagueTextView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public LeagueTextView (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{

			//Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

			//SetTypeface (leagueFont, TypefaceStyle.Normal);
			//SetTextSize (ComplexUnitType.Dip, textSize);

			Typeface leagueFont;
			try
			{
				leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
			}
			catch (Exception e)
			{
				//Log.Error(Tag, string.Format("Could not get Typeface: Error: {1}", e));
				return;
			}

			if (null == leagueFont) return;

			var tfStyle = TypefaceStyle.Normal;
			if (null != Typeface)
				tfStyle = Typeface.Style;
			SetTypeface(leagueFont, tfStyle);

		}


	}
}

