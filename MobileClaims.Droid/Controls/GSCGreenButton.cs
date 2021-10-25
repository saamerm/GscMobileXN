using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;

namespace MobileClaims.Droid
{		
	public class GSCGreenButton : Button
	{


		protected TextView headerText;


		public GSCGreenButton (Context context) :
		base (context)
		{
			Initialize ();
		}

		public GSCGreenButton (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public GSCGreenButton (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{

			float textSize;

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				textSize = 24.0f;
			} else {
				textSize = 24.0f;
			}

			Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

			SetTypeface (leagueFont, TypefaceStyle.Normal);
			SetTextSize (ComplexUnitType.Dip, textSize);

			//SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
			//this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
		}

	}
}

