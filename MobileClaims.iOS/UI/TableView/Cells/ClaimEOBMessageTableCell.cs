using System;
using MobileClaims.Core.Entities;
using UIKit;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimEOBMessageTableCell")]
	public class ClaimEOBMessageTableCell : SingleSelectionTableViewCell
	{
		protected UIView cellBackingView;

		public ClaimEOBMessageTableCell () : base () {}
		public ClaimEOBMessageTableCell (IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			cellBackingView.BackgroundColor = Colors.Clear;
			BackgroundView = cellBackingView;

			if (label == null)
				label = new UILabel ();
			label.Frame = new CGRect (Constants.DRUG_LOOKUP_SIDE_PADDING, Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING/2, (float)this.Frame.Width, (float)this.Frame.Height-Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING);
			label.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			label.TextAlignment = UITextAlignment.Left;
			label.ContentMode = UIViewContentMode.Center;
			label.BackgroundColor = Colors.Clear;
			label.Lines = 0;
			AddSubview (label);

		}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimEOBMessageTableCell, ClaimEOBMessageGSC>();
					set.Bind(this.label).To(item => item.Message).OneWay();
					set.Apply();
				});
		}
	}
}

