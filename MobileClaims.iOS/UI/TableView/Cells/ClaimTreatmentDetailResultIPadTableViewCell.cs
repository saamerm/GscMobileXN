 using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MobileClaims.Core.Entities;

namespace MobileClaims.iOS
{
	[Register("ClaimTreatmentDetailResultIPadTableViewCell")]
	public class ClaimTreatmentDetailResultIPadTableViewCell : MvxDeleteTableViewCell
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

		public ClaimTreatmentDetailResultIPadTableViewCell () : base () {}
		public ClaimTreatmentDetailResultIPadTableViewCell (IntPtr handle) : base (handle) {}

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
			float yPos = topPadding;
			float rowHeight = Constants.CLAIMS_RESULTS_IPAD_HEIGHT;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;

			float sectionWidth = (Frame.Width -fieldPadding * 5) / 4 ;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new RectangleF (0, 0, this.Frame.Width, this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (typeLabel == null)
				typeLabel = new UILabel ();
			typeLabel.Frame = new RectangleF (fieldPadding, rowHeight/2 - typeLabel.Frame.Height/2, sectionWidth, typeLabel.Frame.Height);
			typeLabel.SizeToFit ();
			typeLabel.Frame = new RectangleF (fieldPadding, rowHeight/2 - typeLabel.Frame.Height/2, sectionWidth, typeLabel.Frame.Height);
			typeLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			typeLabel.TextAlignment = UITextAlignment.Left;
			typeLabel.BackgroundColor = UIColor.Clear;
			typeLabel.TextColor = Constants.DARK_GREY_COLOR;
			typeLabel.Lines = 0;
			typeLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (typeLabel);

			if (dateLabel == null)
				dateLabel = new UILabel ();
			dateLabel.Frame = new RectangleF (fieldPadding*2 + sectionWidth, rowHeight/2 - dateLabel.Frame.Height/2, sectionWidth, dateLabel.Frame.Height);
			dateLabel.SizeToFit ();
			dateLabel.Frame = new RectangleF (fieldPadding*2 + sectionWidth, rowHeight/2 - dateLabel.Frame.Height/2, sectionWidth, dateLabel.Frame.Height);
			dateLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			dateLabel.TextAlignment = UITextAlignment.Left;
			dateLabel.BackgroundColor = UIColor.Clear;
			dateLabel.TextColor = Constants.DARK_GREY_COLOR;
			dateLabel.Lines = 0;
			dateLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (dateLabel);

			if (amountLabel == null)
				amountLabel = new UILabel ();
			amountLabel.Frame = new RectangleF (fieldPadding*3 + sectionWidth*2, rowHeight/2 - amountLabel.Frame.Height/2, sectionWidth, amountLabel.Frame.Height);
			amountLabel.SizeToFit ();
			amountLabel.Frame = new RectangleF (fieldPadding*3 + sectionWidth*2, rowHeight/2 - amountLabel.Frame.Height/2, sectionWidth, amountLabel.Frame.Height);
			amountLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			amountLabel.TextAlignment = UITextAlignment.Left;
			amountLabel.BackgroundColor = UIColor.Clear;
			amountLabel.TextColor = Constants.DARK_GREY_COLOR;
			amountLabel.Lines = 0;
			amountLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (amountLabel);

			if (altCarrierLabel == null)
				altCarrierLabel = new UILabel ();
			altCarrierLabel.Frame = new RectangleF ( fieldPadding*4 + sectionWidth*3, rowHeight/2 - altCarrierLabel.Frame.Height/2, sectionWidth, altCarrierLabel.Frame.Height );
			altCarrierLabel.SizeToFit ();
			altCarrierLabel.Frame = new RectangleF ( fieldPadding*4 + sectionWidth*3, rowHeight/2 - altCarrierLabel.Frame.Height/2, sectionWidth, altCarrierLabel.Frame.Height );
			altCarrierLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			altCarrierLabel.TextAlignment = UITextAlignment.Left;
			altCarrierLabel.BackgroundColor = UIColor.Clear;
			altCarrierLabel.TextColor = Constants.DARK_GREY_COLOR;
			altCarrierLabel.Lines = 0;
			altCarrierLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (altCarrierLabel);
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
					var set = this.CreateBindingSet<ClaimTreatmentDetailResultIPadTableViewCell,TreatmentDetail>();
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
