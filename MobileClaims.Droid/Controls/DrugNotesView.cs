using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;
using Android.Graphics;


namespace MobileClaims.Droid
{
	public class DrugNotesView :  LinearLayout, View.IOnTouchListener
	{


		public DrugNotesView (Context context) :
			base (context)
		{
			Initialize ();

		}

		public DrugNotesView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}

		public bool OnTouch(View v, MotionEvent e)
		{
			return true;
		}

		private DrugInfo _dinfo;
		public DrugInfo dInfo
		{
			get{
				return _dinfo;
			}
			set{
				_dinfo = value;
				if(this.ChildCount >0)
					this.RemoveAllViews ();

				Invalidate ();

				Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");



//				if (_dinfo.SpecialAuthRequired) {
					TextView divider1 = new TextView (this.Context);
					divider1.SetText ("", TextView.BufferType.Normal);
					divider1.SetTypeface (leagueFont, TypefaceStyle.Normal);
					divider1.Gravity = GravityFlags.Center;
					divider1.SetPadding (15, 0, 15, 0);
					divider1.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
					divider1.SetLayerType (LayerType.Software, null);
					this.AddView (divider1);
//				}

				TextView notesLabel = new TextView(this.Context);
				notesLabel.SetText (Resources.GetString(Resource.String.notes), TextView.BufferType.Normal);
				notesLabel.SetTypeface (leagueFont, TypefaceStyle.Normal);
				notesLabel.Gravity = GravityFlags.Left;
				notesLabel.SetPadding (15, 10, 0, 0);
				notesLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				notesLabel.SetTextSize (ComplexUnitType.Dip, 22.0f);
				this.AddView(notesLabel);

				TextView notesField = new TextView(this.Context);
				notesField.SetText (_dinfo.Notes, TextView.BufferType.Normal);
				notesField.SetTypeface (leagueFont, TypefaceStyle.Normal);
				notesField.Gravity = GravityFlags.Left;
				notesField.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				notesField.SetBackgroundColor (Android.Graphics.Color.White);
				notesField.SetTextSize (ComplexUnitType.Dip, 18.0f);
				notesField.SetPadding (15, 0, 0, 10);
				this.AddView(notesField);

//				TextView notesFieldDesc = new TextView(this.Context);
//				notesFieldDesc.TextFormatted = Html.FromHtml (Resources.GetString (Resource.String.DrugsNotesDesc).ToString ());
//				notesFieldDesc.Gravity = GravityFlags.Left;
//				notesFieldDesc.SetTextColor (Android.Graphics.Color.Black);
//				notesFieldDesc.SetBackgroundColor (Android.Graphics.Color.White);
//				notesFieldDesc.SetTextSize (ComplexUnitType.Dip, 14.0f);
//				notesFieldDesc.SetPadding (15, 0, 0, 10);
//				this.AddView(notesFieldDesc);
			}
		}

		public override void Draw (Android.Graphics.Canvas canvas)
		{
			base.Draw (canvas);

		}
	}
}

