using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;
using MobileClaims.Droid.Helpers;
namespace MobileClaims.Droid
{
	public class visibility_check_layout :  LinearLayout, View.IOnTouchListener
	{


		public visibility_check_layout (Context context) :
			base (context)
		{
			Initialize ();

		}

		public visibility_check_layout (Context context, IAttributeSet attrs) :
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
				Invalidate ();

				TextView drugInfoLabel = new TextView(this.Context);
				drugInfoLabel.SetText (Resources.GetString(Resource.String.drugInformation), TextView.BufferType.Normal);
				drugInfoLabel.Gravity = GravityFlags.Left;
				drugInfoLabel.SetPadding (15, 10, 0, 0);
				drugInfoLabel.SetTextColor (Android.Graphics.Color.Black);
				drugInfoLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				this.AddView(drugInfoLabel);

				TextView nameTitle = new TextView(this.Context);
				nameTitle.SetText (Resources.GetString(Resource.String.drugNameAndStrength), TextView.BufferType.Normal);
				nameTitle.Gravity = GravityFlags.Left;
				nameTitle.SetPadding (15, 10, 0, 0);
				nameTitle.SetTextColor ( Resources.GetColor( Resource.Color.highlight_color) );
				nameTitle.SetBackgroundColor (Android.Graphics.Color.White);
				nameTitle.SetTextSize (ComplexUnitType.Dip, 14.0f);
				this.AddView(nameTitle);

				TextView nameLabel = new TextView(this.Context);
				nameLabel.SetText (_dinfo.Name, TextView.BufferType.Normal);
				nameLabel.Gravity = GravityFlags.Left;
				nameLabel.SetTextColor (Android.Graphics.Color.Black);
				nameLabel.SetBackgroundColor (Android.Graphics.Color.White);
				nameLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				nameLabel.SetPadding (15, 0, 0, 10);
				this.AddView(nameLabel);

				TextView dinTitle = new TextView(this.Context);
				dinTitle.SetText (Resources.GetString(Resource.String.drugDINFieldText), TextView.BufferType.Normal);
				dinTitle.Gravity = GravityFlags.Left;
				dinTitle.SetPadding (15, 10, 0, 0);
				dinTitle.SetTextColor ( Resources.GetColor( Resource.Color.highlight_color) );
				dinTitle.SetBackgroundColor (Android.Graphics.Color.White);
				dinTitle.SetTextSize (ComplexUnitType.Dip, 14.0f);
				this.AddView(dinTitle);

				TextView dinLabel = new TextView(this.Context);
				dinLabel.SetText (_dinfo.DIN.ToString(), TextView.BufferType.Normal);
				dinLabel.Gravity = GravityFlags.Left;
				dinLabel.SetTextColor (Android.Graphics.Color.Black);
				dinLabel.SetBackgroundColor (Android.Graphics.Color.White);
				dinLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				dinLabel.SetPadding (15, 0, 0, 10);
				this.AddView(dinLabel);

				TextView coveredTitle = new TextView(this.Context);
                coveredTitle.SetText(Resources.FormatterBrandKeywords(Resource.String.drugCovered, new string[] { Resources.GetString(Resource.String.greenshieldcanada) }), TextView.BufferType.Normal);
                coveredTitle.Gravity = GravityFlags.Left;
				coveredTitle.SetPadding (15, 10, 0, 0);
				coveredTitle.SetTextColor ( Resources.GetColor( Resource.Color.highlight_color) );
				coveredTitle.SetBackgroundColor (Android.Graphics.Color.White);
				coveredTitle.SetTextSize (ComplexUnitType.Dip, 14.0f);
                coveredTitle.SetAllCaps(true);
                this.AddView(coveredTitle);

				TextView coveredLabel = new TextView(this.Context);
				coveredLabel.SetText (_dinfo.Covered ? "Covered" : "No Covered", TextView.BufferType.Normal);
				coveredLabel.Gravity = GravityFlags.Left;
				coveredLabel.SetTextColor (Android.Graphics.Color.Black);
				coveredLabel.SetBackgroundColor (Android.Graphics.Color.White);
				coveredLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				coveredLabel.SetPadding (15, 0, 0, 10);
				this.AddView(coveredLabel);

				TextView reimbursementTitle = new TextView(this.Context);
				reimbursementTitle.SetText (Resources.GetString(Resource.String.reimbursement), TextView.BufferType.Normal);
				reimbursementTitle.Gravity = GravityFlags.Left;
				reimbursementTitle.SetPadding (15, 10, 0, 0);
				reimbursementTitle.SetTextColor ( Resources.GetColor( Resource.Color.highlight_color) );
				reimbursementTitle.SetBackgroundColor (Android.Graphics.Color.White);
				reimbursementTitle.SetTextSize (ComplexUnitType.Dip, 14.0f);
				this.AddView(reimbursementTitle);

				TextView reimbursementLabel = new TextView(this.Context);
				reimbursementLabel.SetText (_dinfo.Reimbursement, TextView.BufferType.Normal);
				reimbursementLabel.Gravity = GravityFlags.Left;
				reimbursementLabel.SetTextColor (Android.Graphics.Color.Black);
				reimbursementLabel.SetBackgroundColor (Android.Graphics.Color.White);
				reimbursementLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				reimbursementLabel.SetPadding (15, 0, 0, 10);
				this.AddView(reimbursementLabel);

				TextView specialAuthTitle = new TextView(this.Context);
				specialAuthTitle.SetText (Resources.GetString(Resource.String.specialAuthRequired), TextView.BufferType.Normal);
				specialAuthTitle.Gravity = GravityFlags.Left;
				specialAuthTitle.SetPadding (15, 10, 0, 0);
				specialAuthTitle.SetTextColor ( Resources.GetColor( Resource.Color.highlight_color) );
				specialAuthTitle.SetBackgroundColor (Android.Graphics.Color.White);
				specialAuthTitle.SetTextSize (ComplexUnitType.Dip, 14.0f);
				this.AddView(specialAuthTitle);

				TextView specialAuthLabel = new TextView(this.Context);
				specialAuthLabel.SetText (_dinfo.SpecialAuthRequired ? "Yes" : "No", TextView.BufferType.Normal);
				specialAuthLabel.Gravity = GravityFlags.Left;
				specialAuthLabel.SetTextColor (Android.Graphics.Color.Black);
				specialAuthLabel.SetBackgroundColor (Android.Graphics.Color.White);
				specialAuthLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				specialAuthLabel.SetPadding (15, 0, 0, 10);
				this.AddView(specialAuthLabel);
			}
		}
	}
}

