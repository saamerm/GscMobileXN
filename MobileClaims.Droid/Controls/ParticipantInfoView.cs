using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;
using Android.Graphics;

namespace MobileClaims.Droid
{
	public class ParticipantInfoView : LinearLayout, View.IOnTouchListener
	{
		public ParticipantInfoView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public ParticipantInfoView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public bool OnTouch(View v, MotionEvent e)
		{
			return true;
		}


		void Initialize ()
		{
		}

		private Participant _participant;
		public Participant participant {
			get {
				return _participant;
			}
			set {
				_participant = value;
				if(this.ChildCount >0)
					this.RemoveAllViews ();

				Invalidate ();

				TextView participantLabel = new TextView(this.Context);
				Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

				participantLabel.SetTypeface (leagueFont, TypefaceStyle.Normal);
				participantLabel.SetText (Resources.GetString(Resource.String.planParticipant), TextView.BufferType.Normal);
				participantLabel.Gravity = GravityFlags.Right;
				participantLabel.SetPadding (15, 0, 40, 0);
				participantLabel.SetTextColor (Resources.GetColor( Resource.Color.dark_grey));
				participantLabel.SetTextSize (ComplexUnitType.Dip, 22.0f);
				this.AddView(participantLabel);

				TextView nameLabel = new TextView(this.Context);
				nameLabel.SetTypeface (leagueFont, TypefaceStyle.Normal);
				nameLabel.SetText (participant.FullName, TextView.BufferType.Normal);
				nameLabel.Gravity = GravityFlags.Right;
				nameLabel.SetTextColor (Resources.GetColor( Resource.Color.highlight_color));
				nameLabel.SetTextSize (ComplexUnitType.Dip, 22.0f);
				nameLabel.SetPadding (15, 0, 40, 0);
				this.AddView(nameLabel);
			}

		}
	}
}

