using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	[Foundation.Register("ParticipantCellTemplate")]
	public class ParticipantCellTemplate : MvxTableViewCell
    {
		protected SelectionMenuItem cellItem;
		public ParticipantCellTemplate(IntPtr Handle) :base(Handle)
        {
			this.DelayBind(() =>
			{

			});
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			cellItem = new SelectionMenuItem(false);
			cellItem.Frame = new CoreGraphics.CGRect(10, 10, (float)base.Frame.Width, 200);
		}
    }
}

