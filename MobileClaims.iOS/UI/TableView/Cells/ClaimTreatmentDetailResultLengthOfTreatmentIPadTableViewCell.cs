 using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	[Register("ClaimTreatmentDetailResultLengthOfTreatmentIPadTableViewCell")]
	public class ClaimTreatmentDetailResultLengthOfTreatmentIPadTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel typeLabel;
		public UILabel lengthLabel;
		public UILabel dateLabel;
		public UILabel datePaymentLabel;
		public UILabel amountLabel;
		public UILabel orthFeeLabel;
		public UILabel altCarrierLabel;

		protected UIView cellBackingView;

		public ClaimTreatmentDetailResultLengthOfTreatmentIPadTableViewCell () : base () {}
		public ClaimTreatmentDetailResultLengthOfTreatmentIPadTableViewCell (IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float topPadding = 15;
			float fieldPadding = Constants.CLAIMS_RESULTS_IPAD_PADDING;
			float rowHeight = Constants.CLAIMS_RESULTS_IPAD_HEIGHT;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

			float sectionWidth = 150;

			float xPos = fieldPadding;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new RectangleF (0, 0, this.Frame.Width, this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (typeLabel == null)
				typeLabel = new UILabel ();
			typeLabel.Frame = new RectangleF (xPos, rowHeight/2 - typeLabel.Frame.Height/2, sectionWidth, typeLabel.Frame.Height);
			typeLabel.SizeToFit ();
			typeLabel.Frame = new RectangleF (xPos, rowHeight/2 - typeLabel.Frame.Height/2, sectionWidth, typeLabel.Frame.Height);
			typeLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			typeLabel.TextAlignment = UITextAlignment.Left;
			typeLabel.BackgroundColor = UIColor.Clear;
			typeLabel.TextColor = Constants.DARK_GREY_COLOR;
			typeLabel.Lines = 0;
			typeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (typeLabel);

			if(!typeLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			if (lengthLabel == null)
				lengthLabel = new UILabel ();
			lengthLabel.Frame = new RectangleF (xPos, rowHeight/2 - lengthLabel.Frame.Height/2, sectionWidth, lengthLabel.Frame.Height);
			lengthLabel.SizeToFit ();
			lengthLabel.Frame = new RectangleF (xPos, rowHeight/2 - lengthLabel.Frame.Height/2, lengthLabel.Frame.Width, lengthLabel.Frame.Height);
			lengthLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			lengthLabel.TextAlignment = UITextAlignment.Left;
			lengthLabel.BackgroundColor = UIColor.Clear;
			lengthLabel.TextColor = Constants.DARK_GREY_COLOR;
			lengthLabel.Lines = 0;
			lengthLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (lengthLabel);

			if(!lengthLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			if (dateLabel == null)
				dateLabel = new UILabel ();
			dateLabel.Frame = new RectangleF (xPos, rowHeight/2 - dateLabel.Frame.Height/2, sectionWidth, dateLabel.Frame.Height);
			dateLabel.SizeToFit ();
			dateLabel.Frame = new RectangleF (xPos, rowHeight/2 - dateLabel.Frame.Height/2, sectionWidth, dateLabel.Frame.Height);
			dateLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			dateLabel.TextAlignment = UITextAlignment.Left;
			dateLabel.BackgroundColor = UIColor.Clear;
			dateLabel.TextColor = Constants.DARK_GREY_COLOR;
			dateLabel.Lines = 0;
			dateLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateLabel);

			if(!dateLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			if (datePaymentLabel == null)
				datePaymentLabel = new UILabel ();
			datePaymentLabel.Frame = new RectangleF (xPos, rowHeight/2 - dateLabel.Frame.Height/2, sectionWidth, dateLabel.Frame.Height);
			datePaymentLabel.SizeToFit ();
			datePaymentLabel.Frame = new RectangleF (xPos, rowHeight/2 - dateLabel.Frame.Height/2, sectionWidth, dateLabel.Frame.Height);
			datePaymentLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			datePaymentLabel.TextAlignment = UITextAlignment.Left;
			datePaymentLabel.BackgroundColor = UIColor.Clear;
			datePaymentLabel.TextColor = Constants.DARK_GREY_COLOR;
			datePaymentLabel.Lines = 0;
			datePaymentLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (datePaymentLabel);

			if(!datePaymentLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			if (amountLabel == null)
				amountLabel = new UILabel ();
			amountLabel.Frame = new RectangleF (xPos, rowHeight/2 - amountLabel.Frame.Height/2, sectionWidth, amountLabel.Frame.Height);
			amountLabel.SizeToFit ();
			amountLabel.Frame = new RectangleF (xPos, rowHeight/2 - amountLabel.Frame.Height/2, sectionWidth, amountLabel.Frame.Height);
			amountLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			amountLabel.TextAlignment = UITextAlignment.Left;
			amountLabel.BackgroundColor = UIColor.Clear;
			amountLabel.TextColor = Constants.DARK_GREY_COLOR;
			amountLabel.Lines = 0;
			amountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (amountLabel);

			if(!amountLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			if (orthFeeLabel == null)
				orthFeeLabel = new UILabel ();
			orthFeeLabel.Frame = new RectangleF (xPos, rowHeight/2 - orthFeeLabel.Frame.Height/2, sectionWidth, orthFeeLabel.Frame.Height);
			orthFeeLabel.SizeToFit ();
			orthFeeLabel.Frame = new RectangleF (xPos, rowHeight/2 - orthFeeLabel.Frame.Height/2, sectionWidth, orthFeeLabel.Frame.Height);
			orthFeeLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			orthFeeLabel.TextAlignment = UITextAlignment.Left;
			orthFeeLabel.BackgroundColor = UIColor.Clear;
			orthFeeLabel.TextColor = Constants.DARK_GREY_COLOR;
			orthFeeLabel.Lines = 0;
			orthFeeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (orthFeeLabel);

			if(!orthFeeLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;

			if (altCarrierLabel == null)
				altCarrierLabel = new UILabel ();
			altCarrierLabel.Frame = new RectangleF (xPos, rowHeight/2 - altCarrierLabel.Frame.Height/2, sectionWidth, altCarrierLabel.Frame.Height );
			altCarrierLabel.SizeToFit ();
			altCarrierLabel.Frame = new RectangleF (xPos, rowHeight/2 - altCarrierLabel.Frame.Height/2, sectionWidth, altCarrierLabel.Frame.Height );
			altCarrierLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			altCarrierLabel.TextAlignment = UITextAlignment.Left;
			altCarrierLabel.BackgroundColor = UIColor.Clear;
			altCarrierLabel.TextColor = Constants.DARK_GREY_COLOR;
			altCarrierLabel.Lines = 0;
			altCarrierLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altCarrierLabel);

			if(!altCarrierLabel.Hidden)	
				xPos += sectionWidth + fieldPadding;
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

		}

		public override bool ShowsDeleteButton ()
		{
			return false;
		}

		public virtual bool PersistsSelection()
		{
			return true;
		}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimTreatmentDetailResultLengthOfTreatmentIPadTableViewCell,TreatmentDetail>();
					set.Bind(this.typeLabel).To(item => item.TypeOfTreatmentListViewItem.Name);
					set.Bind(typeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfTreatmentVisible).WithConversion("BoolOpposite");

					set.Bind(this.lengthLabel).To(item => item.TreatmentDurationListViewItem.Name);
					set.Bind(lengthLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentDurationVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateLabel).To(item => item.TreatmentDate).WithConversion("DateToString");
					set.Bind(dateLabel).For(v => v.Hidden).To(vm => vm.IsTreatementDateVisible).WithConversion("BoolOpposite");

					set.Bind(this.datePaymentLabel).To(item => item.DateOfMonthlyTreatment).WithConversion("DateToString");
					set.Bind(datePaymentLabel).For(v => v.Hidden).To(vm => vm.IsDateOfMonthlyTreatmentVisible).WithConversion("BoolOpposite");

					set.Bind(this.amountLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");
					set.Bind(amountLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentAmountVisible).WithConversion("BoolOpposite");

					set.Bind(this.orthFeeLabel).To(item => item.OrthodonticMonthlyFee).WithConversion("DollarSignDoublePrefix");
					set.Bind(orthFeeLabel).For(v => v.Hidden).To(vm => vm.IsOrthodonticMonthlyFeeVisible).WithConversion("BoolOpposite");

					set.Bind(this.altCarrierLabel).To(item => item.AlternateCarrierPayment).WithConversion("DollarSignDoublePrefix");
					set.Bind(altCarrierLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");

					set.Bind (this).For (v => v.TreatmentDetailItem).To (vm => vm.TypeOfTreatment);


					set.Apply();


				});
		}

		private ClaimSubmissionBenefit _treatmentDetailItem;
		public ClaimSubmissionBenefit TreatmentDetailItem
		{
			get
			{
				return _treatmentDetailItem;
			}
			set
			{
				_treatmentDetailItem = value;


			}
		}


	}
}
