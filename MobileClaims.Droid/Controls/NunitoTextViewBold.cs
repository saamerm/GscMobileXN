using System;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Util;

namespace MobileClaims.Droid
{
    [Register("gsc.NunitoTextViewBold")]
    public class NunitoTextViewBold : TextView
    {


        protected TextView headerText;


        public NunitoTextViewBold(Context context) :
            base(context)
        {
            Initialize();
        }

        public NunitoTextViewBold(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public NunitoTextViewBold(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {

            //Typeface leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/LeagueGothic.ttf");

            //SetTypeface (leagueFont, TypefaceStyle.Normal);
            //SetTextSize (ComplexUnitType.Dip, textSize);

            Typeface leagueFont;
            try
            {
                leagueFont = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/NunitoSansRegular.ttf");
                SetTypeface(leagueFont, TypefaceStyle.Bold);
            }
            catch (Exception e)
            {
                //Log.Error(Tag, string.Format("Could not get Typeface: Error: {1}", e));
                return;
            }

            if (null == leagueFont) return;

            var tfStyle = TypefaceStyle.Bold;
            if (null != Typeface)
                tfStyle = Typeface.Style;
            SetTypeface(leagueFont, tfStyle);

        }


    }
   
}