using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace MobileClaims.iOS
{
	public class SingleSelectionTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel label;
		protected UIView cellBackingView;

		public SingleSelectionTableViewCell() : base () {}
		public SingleSelectionTableViewCell(IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

            if (cellBackingView == null)
            {
                cellBackingView = new UIView();
            }

			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height + 40);
            cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
            cellBackingView.ContentMode = UIViewContentMode.TopLeft;
            cellBackingView.BackgroundColor = Colors.BACKGROUND_COLOR;
			BackgroundView = cellBackingView;

            if (label == null)
            {
                label = new UILabel();
            }

			label.Frame = new CGRect (0, 0, (float)this.Frame.Width, 50);
			label.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC,(nfloat)Constants.LIST_ITEM_FONT_SIZE);
			label.TextAlignment = UITextAlignment.Center;
            label.ContentMode = UIViewContentMode.Center;
            label.Lines = 0;
			label.AdjustsFontSizeToFitWidth = true;
			AddSubview (label);
		}

		public override void SetSelected (bool selected, bool animated)
		{
			SetHighlightColors (selected);
			base.SetSelected (selected, animated);
		}

		public override void SetHighlighted (bool highlighted, bool animated)
		{
			//if (!PersistsSelection())
			SetHighlightColors (highlighted);
			base.SetHighlighted (highlighted, animated);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			SetHighlightColors (Selected);
			base.TouchesEnded (touches, evt);
		}

		public override void TouchesCancelled (NSSet touches, UIEvent evt)
		{
			SetHighlightColors (Selected);
			base.TouchesCancelled (touches, evt);
		}
			
		private void SetHighlightColors(bool selected)
        {
			if (selected) {
                if (string.IsNullOrEmpty(label.Text))
                {  
                    label.BackgroundColor = Colors.LightGrayColor;
                }
                else
                {

                    label.TextColor = Colors.SINGLE_SELECTION_LABEL_HIGHLIGHT_COLOR;
                    label.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                }
			} else {
				label.TextColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
				label.BackgroundColor = Colors.LightGrayColor;
			}
		}

		public override bool ShowsDeleteButton ()
		{
			return false;
		}

		public virtual bool PersistsSelection()
		{
			return true;
		}
			
		public override void WillTransitionToState (UITableViewCellState mask)
		{
			//set delete button frame to account for cell margins
			if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) == UITableViewCellState.ShowingDeleteConfirmationMask && ShowsDeleteButton ())
            {
				UIView target = this.Subviews [this.Subviews.Length - 1];
				base.deleteFrame = new CGRect ((float)target.Frame.Width - (float)this.Frame.Width, (float)label.Frame.Y, (float)target.Frame.Width, (float)label.Frame.Height);
				base.WillTransitionToState (mask);
			}
		}
	}
}

