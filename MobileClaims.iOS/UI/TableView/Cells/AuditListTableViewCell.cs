using System;
using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("AuditListTableViewCell")]
	public class AuditListTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel formNumLabel;
		public UILabel submittedDateLabel;
		public UILabel dueDateLabel;

		public UILabel formNumTitleLabel;
		public UILabel submittedDateTitleLabel;
		public UILabel dueDateTitleLabel;

		protected UIView cellBackingView;

		public AuditListTableViewCell () : base () {

			titleColor = Colors.HIGHLIGHT_COLOR;
			backgroundColor = Colors.BACKGROUND_COLOR;
			labelColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
		}
		public AuditListTableViewCell (IntPtr handle) : base (handle) {

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

			float fieldPadding = 3;
			float bottomPadding = 10;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = (float)this.Frame.Width;
			float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth/2 + fieldPadding;
			float topPadding = 5;
			float yPos = topPadding;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (formNumLabel == null)
				formNumLabel = new UILabel ();
			formNumLabel.Frame = new CGRect (resultPos, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			formNumLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			formNumLabel.TextColor = labelColor;
			formNumLabel.TextAlignment = UITextAlignment.Left;
			formNumLabel.BackgroundColor = Colors.Clear;
			formNumLabel.Lines = 0;
			formNumLabel.SizeToFit ();
			formNumLabel.AdjustsFontSizeToFitWidth = true;
			formNumLabel.UserInteractionEnabled = false;
			AddSubview (formNumLabel);

			if (formNumTitleLabel == null)
				formNumTitleLabel = new UILabel ();
			formNumTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			formNumTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			formNumTitleLabel.TextAlignment = UITextAlignment.Left;
			formNumTitleLabel.TextColor = titleColor;
			formNumTitleLabel.BackgroundColor = Colors.Clear;
			formNumTitleLabel.Lines = 0;
			formNumTitleLabel.Text="formNumberNoColon".tr();
			formNumTitleLabel.SizeToFit ();
			formNumTitleLabel.AdjustsFontSizeToFitWidth = true;
			formNumTitleLabel.UserInteractionEnabled = false;
			AddSubview (formNumTitleLabel);

			yPos += (float)formNumTitleLabel.Frame.Height + fieldPadding;

			if (submittedDateLabel == null)
				submittedDateLabel = new UILabel ();
			submittedDateLabel.Frame = new CGRect (resultPos, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			submittedDateLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			submittedDateLabel.TextColor = labelColor;
			submittedDateLabel.TextAlignment = UITextAlignment.Left;
			submittedDateLabel.BackgroundColor = Colors.Clear;
			submittedDateLabel.Lines = 0;
			submittedDateLabel.SizeToFit ();
			submittedDateLabel.AdjustsFontSizeToFitWidth = true;
			submittedDateLabel.UserInteractionEnabled = false;
			AddSubview (submittedDateLabel);

			if (submittedDateTitleLabel == null)
				submittedDateTitleLabel = new UILabel ();
			submittedDateTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			submittedDateTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			submittedDateTitleLabel.TextAlignment = UITextAlignment.Left;
			submittedDateTitleLabel.TextColor = titleColor;
			submittedDateTitleLabel.BackgroundColor = Colors.Clear;
			submittedDateTitleLabel.Lines = 0;
			submittedDateTitleLabel.Text="submittedDate".tr();
			submittedDateTitleLabel.SizeToFit ();
			submittedDateTitleLabel.AdjustsFontSizeToFitWidth = true;
			submittedDateTitleLabel.UserInteractionEnabled = false;
			AddSubview (submittedDateTitleLabel);

			yPos += (float)submittedDateTitleLabel.Frame.Height + fieldPadding;

			if (dueDateLabel == null)
				dueDateLabel = new UILabel ();
			dueDateLabel.Frame = new CGRect (resultPos, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			dueDateLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			dueDateLabel.TextColor = labelColor;
			dueDateLabel.TextAlignment = UITextAlignment.Left;
			dueDateLabel.BackgroundColor = Colors.Clear;
			dueDateLabel.Lines = 0;
			dueDateLabel.SizeToFit ();
			dueDateLabel.AdjustsFontSizeToFitWidth = true;
			dueDateLabel.UserInteractionEnabled = false;
			AddSubview (dueDateLabel);

			if (dueDateTitleLabel == null)
				dueDateTitleLabel = new UILabel ();
			dueDateTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			dueDateTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BLACK,(nfloat)Constants.SMALL_FONT_SIZE);
			dueDateTitleLabel.TextAlignment = UITextAlignment.Left;
			dueDateTitleLabel.TextColor = titleColor;
			dueDateTitleLabel.BackgroundColor = Colors.Clear;
			dueDateTitleLabel.Lines = 0;
			dueDateTitleLabel.Text="dueDate".tr();
			dueDateTitleLabel.SizeToFit ();
			dueDateTitleLabel.AdjustsFontSizeToFitWidth = true;
			dueDateTitleLabel.UserInteractionEnabled = false;
			AddSubview (dueDateTitleLabel);

			yPos += (float)dueDateTitleLabel.Frame.Height + bottomPadding;
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
				formNumTitleLabel.TextColor = titleColor = Colors.SINGLE_SELECTION_LABEL_HIGHLIGHT_COLOR;
				cellBackingView.BackgroundColor = backgroundColor = Colors.HIGHLIGHT_COLOR;
			} else {
				formNumLabel.TextColor = labelColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
				formNumTitleLabel.TextColor = titleColor = Colors.HIGHLIGHT_COLOR;
				cellBackingView.BackgroundColor = backgroundColor = Colors.BACKGROUND_COLOR;
			}

			submittedDateLabel.TextColor = formNumLabel.TextColor;
			dueDateLabel.TextColor = formNumLabel.TextColor;

			submittedDateTitleLabel.TextColor = formNumTitleLabel.TextColor;
			dueDateTitleLabel.TextColor = formNumTitleLabel.TextColor;
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
					var set = this.CreateBindingSet<AuditListTableViewCell, ClaimAudit>();
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

