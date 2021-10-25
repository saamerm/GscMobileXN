 using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	[Register("ClaimTreatmentDetailResultLengthOfTreatmentTableViewCell")]
	public class ClaimTreatmentDetailResultLengthOfTreatmentTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel typeTitleLabel;
		public UILabel lengthTitleLabel;
		public UILabel dateTitleLabel;
		public UILabel datePaymentTitleLabel;
		public UILabel amountTitleLabel;
		public UILabel orthFeeTitleLabel;
		public UILabel altCarrierTitleLabel;

		public UILabel typeLabel;
		public UILabel lengthLabel;
		public UILabel dateLabel;
		public UILabel datePaymentLabel;
		public UILabel amountLabel;
		public UILabel orthFeeLabel;
		public UILabel altCarrierLabel;

		protected UIView cellBackingView;

		public ClaimTreatmentDetailResultLengthOfTreatmentTableViewCell () : base () {}
		public ClaimTreatmentDetailResultLengthOfTreatmentTableViewCell (IntPtr handle) : base (handle) {}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public override void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float topPadding = 15;
			float fieldPadding = 10;
			float yPos = topPadding;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new RectangleF (0, 0, this.Frame.Width, this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (typeLabel == null)
				typeLabel = new UILabel ();
			typeLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, typeLabel.Frame.Height);
			typeLabel.SizeToFit ();
			typeLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, typeLabel.Frame.Height);
			typeLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			typeLabel.TextAlignment = UITextAlignment.Right;
			typeLabel.BackgroundColor = UIColor.Clear;
			typeLabel.TextColor = Constants.DARK_GREY_COLOR;
			typeLabel.Lines = 0;
			typeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (typeLabel);

			if (typeTitleLabel == null)
				typeTitleLabel = new UILabel ();
			typeTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, typeTitleLabel.Frame.Height);
			typeTitleLabel.SizeToFit ();
			typeTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, typeTitleLabel.Frame.Height);
			typeTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			typeTitleLabel.TextAlignment = UITextAlignment.Left;
			typeTitleLabel.BackgroundColor = UIColor.Clear;
			typeTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			typeTitleLabel.Lines = 0;
			typeTitleLabel.Text="typeOfTreatmentTitle".tr() + "colon".tr();
			typeTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (typeTitleLabel);

			if(!typeLabel.Hidden)	
				yPos += typeLabel.Frame.Height + fieldPadding;

			if (lengthLabel == null)
				lengthLabel = new UILabel ();
			lengthLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, lengthLabel.Frame.Height);
			lengthLabel.SizeToFit ();
			lengthLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, lengthLabel.Frame.Height);
			lengthLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			lengthLabel.TextAlignment = UITextAlignment.Right;
			lengthLabel.BackgroundColor = UIColor.Clear;
			lengthLabel.TextColor = Constants.DARK_GREY_COLOR;
			lengthLabel.Lines = 0;
			lengthLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (lengthLabel);

			if (lengthTitleLabel == null)
				lengthTitleLabel = new UILabel ();
			lengthTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, lengthTitleLabel.Frame.Height);
			lengthTitleLabel.SizeToFit ();
			lengthTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, lengthTitleLabel.Frame.Height);
			lengthTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			lengthTitleLabel.TextAlignment = UITextAlignment.Left;
			lengthTitleLabel.BackgroundColor = UIColor.Clear;
			lengthTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			lengthTitleLabel.Lines = 0;
			lengthTitleLabel.Text="lengthOfTreatmentTitle".tr() + "colon".tr();
			lengthTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (lengthTitleLabel);

			if(!lengthLabel.Hidden)
				yPos += lengthLabel.Frame.Height + fieldPadding;
				
			if (dateLabel == null)
				dateLabel = new UILabel ();
			dateLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, dateLabel.Frame.Height);
			dateLabel.SizeToFit ();
			dateLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, dateLabel.Frame.Height);
			dateLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			dateLabel.TextAlignment = UITextAlignment.Right;
			dateLabel.BackgroundColor = UIColor.Clear;
			dateLabel.TextColor = Constants.DARK_GREY_COLOR;
			dateLabel.Lines = 0;
			dateLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateLabel);

			if (dateTitleLabel == null)
				dateTitleLabel = new UILabel ();
			dateTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, dateTitleLabel.Frame.Height);
			dateTitleLabel.SizeToFit ();
			dateTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, dateTitleLabel.Frame.Height);
			dateTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			dateTitleLabel.TextAlignment = UITextAlignment.Left;
			dateTitleLabel.BackgroundColor = UIColor.Clear;
			dateTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			dateTitleLabel.Lines = 0;
			dateTitleLabel.Text="dateOfTreatmentTitle".tr() + "colon".tr();
			dateTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateTitleLabel);

			if(!dateLabel.Hidden)
				yPos += dateLabel.Frame.Height + fieldPadding;

			if (datePaymentLabel == null)
				datePaymentLabel = new UILabel ();
			datePaymentLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, dateLabel.Frame.Height);
			datePaymentLabel.SizeToFit ();
			datePaymentLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, dateLabel.Frame.Height);
			datePaymentLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			datePaymentLabel.TextAlignment = UITextAlignment.Right;
			datePaymentLabel.BackgroundColor = UIColor.Clear;
			datePaymentLabel.TextColor = Constants.DARK_GREY_COLOR;
			datePaymentLabel.Lines = 0;
			datePaymentLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (datePaymentLabel);

			if (datePaymentTitleLabel == null)
				datePaymentTitleLabel = new UILabel ();
			datePaymentTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, dateTitleLabel.Frame.Height);
			datePaymentTitleLabel.SizeToFit ();
			datePaymentTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, dateTitleLabel.Frame.Height);
			datePaymentTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			datePaymentTitleLabel.TextAlignment = UITextAlignment.Left;
			datePaymentTitleLabel.BackgroundColor = UIColor.Clear;
			datePaymentTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			datePaymentTitleLabel.Lines = 0;
			datePaymentTitleLabel.Text="dateOfMonthlyPaymentTitle".tr() + "colon".tr();
			datePaymentTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (datePaymentTitleLabel);

			if(!datePaymentLabel.Hidden)
				yPos += datePaymentLabel.Frame.Height + fieldPadding;

			if (amountLabel == null)
				amountLabel = new UILabel ();
			amountLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, amountLabel.Frame.Height);
			amountLabel.SizeToFit ();
			amountLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, amountLabel.Frame.Height);
			amountLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			amountLabel.TextAlignment = UITextAlignment.Right;
			amountLabel.BackgroundColor = UIColor.Clear;
			amountLabel.TextColor = Constants.DARK_GREY_COLOR;
			amountLabel.Lines = 0;
			amountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (amountLabel);

			if (amountTitleLabel == null)
				amountTitleLabel = new UILabel ();
			amountTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, amountTitleLabel.Frame.Height);
			amountTitleLabel.SizeToFit ();
			amountTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, amountTitleLabel.Frame.Height);
			amountTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			amountTitleLabel.TextAlignment = UITextAlignment.Left;
			amountTitleLabel.BackgroundColor = UIColor.Clear;
			amountTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			amountTitleLabel.Lines = 0;
			amountTitleLabel.Text="totalAmountOfVisit".tr() + "colon".tr();
			amountTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (amountTitleLabel);

			if(!amountLabel.Hidden)
				yPos += amountLabel.Frame.Height + fieldPadding;





			if (orthFeeLabel == null)
				orthFeeLabel = new UILabel ();
			orthFeeLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, orthFeeLabel.Frame.Height);
			orthFeeLabel.SizeToFit ();
			orthFeeLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding* 2, orthFeeLabel.Frame.Height);
			orthFeeLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			orthFeeLabel.TextAlignment = UITextAlignment.Right;
			orthFeeLabel.BackgroundColor = UIColor.Clear;
			orthFeeLabel.TextColor = Constants.DARK_GREY_COLOR;
			orthFeeLabel.Lines = 0;
			orthFeeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (orthFeeLabel);

			if (orthFeeTitleLabel == null)
				orthFeeTitleLabel = new UILabel ();
			orthFeeTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, orthFeeTitleLabel.Frame.Height);
			orthFeeTitleLabel.SizeToFit ();
			orthFeeTitleLabel.Frame = new RectangleF (sidePadding, yPos, Frame.Width-sidePadding * 2, orthFeeTitleLabel.Frame.Height);
			orthFeeTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			orthFeeTitleLabel.TextAlignment = UITextAlignment.Left;
			orthFeeTitleLabel.BackgroundColor = UIColor.Clear;
			orthFeeTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			orthFeeTitleLabel.Lines = 0;
			orthFeeTitleLabel.Text="omf".tr() + "colon".tr();
			orthFeeTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (orthFeeTitleLabel);

			if(!orthFeeLabel.Hidden)
				yPos += orthFeeLabel.Frame.Height + fieldPadding;


			if (altCarrierLabel == null)
				altCarrierLabel = new UILabel ();
			altCarrierLabel.Frame = new RectangleF ( sidePadding, yPos, Frame.Width-sidePadding * 2, altCarrierLabel.Frame.Height );
			altCarrierLabel.SizeToFit ();
			altCarrierLabel.Frame = new RectangleF ( sidePadding, yPos, Frame.Width-sidePadding * 2, altCarrierLabel.Frame.Height );
			altCarrierLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			altCarrierLabel.TextAlignment = UITextAlignment.Right;
			altCarrierLabel.BackgroundColor = UIColor.Clear;
			altCarrierLabel.TextColor = Constants.DARK_GREY_COLOR;
			altCarrierLabel.Lines = 0;
			altCarrierLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altCarrierLabel);

			if (altCarrierTitleLabel == null)
				altCarrierTitleLabel = new UILabel ();
			altCarrierTitleLabel.Frame = new RectangleF ( sidePadding, yPos, Frame.Width-sidePadding * 2, altCarrierTitleLabel.Frame.Height );
			altCarrierTitleLabel.SizeToFit ();
			altCarrierTitleLabel.Frame = new RectangleF ( sidePadding, yPos, Frame.Width-sidePadding * 2, altCarrierTitleLabel.Frame.Height );
			altCarrierTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			altCarrierTitleLabel.TextAlignment = UITextAlignment.Left;
			altCarrierTitleLabel.BackgroundColor = UIColor.Clear;
			altCarrierTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			altCarrierTitleLabel.Lines = 0;
			altCarrierTitleLabel.Text="amountPaidAlt".tr() + "colon".tr();
			altCarrierTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altCarrierTitleLabel);

//			if (TreatmentDetailItem != null && !TreatmentDetailItem.IsAlternateCarrierPaymentVisible) {
//				altCarrierLabel.Alpha = 0;
//				altCarrierTitleLabel.Alpha = 0;
//			} else {
			if(!altCarrierLabel.Hidden)
				yPos += altCarrierLabel.Frame.Height + fieldPadding;

			//}

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
					var set = this.CreateBindingSet<ClaimTreatmentDetailResultLengthOfTreatmentTableViewCell,TreatmentDetail>();
					set.Bind(this.typeLabel).To(item => item.TypeOfTreatmentListViewItem.Name);
					set.Bind(typeLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfTreatmentVisible).WithConversion("BoolOpposite");
					set.Bind(typeTitleLabel).For(v => v.Hidden).To(vm => vm.IsTypeOfTreatmentVisible).WithConversion("BoolOpposite");

					set.Bind(this.lengthLabel).To(item => item.TreatmentDurationListViewItem.Name);
					set.Bind(lengthLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentDurationVisible).WithConversion("BoolOpposite");
					set.Bind(lengthTitleLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentDurationVisible).WithConversion("BoolOpposite");

					set.Bind(this.dateLabel).To(item => item.TreatmentDate).WithConversion("DateToString");
					set.Bind(dateLabel).For(v => v.Hidden).To(vm => vm.IsTreatementDateVisible).WithConversion("BoolOpposite");
					set.Bind(dateTitleLabel).For(v => v.Hidden).To(vm => vm.IsTreatementDateVisible).WithConversion("BoolOpposite");

					set.Bind(this.datePaymentLabel).To(item => item.DateOfMonthlyTreatment).WithConversion("DateToString");
					set.Bind(datePaymentLabel).For(v => v.Hidden).To(vm => vm.IsDateOfMonthlyTreatmentVisible).WithConversion("BoolOpposite");
					set.Bind(datePaymentTitleLabel).For(v => v.Hidden).To(vm => vm.IsDateOfMonthlyTreatmentVisible).WithConversion("BoolOpposite");

					set.Bind(this.amountLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");
					set.Bind(amountLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentAmountVisible).WithConversion("BoolOpposite");
					set.Bind(amountTitleLabel).For(v => v.Hidden).To(vm => vm.IsTreatmentAmountVisible).WithConversion("BoolOpposite");

					set.Bind(this.orthFeeLabel).To(item => item.OrthodonticMonthlyFee).WithConversion("DollarSignDoublePrefix");
					set.Bind(orthFeeLabel).For(v => v.Hidden).To(vm => vm.IsOrthodonticMonthlyFeeVisible).WithConversion("BoolOpposite");
					set.Bind(orthFeeTitleLabel).For(v => v.Hidden).To(vm => vm.IsOrthodonticMonthlyFeeVisible).WithConversion("BoolOpposite");

					set.Bind(this.altCarrierLabel).To(item => item.AlternateCarrierPayment).WithConversion("DollarSignDoublePrefix");
					set.Bind (this).For (v => v.TreatmentDetailItem).To (vm => vm);
					set.Bind(altCarrierLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");
					set.Bind(altCarrierTitleLabel).For(v => v.Hidden).To(vm => vm.IsAlternateCarrierPaymentVisible).WithConversion("BoolOpposite");

					set.Apply();


				});
		}

		private TreatmentDetail _treatmentDetailItem;
		public TreatmentDetail TreatmentDetailItem
		{
			get
			{
				return _treatmentDetailItem;
			}
			set
			{
				_treatmentDetailItem = value;

				SetNeedsLayout ();

//				if (!value.IsAlternateCarrierPaymentVisible) {
//					this.Hidden = true;
//				} else {
//					this.Hidden = false;
//					//altCarrierTitleLabel.Hidden = false;
//				}


			}
		}


	}
}
