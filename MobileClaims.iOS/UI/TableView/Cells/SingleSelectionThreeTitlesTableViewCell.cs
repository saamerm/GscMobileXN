using System;
using UIKit;
using CoreGraphics;

namespace MobileClaims.iOS
{
	public class SingleSelectionThreeTitlesTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel label,label2,label3;
		protected UIView cellBackingView;

		public SingleSelectionThreeTitlesTableViewCell() : base () {}
		public SingleSelectionThreeTitlesTableViewCell(IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (label == null)
				label = new UILabel ();
			label.Frame = new CGRect (Constants.SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT, Constants.SINGLE_SELECTION_LOCATED_PROVIDERS_CELL_HEIGHT/2-Constants.SINGLE_SELECTION_LOCATED_PROVIDERS_CELL_CONTENTS_HEIGHT/2, Frame.Width-Constants.SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE+5);
			label.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			label.BackgroundColor = Colors.Clear;
			label.Lines = 0;
			label.AdjustsFontSizeToFitWidth = true;
			AddSubview (label);

			if (label2 == null)
				label2 = new UILabel ();
			label2.Frame = new CGRect (Constants.SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT, (float)label.Frame.Y+(float)label.Frame.Height+Constants.SINGLE_SELECTION_LOCATED_PROVIDERS_INNER_MARGIN, Frame.Width-Constants.SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE+5);
			label2.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			label2.BackgroundColor = Colors.Clear;
			label2.Lines = 0;
			label2.AdjustsFontSizeToFitWidth = true;
			AddSubview (label2);

			if (label3 == null)
				label3 = new UILabel ();
			label3.Frame = new CGRect (Constants.SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT, (float)label2.Frame.Y+(float)label2.Frame.Height+Constants.SINGLE_SELECTION_LOCATED_PROVIDERS_INNER_MARGIN, Frame.Width-Constants.SINGLE_SELECTION_ACCENTED_TITLE_SUBTITLE_PADDING_LEFT, Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE+5);
			label3.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			label3.BackgroundColor = Colors.Clear;
			label3.Lines = 0;
			label3.AdjustsFontSizeToFitWidth = true;
			AddSubview (label3);
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

		private void SetHighlightColors(bool selected)
		{
			if (selected) {
				label.TextColor = Colors.SINGLE_SELECTION_LABEL_HIGHLIGHT_COLOR;
				cellBackingView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			} else {
				label.TextColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
				cellBackingView.BackgroundColor = Colors.LightGrayColor;
			}

			label2.TextColor = label.TextColor;
			label3.TextColor = label.TextColor;
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
			if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) == UITableViewCellState.ShowingDeleteConfirmationMask && ShowsDeleteButton ()) {
				UIView target = this.Subviews [this.Subviews.Length - 1];
				base.deleteFrame = new CGRect ((float)target.Frame.Width - (float)this.Frame.Width, (float)label.Frame.Y, (float)target.Frame.Width, (float)label.Frame.Height);
				base.WillTransitionToState (mask);
			}
		}
	}
}

