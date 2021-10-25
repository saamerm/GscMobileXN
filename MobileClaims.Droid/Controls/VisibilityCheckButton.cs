using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using MobileClaims.Core.Entities;

namespace MobileClaims.Droid
{
	public class VisibilityCheckButton : Button
	{
		public VisibilityCheckButton (Context context) :
			base (context)
		{
			Initialize ();
		}

		public VisibilityCheckButton (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public VisibilityCheckButton (Context context, IAttributeSet attrs, int defStyle) :
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

		}

		private DrugInfo _dinfo;
		public DrugInfo dInfo
		{
			get{
				return _dinfo;
			}
			set{
				_dinfo = value;

				Invalidate ();


				if (_dinfo.SpecialAuthRequired) {
					this.Visibility = ViewStates.Visible;
				} else {
					this.Visibility = ViewStates.Gone;
				}

			}

		}
	}
}

