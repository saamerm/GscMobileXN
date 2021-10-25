using System;
using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimTreatmentDetailListTableViewCell")]
	public class ClaimTreatmentDetailListTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel dateLabel;
		public UILabel treatmentDurationLabel;
		public UILabel treatmentLabel;
		public UILabel altPaymentLabel;
		public UILabel paymentLabel;

		protected UIView cellBackingView;

		public ClaimTreatmentDetailListTableViewCell () : base () {}
		public ClaimTreatmentDetailListTableViewCell (IntPtr handle) : base (handle) {}

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

			if (paymentLabel == null)
				paymentLabel = new UILabel ();
			paymentLabel.Frame = new CGRect ( (float)this.Frame.Width - Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING - (float)paymentLabel.Frame.Width, (float)this.Frame.Height/2 - Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE /2 , (float)paymentLabel.Frame.Width, (float)paymentLabel.Frame.Height);
			paymentLabel.SizeToFit ();
			paymentLabel.Frame = new CGRect ( (float)this.Frame.Width - Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING - (float)paymentLabel.Frame.Width, (float)this.Frame.Height/2 - Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE /2 , (float)paymentLabel.Frame.Width, (float)paymentLabel.Frame.Height);
			paymentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			paymentLabel.TextAlignment = UITextAlignment.Left;
			paymentLabel.BackgroundColor = Colors.Clear;
			paymentLabel.Lines = 1;
			paymentLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (paymentLabel);

			if (dateLabel == null)
				dateLabel = new UILabel ();
			dateLabel.Frame = new CGRect (Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING, Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_HEIGHT/2-Constants.SINGLE_SELECTION_ACCENTED_TITLE_CELL_CONTENTS_HEIGHT/2, Frame.Width-Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING - (float)paymentLabel.Frame.Width, (float)dateLabel.Frame.Height);
			dateLabel.SizeToFit ();
			dateLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			dateLabel.BackgroundColor = Colors.Clear;
			dateLabel.Lines = 0;
			dateLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateLabel);

			if (treatmentDurationLabel == null)
				treatmentDurationLabel = new UILabel ();
			treatmentDurationLabel.Frame = new CGRect (Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING, (float)dateLabel.Frame.Y+(float)dateLabel.Frame.Height+Constants.SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN, Frame.Width-Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING - (float)paymentLabel.Frame.Width, (float)treatmentDurationLabel.Frame.Height);
			treatmentDurationLabel.SizeToFit ();
			treatmentDurationLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			treatmentDurationLabel.BackgroundColor = Colors.Clear;
			treatmentDurationLabel.Lines = 0;
			treatmentDurationLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (treatmentDurationLabel);

			if (treatmentLabel == null)
				treatmentLabel = new UILabel ();
			treatmentLabel.Frame = new CGRect (Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING, (float)treatmentDurationLabel.Frame.Y+(float)treatmentDurationLabel.Frame.Height+Constants.SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN, Frame.Width-Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING - (float)paymentLabel.Frame.Width, (float)treatmentLabel.Frame.Height);
			treatmentLabel.SizeToFit ();
			treatmentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			treatmentLabel.BackgroundColor = Colors.Clear;
			treatmentLabel.Lines = 0;
			treatmentLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (treatmentLabel);

			if (altPaymentLabel == null)
				altPaymentLabel = new UILabel ();
			altPaymentLabel.Frame = new CGRect (Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING, (float)treatmentLabel.Frame.Y+(float)treatmentLabel.Frame.Height+Constants.SINGLE_SELECTION_ACCENTED_TITLE_INNER_MARGIN, Frame.Width-Constants.TREATMENT_DETAILS_LIST_ITEM_PADDING - (float)paymentLabel.Frame.Width, (float)altPaymentLabel.Frame.Height);
			altPaymentLabel.SizeToFit ();
			altPaymentLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SINGLE_SELECTION_ACCENTED_TITLE_SMALL_LABEL_FONT_SIZE);
			altPaymentLabel.BackgroundColor = Colors.Clear;
			altPaymentLabel.Lines = 0;
			altPaymentLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altPaymentLabel);

		}

		public override void SetSelected (bool selected, bool animated)
		{
			SetHighlightColors (selected);
			base.SetSelected (selected, animated);
		}

		public override void SetHighlighted (bool highlighted, bool animated)
		{
			if (!PersistsSelection())
				SetHighlightColors (highlighted);
			base.SetHighlighted (highlighted, animated);
		}

		private void SetHighlightColors(bool selected)
		{
			if (selected) {
				treatmentLabel.TextColor = Colors.SINGLE_SELECTION_LABEL_HIGHLIGHT_COLOR;
				cellBackingView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
			} else {
				treatmentLabel.TextColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
				cellBackingView.BackgroundColor = Colors.LightGrayColor;
			}

			treatmentDurationLabel.TextColor = treatmentLabel.TextColor;
			altPaymentLabel.TextColor = treatmentLabel.TextColor;
			paymentLabel.TextColor = treatmentLabel.TextColor;
			dateLabel.TextColor = treatmentLabel.TextColor;
		}

		public override bool ShowsDeleteButton ()
		{
			return true;
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
					var set = this.CreateBindingSet<ClaimTreatmentDetailListTableViewCell,TreatmentDetail>();

                    set.Bind(dateLabel).To(item => item.TreatmentDateListViewItem).WithConversion("DateToString");
					set.Bind(treatmentDurationLabel).To(item => item.TreatmentDurationListViewItem.Name ).OneWay();
					set.Bind(treatmentLabel).To(item => item.TypeOfTreatmentListViewItem.Name ).OneWay();
					set.Bind(altPaymentLabel).To(item => item.AlternateCarrierPayment).WithConversion("AltCarrierPrefix");
					set.Bind(altPaymentLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");
					set.Bind(paymentLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");

                    set.Apply();

					this.SetNeedsLayout();
				});
		}

	}
}

