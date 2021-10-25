using Android.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Views;

namespace MobileClaims.Droid
{
	class SingleSelectionListItemView : MvxListItemView, ICheckable
	{
        static readonly int[] CHECKED_STATE_SET = {Android.Resource.Attribute.StateChecked};
		private bool mChecked;

		public SingleSelectionListItemView(Context context,
            IMvxLayoutInflaterHolder layoutInflater,
			object dataContext,
			ViewGroup parent,
			int templateId)
			: base(context, layoutInflater, dataContext, parent, templateId)
		{         
		}

        public void Toggle()
        {
            Checked = !mChecked;
        }

        public bool Checked
        {
            get => mChecked;
            set
            {
                if (value != mChecked)
                {
                    mChecked = value;

					// TODO: Find proper corresponding method
                    // RefreshDrawableState ();
				}
			}
        }

		// TODO: Find appropriate method for below
		//protected override int[] OnCreateDrawableState (int extraSpace)
		//{
		//	int[] drawableState = base.OnCreateDrawableState (extraSpace + 1);

		//	if (Checked)
		//		MergeDrawableStates (drawableState, CHECKED_STATE_SET);

		//	return drawableState;
		//}
	}
}