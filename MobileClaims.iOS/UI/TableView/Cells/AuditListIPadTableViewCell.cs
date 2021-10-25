using System;
using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("AuditListIPadTableViewCell")]
	public class AuditListIPadTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel formNumLabel;
		public UILabel submittedDateLabel;
		public UILabel dueDateLabel;

		protected UIView cellBackingView;

		public AuditListIPadTableViewCell () : base () {

			titleColor = Colors.HIGHLIGHT_COLOR;
			backgroundColor = Colors.BACKGROUND_COLOR;
			labelColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
		}
		public AuditListIPadTableViewCell (IntPtr handle) : base (handle) {

			titleColor = Colors.HIGHLIGHT_COLOR;
			backgroundColor = Colors.BACKGROUND_COLOR;
			labelColor = Colors.SINGLE_SELECTION_LABEL_COLOR;

		}

		private UIColor titleColor;
		private UIColor backgroundColor;
		private UIColor labelColor;

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float fieldPadding = Constants.CLAIMS_RESULTS_IPAD_PADDING;
			float bottomPadding = 10;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = (float)this.Frame.Width;
			float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth/2 + fieldPadding;
			float topPadding = 15;

			float fieldWidth = ((float)this.Frame.Width - fieldPadding*2) / 3;

			float xPos = fieldPadding;
			float yPos = topPadding;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (formNumLabel == null)
				formNumLabel = new UILabel ();
			formNumLabel.Frame = new CGRect (xPos, yPos, fieldWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			formNumLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			formNumLabel.TextColor = labelColor;
			formNumLabel.TextAlignment = UITextAlignment.Left;
			formNumLabel.BackgroundColor = Colors.Clear;
			formNumLabel.Lines = 0;
			formNumLabel.SizeToFit ();
			formNumLabel.Frame = new CGRect (xPos, yPos, (float)formNumLabel.Frame.Width, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			formNumLabel.AdjustsFontSizeToFitWidth = true;
			formNumLabel.UserInteractionEnabled = false;
			AddSubview (formNumLabel);

			xPos += (float)formNumLabel.Frame.Height + fieldWidth;

			if (submittedDateLabel == null)
				submittedDateLabel = new UILabel ();
			submittedDateLabel.Frame = new CGRect (xPos, yPos, fieldWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			submittedDateLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			submittedDateLabel.TextColor = labelColor;
			submittedDateLabel.TextAlignment = UITextAlignment.Left;
			submittedDateLabel.BackgroundColor = Colors.Clear;
			submittedDateLabel.Lines = 0;
			submittedDateLabel.SizeToFit ();
			submittedDateLabel.Frame = new CGRect (xPos, yPos, (float)submittedDateLabel.Frame.Width, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			submittedDateLabel.AdjustsFontSizeToFitWidth = true;
			submittedDateLabel.UserInteractionEnabled = false;
			AddSubview (submittedDateLabel);

			xPos += (float)submittedDateLabel.Frame.Height + fieldWidth;

			if (dueDateLabel == null)
				dueDateLabel = new UILabel ();
			dueDateLabel.Frame = new CGRect (xPos, yPos, fieldWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			dueDateLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			dueDateLabel.TextColor = labelColor;
			dueDateLabel.TextAlignment = UITextAlignment.Left;
			dueDateLabel.BackgroundColor = Colors.Clear;
			dueDateLabel.Lines = 0;
			dueDateLabel.SizeToFit ();
			dueDateLabel.Frame = new CGRect (xPos, yPos, (float)dueDateLabel.Frame.Width, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			dueDateLabel.AdjustsFontSizeToFitWidth = true;
			dueDateLabel.UserInteractionEnabled = false;
			AddSubview (dueDateLabel);

			xPos += (float)dueDateLabel.Frame.Height + fieldWidth;
		}

		string _indexValue = "0";
		public void setIndex(int indexValue)
		{
			_indexValue = indexValue.ToString();
		}

		public override void SetSelected (bool selected, bool animated)
		{
			SetHighlightColors (selected);
			base.SetSelected (selected, animated);
		}

		public override void SetHighlighted (bool highlighted, bool animated)
		{
			SetHighlightColors (highlighted);
			base.SetHighlighted (highlighted, animated);
		}

		private void SetHighlightColors(bool selected)
		{
			if (selected) {
				formNumLabel.TextColor = labelColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
				cellBackingView.BackgroundColor = backgroundColor = Colors.HIGHLIGHT_COLOR;
			} else {
				formNumLabel.TextColor = labelColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
				cellBackingView.BackgroundColor = backgroundColor = Colors.BACKGROUND_COLOR;
			}

			submittedDateLabel.TextColor = formNumLabel.TextColor;
			dueDateLabel.TextColor = formNumLabel.TextColor;
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
			base.WillTransitionToState (mask);
		}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<AuditListIPadTableViewCell, ClaimAudit>();
					set.Bind(this.formNumLabel).To(item => item.ClaimFormID);
					set.Bind(this.submittedDateLabel).To(item => item.SubmissionDate).WithConversion("DateToString");
					set.Bind(this.dueDateLabel).To(item => item.DueDate ).WithConversion("DateToString");
					//set.Bind(this.treatmentLabel).To(item => item.TypeOfTreatmentListViewItem.Name ).OneWay();
//					set.Bind(this.altPaymentLabel).To(item => item.AlternateCarrierPayment).WithConversion("AltCarrierPrefix").OneWay();
//					set.Bind(this.paymentLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");
					set.Apply();

					this.SetNeedsLayout();
				});
		}

	}
}

