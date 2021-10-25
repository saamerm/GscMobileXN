using System;
using UIKit;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	public class MvxDeleteTableViewCell : MvxTableViewCell
	{
		public CGRect deleteFrame;

		public MvxDeleteTableViewCell()            
		{
			CreateLayout();
			InitializeBindings();
		}

		public MvxDeleteTableViewCell(IntPtr handle)
			: base(handle)
		{
			CreateLayout();
			InitializeBindings();
		}

		public virtual void CreateLayout()
		{
			//override this in a class to create a globally accessible cell with distinct styling
		}

		public virtual void InitializeBindings ()
		{
			//override this in the view to set binds
		}

		public virtual bool ShowsDeleteButton()
		{
			return true;
		}

		public override void WillTransitionToState (UITableViewCellState mask)
		{
			base.WillTransitionToState (mask);
		}
	}
}

