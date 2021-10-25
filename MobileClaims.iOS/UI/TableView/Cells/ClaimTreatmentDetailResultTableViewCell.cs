 using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	[Register("ClaimTreatmentDetailResultTableViewCell")]
	public class ClaimTreatmentDetailResultTableViewCell : MvxDeleteTableViewCell
	{
		public UILabel typeTitleLabel;
		public UILabel dateTitleLabel;
		public UILabel amountTitleLabel;
		public UILabel altCarrierTitleLabel;

		public UILabel typeLabel;
		public UILabel dateLabel;
		public UILabel amountLabel;
		public UILabel altCarrierLabel;

		protected UIView cellBackingView;

		public ClaimTreatmentDetailResultTableViewCell () : base () {}
		public ClaimTreatmentDetailResultTableViewCell (IntPtr handle) : base (handle) {}

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
			typeLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
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
			typeTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			typeTitleLabel.TextAlignment = UITextAlignment.Left;
			typeTitleLabel.BackgroundColor = UIColor.Clear;
			typeTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			typeTitleLabel.Lines = 0;
			typeTitleLabel.Text="typeOfTreatmentTitle".tr() + "colon".tr();
			typeTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (typeTitleLabel);

			yPos += typeLabel.Frame.Height + fieldPadding;

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

			yPos += dateLabel.Frame.Height + fieldPadding;

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

			yPos += dateLabel.Frame.Height + fieldPadding;

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

			yPos += altCarrierLabel.Frame.Height + fieldPadding;
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
					var set = this.CreateBindingSet<ClaimTreatmentDetailResultTableViewCell,TreatmentDetail>();
					set.Bind(this.typeLabel).To(item => item.TypeOfTreatmentListViewItem.Name).OneWay();
					set.Bind(this.dateLabel).To(item => item.TreatmentDateListViewItem).WithConversion("DateToString");
					set.Bind(this.amountLabel).To(item => item.TreatmentAmountListViewItem).WithConversion("DollarSignDoublePrefix");
					set.Bind(this.altCarrierLabel).To(item => item.AlternateCarrierPayment).WithConversion("DollarSignDoublePrefix");
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
