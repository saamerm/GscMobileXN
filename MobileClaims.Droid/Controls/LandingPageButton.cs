using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;

namespace MobileClaims.Droid
{		
	public class LandingPageButton : LinearLayout
	{

		protected ImageView buttonImage;
		protected TextView buttonText;
		protected FrameLayout divider;
		protected bool touchExited;

		public LandingPageButton (Context context) :
		base (context)
		{
			Initialize ();
		}

		public LandingPageButton (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public LandingPageButton (Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{

			int imagewidth;
			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				imagewidth=120;
			} else {
				imagewidth=55;
			}


			var imagedp = (int) ((imagewidth)*Resources.DisplayMetrics.Density);


			buttonImage = new ImageView (this.Context);
			if(activeResID > 0)
				buttonImage.SetImageResource (Resource.Drawable.menu_active_balances);
			buttonImage.LayoutParameters = new LayoutParams(imagedp, LayoutParams.WrapContent);
			this.AddView (buttonImage);


			int whitespace=5;
			var whitespacedp = (int) ((whitespace)*Resources.DisplayMetrics.Density);

			View view = new View(this.Context);
			view.LayoutParameters = new LayoutParams(whitespacedp, LayoutParams.FillParent);
			view.SetBackgroundColor (Color.White);
			this.AddView (view);


			float textSize;

			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				textSize = 40.0f;
			} else {
				textSize = 28.0f;
			}

			int padding=10;
			if (this.Resources.GetBoolean (Resource.Boolean.isTablet)) {
				padding=25;
			} else {
				padding=5;
			}
			var paddingdp = (int) ((padding)*Resources.DisplayMetrics.Density);


			Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");
			buttonText = new TextView (this.Context);
			buttonText.LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
			buttonText.SetTypeface (leagueFont, TypefaceStyle.Normal);
			buttonText.SetTextSize (ComplexUnitType.Dip, textSize);

			buttonText.SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
			buttonText.SetPadding(paddingdp,0,0,0);
			this.AddView (buttonText);

			this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
		}


		public override bool OnTouchEvent (MotionEvent e)
		{
			bool isInsideX = e.GetX() > 0 && e.GetX() < this.Width;
			bool isInsideY = e.GetY() > 0 && e.GetY() < this.Height;
			bool isInside = isInsideX && isInsideY;

			if (!isInside && !touchExited) {
				touchExited = true;
				buttonImage.SetImageResource (this.activeResID);
				buttonText.SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
				this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
			}

			if (e.Action == MotionEventActions.Down && !touchExited) {
				buttonImage.SetImageResource (this.pressedResID);
				buttonText.SetTextColor (Resources.GetColor (Resource.Color.white));
				this.SetBackgroundColor (Resources.GetColor (Resource.Color.highlight_color));
			} else if (e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel) {
				buttonImage.SetImageResource (this.activeResID);
				buttonText.SetTextColor (Resources.GetColor (Resource.Color.highlight_color));
				this.SetBackgroundColor (Resources.GetColor (Resource.Color.light_grey));
				this.touchExited = false;
			}

			return base.OnTouchEvent (e);
		}

		public void setValues(int activeImageID, int pressedImageID, string titleText)
		{
			this.activeResID = activeImageID;
			this.pressedResID = pressedImageID;
			this.text = titleText;
		}

		private int _activeResID;

		public int activeResID
		{
			get{return _activeResID;}
			set{

				_activeResID = value;
				if(buttonImage != null)
					buttonImage.SetImageResource (value);
			}
		}

		private int _pressedResID;

		public int pressedResID
		{
			get{return _pressedResID;}
			set{

				_pressedResID = value;

			}

		}

		private string _text;

		public string text
		{
			get{return _text;}
			set{

				_text = value;
				buttonText.Text = value;
			}

		}

		public override void Draw (Android.Graphics.Canvas canvas)
		{
			base.Draw (canvas);

			float buttonInitial = 10;
			float buttonArea = 110;


			//buttonImage.SetX (buttonInitial + buttonArea/2 - buttonImage.Width/2);
			buttonImage.SetY( this.Height / 2 - buttonImage.Height / 2);

			//buttonText.SetX (buttonInitial*2 + buttonArea);
			buttonText.SetY( this.Height / 2 - buttonText.Height / 2);

		}

	}
}

