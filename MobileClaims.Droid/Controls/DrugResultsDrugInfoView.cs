using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MobileClaims.Core.Entities;
using Android.Graphics;
using MobileClaims.Droid.Helpers;


namespace MobileClaims.Droid
{
	public class DrugResultsDrugInfoView :  LinearLayout, View.IOnTouchListener
	{


		public DrugResultsDrugInfoView (Context context) :
			base (context)
		{
			Initialize ();

		}

		public DrugResultsDrugInfoView (Context context, IAttributeSet attrs) :
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

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);


		}

		private DrugInfo _dinfo;
		public DrugInfo dInfo
		{
			get{
				return _dinfo;
			}
			set{
				_dinfo = value;

				if (this.ChildCount > 0)
					this.RemoveAllViews ();

				Invalidate ();

				Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");



				TextView drugInfoLabel = new TextView (this.Context);
				drugInfoLabel.SetTypeface (leagueFont, TypefaceStyle.Normal);
				drugInfoLabel.SetText (Resources.GetString (Resource.String.drugInformation), TextView.BufferType.Normal);
				drugInfoLabel.Gravity = GravityFlags.Left;
				drugInfoLabel.SetPadding (15, 10, 0, 0);
				drugInfoLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				drugInfoLabel.SetTextSize (ComplexUnitType.Dip, 22.0f);
				this.AddView (drugInfoLabel);

				Typeface nunitoFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");
				TextView divider1 = new TextView (this.Context);
				divider1.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				divider1.SetText ("", TextView.BufferType.Normal);
				divider1.Gravity = GravityFlags.Center;
				divider1.SetPadding (15, 0, 15, 0);
				divider1.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
				divider1.SetLayerType (LayerType.Software, null);
				this.AddView (divider1);

				TextView nameTitle = new TextView (this.Context);
				nameTitle.SetText (Resources.GetString (Resource.String.drugNameAndStrength), TextView.BufferType.Normal);
				nameTitle.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				nameTitle.Gravity = GravityFlags.Left;
				nameTitle.SetPadding (15, 10, 0, 0);
				nameTitle.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				nameTitle.SetBackgroundColor (Android.Graphics.Color.White);
				nameTitle.SetTextSize (ComplexUnitType.Dip, 18.0f);
				this.AddView (nameTitle);

				TextView nameLabel = new TextView (this.Context);
				nameLabel.SetText (_dinfo.Name, TextView.BufferType.Normal);
				nameLabel.Gravity = GravityFlags.Left;
				nameLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				nameLabel.SetBackgroundColor (Android.Graphics.Color.White);
				nameLabel.SetTextSize (ComplexUnitType.Dip, 22.0f);
				nameLabel.SetPadding (15, 0, 0, 10);
				this.AddView (nameLabel);

				TextView divider2 = new TextView (this.Context);
				divider2.SetText ("", TextView.BufferType.Normal);
				divider2.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				divider2.Gravity = GravityFlags.Center;
				divider2.SetPadding (15, 0, 15, 0);
				divider2.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
				divider2.SetLayerType (LayerType.Software, null);
				this.AddView (divider2);

				TextView dinTitle = new TextView (this.Context);
				dinTitle.SetText (Resources.GetString (Resource.String.drugDINLabel), TextView.BufferType.Normal);
				dinTitle.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				dinTitle.Gravity = GravityFlags.Left;
				dinTitle.SetPadding (15, 10, 0, 0);
				dinTitle.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				dinTitle.SetBackgroundColor (Android.Graphics.Color.White);
				dinTitle.SetTextSize (ComplexUnitType.Dip, 18.0f);
				this.AddView (dinTitle);

				TextView dinLabel = new TextView (this.Context);
				dinLabel.SetText (_dinfo.DIN.ToString (), TextView.BufferType.Normal);
				dinLabel.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				dinLabel.Gravity = GravityFlags.Left;
				dinLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				dinLabel.SetBackgroundColor (Android.Graphics.Color.White);
				dinLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				dinLabel.SetPadding (15, 0, 0, 10);
				this.AddView (dinLabel);

				TextView divider3 = new TextView (this.Context);
				divider3.SetText ("", TextView.BufferType.Normal);
				divider3.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				divider3.Gravity = GravityFlags.Center;
				divider3.SetPadding (15, 0, 15, 0);
				divider3.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
				divider3.SetLayerType (LayerType.Software, null);
				this.AddView (divider3);

				TextView coveredTitle = new TextView (this.Context);
				coveredTitle.SetText (Resources.FormatterBrandKeywords(Resource.String.drugCovered, new string[] { Resources.GetString(Resource.String.greenshieldcanada) }), TextView.BufferType.Normal);
				coveredTitle.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				coveredTitle.Gravity = GravityFlags.Left;
				coveredTitle.SetPadding (15, 10, 0, 0);
				coveredTitle.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				coveredTitle.SetBackgroundColor (Android.Graphics.Color.White);
				coveredTitle.SetTextSize (ComplexUnitType.Dip, 18.0f);
                coveredTitle.SetAllCaps(true);
				this.AddView (coveredTitle);

				TextView coveredLabel = new TextView (this.Context);
				coveredLabel.SetText (_dinfo.Covered ? "Covered" : _dinfo.CoveredMessage, TextView.BufferType.Normal);
				coveredLabel.SetTypeface (nunitoFont, TypefaceStyle.Normal);
				coveredLabel.Gravity = GravityFlags.Left;
				coveredLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
				coveredLabel.SetBackgroundColor (Android.Graphics.Color.White);
				coveredLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
				coveredLabel.SetPadding (15, 0, 0, 10);
				this.AddView (coveredLabel);



				if(_dinfo.Reimbursement != null && _dinfo.Reimbursement != String.Empty){
						TextView divider4 = new TextView (this.Context);
						divider4.SetText ("", TextView.BufferType.Normal);
						divider4.SetTypeface (nunitoFont, TypefaceStyle.Normal);
						divider4.Gravity = GravityFlags.Center;
						divider4.SetPadding (15, 0, 15, 0);
						divider4.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
						divider4.SetLayerType (LayerType.Software, null);
						this.AddView (divider4);
						
						TextView reimbursementTitle = new TextView (this.Context);
						reimbursementTitle.SetText (Resources.GetString (Resource.String.reimbursement), TextView.BufferType.Normal);
						reimbursementTitle.SetTypeface (nunitoFont, TypefaceStyle.Normal);
						reimbursementTitle.Gravity = GravityFlags.Left;
						reimbursementTitle.SetPadding (15, 10, 0, 0);
						reimbursementTitle.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
						reimbursementTitle.SetBackgroundColor (Android.Graphics.Color.White);
						reimbursementTitle.SetTextSize (ComplexUnitType.Dip, 18.0f);
						this.AddView (reimbursementTitle);

						TextView reimbursementLabel = new TextView (this.Context);
						reimbursementLabel.SetText (_dinfo.Reimbursement, TextView.BufferType.Normal);
						reimbursementLabel.SetTypeface (nunitoFont, TypefaceStyle.Normal);
						reimbursementLabel.Gravity = GravityFlags.Left;
						reimbursementLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
						reimbursementLabel.SetBackgroundColor (Android.Graphics.Color.White);
						reimbursementLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
						reimbursementLabel.SetPadding (15, 0, 0, 10);
						this.AddView (reimbursementLabel);

						
					}
				if (_dinfo.SpecialAuthRequired) {

					TextView divider5 = new TextView (this.Context);
					divider5.SetText ("", TextView.BufferType.Normal);
					divider5.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					divider5.Gravity = GravityFlags.Center;
					divider5.SetPadding (15, 0, 15, 0);
					divider5.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
					divider5.SetLayerType (LayerType.Software, null);
					this.AddView (divider5);

					TextView specialAuthTitle = new TextView (this.Context);
					specialAuthTitle.SetText (Resources.GetString (Resource.String.specialAuthRequired), TextView.BufferType.Normal);
					specialAuthTitle.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					specialAuthTitle.Gravity = GravityFlags.Left;
					specialAuthTitle.SetPadding (15, 10, 0, 0);
					specialAuthTitle.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
					specialAuthTitle.SetBackgroundColor (Android.Graphics.Color.White);
					specialAuthTitle.SetTextSize (ComplexUnitType.Dip, 18.0f);
					this.AddView (specialAuthTitle);

					TextView specialAuthLabel = new TextView (this.Context);
					specialAuthLabel.SetText (_dinfo.SpecialAuthRequired ? _dinfo.AuthorizationMessage : "No", TextView.BufferType.Normal);
					specialAuthLabel.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					specialAuthLabel.Gravity = GravityFlags.Left;
					specialAuthLabel.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
					specialAuthLabel.SetBackgroundColor (Android.Graphics.Color.White);
					specialAuthLabel.SetTextSize (ComplexUnitType.Dip, 18.0f);
					specialAuthLabel.SetPadding (15, 0, 0, 10);
					this.AddView (specialAuthLabel);
				}

				if (_dinfo.LowCostReplacementOccurred) {

					TextView divider6 = new TextView (this.Context);
					divider6.SetText ("", TextView.BufferType.Normal);
					divider6.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					divider6.Gravity = GravityFlags.Center;
					divider6.SetPadding (15, 0, 15, 0);
					divider6.SetBackgroundDrawable (Resources.GetDrawable (Resource.Drawable.dotted_line));
					divider6.SetLayerType (LayerType.Software, null);
					this.AddView (divider6);

					TextView specialMessagesTitle = new TextView (this.Context);
					specialMessagesTitle.SetText (Resources.GetString (Resource.String.specialMessages).ToUpper(), TextView.BufferType.Normal);
					specialMessagesTitle.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					specialMessagesTitle.Gravity = GravityFlags.Left;
					specialMessagesTitle.SetPadding (15, 10, 0, 0);
					specialMessagesTitle.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
					specialMessagesTitle.SetBackgroundColor (Android.Graphics.Color.White);
					specialMessagesTitle.SetTextSize (ComplexUnitType.Dip, 18.0f);
					this.AddView (specialMessagesTitle);

					TextView specialMessagesDesc = new TextView (this.Context);
					specialMessagesDesc.SetText (Resources.GetString (Resource.String.reimbursementMessage), TextView.BufferType.Normal);
					specialMessagesDesc.SetTypeface (nunitoFont, TypefaceStyle.Normal);
					specialMessagesDesc.Gravity = GravityFlags.Left;
					specialMessagesDesc.SetTextColor (Resources.GetColor (Resource.Color.dark_grey));
					specialMessagesDesc.SetBackgroundColor (Android.Graphics.Color.White);
					specialMessagesDesc.SetTextSize (ComplexUnitType.Dip, 18.0f);
					specialMessagesDesc.SetPadding (15, 0, 0, 10);
					this.AddView (specialMessagesDesc);
				}
			}
		}
	}
}

