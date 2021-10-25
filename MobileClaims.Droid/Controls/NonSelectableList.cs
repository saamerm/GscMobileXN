using Android.Content;
using Android.Util;
using MvvmCross.Platforms.Android.Binding.Views;


namespace MobileClaims.Droid
{
	public class NonSelectableList : MvxListView
	{
		public NonSelectableList (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}


		void Initialize ()
		{
			//Enabled = false;
		}

	}
}

