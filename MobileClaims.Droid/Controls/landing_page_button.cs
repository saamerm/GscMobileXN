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

namespace MobileClaims.Droid
{		
	public class landing_page_button : LinearLayout
	{

		protected ImageView buttonImage;
		protected TextView buttonText;
		protected FrameLayout divider;
		protected bool touchExited;

		public landing_page_button (Context context) :
		base (context)
		{
			Initialize ();
		}

		public landing_page_button (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public landing_page_button (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			System.Console.WriteLine ("test");
			buttonImage = new ImageView (this.Context);
			buttonImage.SetImageResource (Resource.Drawable.balances_active);
			buttonImage.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
			this.AddView (buttonImage);
			buttonText = new TextView (this.Context);
			buttonText.Text = Resources.GetString (Resource.String.myBalances);
			buttonText.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
			buttonText.SetTextSize (ComplexUnitType.Dip, 32.0f);

			buttonText.SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
			this.AddView (buttonText);

			this.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.gsc_button_background));
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			bool isInsideX = e.GetX() > 0 && e.GetX() < this.Width;
			bool isInsideY = e.GetY() > 0 && e.GetY() < this.Height;
			bool isInside = isInsideX && isInsideY;

			if (!isInside && !touchExited) {
				touchExited = true;
				buttonImage.SetImageResource (Resource.Drawable.balances_active);
				buttonText.SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
			}

			if (e.Action == MotionEventActions.Down && !touchExited) {
				buttonImage.SetImageResource (Resource.Drawable.balances_touch);
				buttonText.SetTextColor (Resources.GetColor (Resource.Color.white));
			} else if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel) {
				buttonImage.SetImageResource (Resource.Drawable.balances_active);
				buttonText.SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
				this.touchExited = false;
			}

			return base.OnTouchEvent (e);
		}

		public override void Draw (Android.Graphics.Canvas canvas)
		{
			base.Draw (canvas);

			buttonImage.SetX (10);
			buttonImage.SetY( this.Height / 2 - buttonImage.Height / 2);

			buttonText.SetX (buttonImage.GetX() + buttonImage.Width + 10);
			buttonText.SetY( this.Height / 2 - buttonText.Height / 2);
		}

	}
}

