using Android.Content;
using Android.Widget;
using Android.Util;

namespace MobileClaims.Droid
{
    class GSCCheckBox:CheckBox
    {
        public GSCCheckBox(Context context)
            : base(context)
        {
            Initialize();
        }
        public GSCCheckBox(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }
		public GSCCheckBox(Context context, IAttributeSet attrs, int defStyle) :
		base (context, attrs, defStyle)
		{
			Initialize ();
		}

        public void Initialize()
		{
			SetButtonDrawable(Resource.Drawable.GSC_checkbox);
		}

    }
}