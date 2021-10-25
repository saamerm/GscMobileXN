using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MobileClaims.Core.Entities;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace MobileClaims.iOS
{
	[Register("ClaimLimitationsEyeExamTableCell")]
	public class ClaimLimitationsEyeExamTableCell : MvxTableViewCell
	{
		public UILabel benefitDescriptionLabel;
		public UILabel participantFamilyTitleLabel;
		public UILabel startDateTitleLabel;
		public UILabel amountTitleLabel;
		public UILabel occurencesTitleLabel;

		public UILabel limitationDescriptionLabel;
		public UILabel participantFamilyLabel;
		public UILabel startDateLabel;
		public UILabel amountLabel;
		public UILabel occurencesLabel;

		protected UIView cellBackingView;

		bool amountVisible;
		bool occurencesVisible;

		public ClaimLimitationsEyeExamTableCell () : base () {
			CreateLayout();
			InitializeBindings();
		}
		public ClaimLimitationsEyeExamTableCell (IntPtr handle) : base (handle) {
			CreateLayout();
			InitializeBindings();
		}

		public override void LayoutSubviews ()
		{
			CreateLayout ();
			base.LayoutSubviews ();
		}

		public void CreateLayout()
		{
			this.SelectionStyle = UITableViewCellSelectionStyle.None;

			float fieldPadding = 20;
			float innerPadding = 10;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = this.Frame.Width;
			float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth/2 + innerPadding;
			float resultWidth = contentWidth / 2 - Constants.DRUG_LOOKUP_SIDE_PADDING - innerPadding ;
			float topPadding = 15;
			float yPos = 0;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
			cellBackingView.Frame = new RectangleF (0, 0, this.Frame.Width, this.Frame.Height);
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
			BackgroundView = cellBackingView;

			if (limitationDescriptionLabel == null)
				limitationDescriptionLabel = new UILabel ();
			limitationDescriptionLabel.Frame = new RectangleF (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			limitationDescriptionLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			limitationDescriptionLabel.TextColor = Constants.DARK_GREY_COLOR;
			limitationDescriptionLabel.TextAlignment = UITextAlignment.Left;
			limitationDescriptionLabel.BackgroundColor = UIColor.Clear;
			limitationDescriptionLabel.Lines = 2;
			limitationDescriptionLabel.SizeToFit ();
			limitationDescriptionLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (limitationDescriptionLabel);

			if (benefitDescriptionLabel == null)
				benefitDescriptionLabel = new UILabel ();
			benefitDescriptionLabel.Frame = new RectangleF (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			benefitDescriptionLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			benefitDescriptionLabel.TextAlignment = UITextAlignment.Left;
			benefitDescriptionLabel.TextColor = Constants.DARK_GREY_COLOR;
			benefitDescriptionLabel.BackgroundColor = UIColor.Clear;
			benefitDescriptionLabel.ClipsToBounds = false;
			benefitDescriptionLabel.Lines = 2;
			benefitDescriptionLabel.SizeToFit ();
			benefitDescriptionLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (benefitDescriptionLabel);

			yPos += benefitDescriptionLabel.Frame.Height + fieldPadding;

			if (participantFamilyLabel == null)
				participantFamilyLabel = new UILabel ();
			participantFamilyLabel.Frame = new RectangleF (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			participantFamilyLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			participantFamilyLabel.TextColor = Constants.DARK_GREY_COLOR;
			participantFamilyLabel.TextAlignment = UITextAlignment.Left;
			participantFamilyLabel.BackgroundColor = UIColor.Clear;
			participantFamilyLabel.Lines = 2;
			participantFamilyLabel.SizeToFit ();
			participantFamilyLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (participantFamilyLabel);

			if (participantFamilyTitleLabel == null)
				participantFamilyTitleLabel = new UILabel ();
			participantFamilyTitleLabel.Frame = new RectangleF (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			participantFamilyTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			participantFamilyTitleLabel.TextAlignment = UITextAlignment.Left;
			participantFamilyTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			participantFamilyTitleLabel.BackgroundColor = UIColor.Clear;
			participantFamilyTitleLabel.Lines = 2;
			participantFamilyTitleLabel.Text="particpantFamily".tr();
			participantFamilyTitleLabel.SizeToFit ();
			participantFamilyTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (participantFamilyTitleLabel);

			yPos += participantFamilyTitleLabel.Frame.Height + fieldPadding;

			if (startDateLabel == null)
				startDateLabel = new UILabel ();
			startDateLabel.Frame = new RectangleF (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			startDateLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			startDateLabel.TextColor = Constants.DARK_GREY_COLOR;
			startDateLabel.TextAlignment = UITextAlignment.Left;
			startDateLabel.BackgroundColor = UIColor.Clear;
			startDateLabel.Lines = 2;
			startDateLabel.SizeToFit ();
			startDateLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (startDateLabel);

			if (startDateTitleLabel == null)
				startDateTitleLabel = new UILabel ();
			startDateTitleLabel.Frame = new RectangleF (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			startDateTitleLabel.Font = UIFont.FromName(Constants.AVENIR_STD_BOLD,Constants.SMALL_FONT_SIZE);
			startDateTitleLabel.TextAlignment = UITextAlignment.Left;
			startDateTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
			startDateTitleLabel.BackgroundColor = UIColor.Clear;
			startDateTitleLabel.Lines = 2;
			startDateTitleLabel.Text="startDate".tr();
			startDateTitleLabel.SizeToFit ();
			startDateTitleLabel.AdjustsFontSizeToFitWidth = true;
			AddSubview (startDateTitleLabel);

			yPos += startDateTitleLabel.Frame.Height + fieldPadding;

			if (amountVisible) {
				if (amountLabel == null) {
					amountLabel = new UILabel ();
					amountLabel.Text = "$" + AccumAmountUsed.ToString();
				}
				amountLabel.Frame = new RectangleF (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				amountLabel.Font = UIFont.FromName (Constants.AVENIR_STD_BOLD, Constants.SMALL_FONT_SIZE);
				amountLabel.TextAlignment = UITextAlignment.Left;
				amountLabel.TextColor = Constants.DARK_GREY_COLOR;
				amountLabel.BackgroundColor = UIColor.Clear;
				amountLabel.Lines = 2;
				amountLabel.SizeToFit ();
				amountLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview (amountLabel);

				if (amountTitleLabel == null)
					amountTitleLabel = new UILabel ();
				amountTitleLabel.Frame = new RectangleF (sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				amountTitleLabel.Font = UIFont.FromName (Constants.AVENIR_STD_BOLD, Constants.SMALL_FONT_SIZE);
				amountTitleLabel.TextAlignment = UITextAlignment.Left;
				amountTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
				amountTitleLabel.BackgroundColor = UIColor.Clear;
				amountTitleLabel.Lines = 2;
				amountTitleLabel.Text = "amountToDate".tr ();
				amountTitleLabel.SizeToFit ();
				amountTitleLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview (amountTitleLabel);

				amountLabel.Hidden = false;
				amountTitleLabel.Hidden = false;


				yPos += amountTitleLabel.Frame.Height + fieldPadding;
			} else {

				if (amountTitleLabel != null) {
					amountTitleLabel.Hidden = true;
				}

				if (amountLabel != null) {
					amountLabel.Hidden = true;
				}

			}

			if (occurencesVisible) {
				if (occurencesLabel == null) {
					occurencesLabel = new UILabel ();
					occurencesLabel.Text = AccumUnitsUsed.ToString();
				}
				occurencesLabel.Frame = new RectangleF (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				occurencesLabel.Font = UIFont.FromName (Constants.AVENIR_STD_BOLD, Constants.SMALL_FONT_SIZE);
				occurencesLabel.TextAlignment = UITextAlignment.Left;
				occurencesLabel.TextColor = Constants.DARK_GREY_COLOR;
				occurencesLabel.BackgroundColor = UIColor.Clear;
				occurencesLabel.Lines = 2;
				occurencesLabel.SizeToFit ();
				occurencesLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview (occurencesLabel);

				if (occurencesTitleLabel == null)
					occurencesTitleLabel = new UILabel ();
				occurencesTitleLabel.Frame = new RectangleF (sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				occurencesTitleLabel.Font = UIFont.FromName (Constants.AVENIR_STD_BOLD, Constants.SMALL_FONT_SIZE);
				occurencesTitleLabel.TextAlignment = UITextAlignment.Left;
				occurencesTitleLabel.TextColor = Constants.DARK_GREY_COLOR;
				occurencesTitleLabel.BackgroundColor = UIColor.Clear;
				occurencesTitleLabel.Lines = 2;
				occurencesTitleLabel.Text = "occurencesToDate".tr ();
				occurencesTitleLabel.SizeToFit ();
				occurencesTitleLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview (occurencesTitleLabel);

				occurencesLabel.Hidden = false;
				occurencesTitleLabel.Hidden = false;

				yPos += occurencesTitleLabel.Frame.Height + fieldPadding;

			} else {

				if (occurencesTitleLabel != null) {
					occurencesTitleLabel.Hidden = true;
				}

				if (occurencesLabel != null) {
					occurencesLabel.Hidden = true;
				}

			}

		}

		public virtual bool ShowsDeleteButton()
		{
			return false;
		}


		public virtual void InitializeBindings ()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ClaimLimitationsEyeExamTableCell, ClaimPlanLimitationGSC>();
					set.Bind(this.benefitDescriptionLabel).To(item => item.BenefitDescription);
					set.Bind(this.limitationDescriptionLabel).To(item => item.LimitationDescription);
					set.Bind(this.participantFamilyLabel).To(item => item.AppliesTo).OneWay();
					set.Bind(this.startDateLabel).To(item => item.AccumStartDate).WithConversion("DateToString");
					set.Bind (this).For (v => v.AccumAmountUsed).To (vm => vm.AccumAmountUsed);
					set.Bind (this).For (v => v.AccumUnitsUsed).To (vm => vm.AccumUnitsUsed);

					set.Apply();

				});
		}

		private double _accumUnitsUsed;
		public double AccumUnitsUsed {
			get {
				return _accumUnitsUsed;
			}
			set {

				_accumUnitsUsed = value;

				setTotal ();
			}

		}

		private double _accumAmountUsed;
		public double AccumAmountUsed {
			get {
				return _accumAmountUsed;
			}
			set {

				_accumAmountUsed = value;

				setTotal ();
			}

		}

		public void setTotal()
		{

			if (AccumAmountUsed > 0 && AccumUnitsUsed <= 0) {

				amountVisible = true;
				occurencesVisible = false;

			} else if (AccumUnitsUsed > 0 && AccumAmountUsed <= 0) {

				occurencesVisible = true;
				amountVisible = false;

			} else {
				amountVisible = occurencesVisible = true;
			}

			amountLabel.Text = "$" + (AccumAmountUsed).ToString();
			occurencesLabel.Text = AccumUnitsUsed.ToString();
		}

	}
}


