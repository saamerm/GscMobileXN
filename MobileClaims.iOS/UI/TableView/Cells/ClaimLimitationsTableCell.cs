using System;
using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
	[Foundation.Register("ClaimLimitationsTableCell")]
	public class ClaimLimitationsTableCell : MvxTableViewCell
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

		public ClaimLimitationsTableCell () : base () {
			CreateLayout();
			InitializeBindings();
		}
		public ClaimLimitationsTableCell (IntPtr handle) : base (handle) {
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
            
			float fieldPadding = 25;
			float innerPadding = 10;
			float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
			float contentWidth = (float)this.Frame.Width;
			float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth/2 + innerPadding;
			float resultWidth = contentWidth / 2 - Constants.DRUG_LOOKUP_SIDE_PADDING - innerPadding ;
			float topPadding = 15;
            float yPos = fieldPadding;// 0;

			if (cellBackingView == null)
				cellBackingView = new UIView ();
            cellBackingView.Frame = new CGRect (0, 0, (float)this.Frame.Width, (float)(this.Frame.Height));
			cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
			cellBackingView.ContentMode = UIViewContentMode.TopLeft;
            BackgroundView = cellBackingView;

			if (limitationDescriptionLabel == null)
				limitationDescriptionLabel = new UILabel ();
            limitationDescriptionLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			limitationDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			limitationDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
			limitationDescriptionLabel.TextAlignment = UITextAlignment.Left;
			limitationDescriptionLabel.BackgroundColor = Colors.Clear;
			limitationDescriptionLabel.Lines = 0;
			limitationDescriptionLabel.SizeToFit ();
			AddSubview (limitationDescriptionLabel);

			if (benefitDescriptionLabel == null)
				benefitDescriptionLabel = new UILabel ();
            benefitDescriptionLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			benefitDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			benefitDescriptionLabel.TextAlignment = UITextAlignment.Left;
			benefitDescriptionLabel.TextColor = Colors.DARK_GREY_COLOR;
			benefitDescriptionLabel.BackgroundColor = Colors.Clear;
			benefitDescriptionLabel.ClipsToBounds = false;
			benefitDescriptionLabel.Lines = 0;
			benefitDescriptionLabel.SizeToFit ();
			AddSubview (benefitDescriptionLabel);

			yPos += (Math.Max ((float)benefitDescriptionLabel.Frame.Height, (float)limitationDescriptionLabel.Frame.Height) + fieldPadding);

			if (participantFamilyLabel == null)
				participantFamilyLabel = new UILabel ();
			participantFamilyLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			participantFamilyLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			participantFamilyLabel.TextColor = Colors.DARK_GREY_COLOR;
			participantFamilyLabel.TextAlignment = UITextAlignment.Left;
			participantFamilyLabel.BackgroundColor = Colors.Clear;
			participantFamilyLabel.Lines = 0;
			participantFamilyLabel.SizeToFit ();
			AddSubview (participantFamilyLabel);

			if (participantFamilyTitleLabel == null)
				participantFamilyTitleLabel = new UILabel ();
			participantFamilyTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			participantFamilyTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			participantFamilyTitleLabel.TextAlignment = UITextAlignment.Left;
			participantFamilyTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			participantFamilyTitleLabel.BackgroundColor = Colors.Clear;
			participantFamilyTitleLabel.Lines = 0;
			participantFamilyTitleLabel.Text="particpantFamily".tr();
			participantFamilyTitleLabel.SizeToFit ();
			AddSubview (participantFamilyTitleLabel);

			yPos += (float)participantFamilyTitleLabel.Frame.Height + fieldPadding;

			if (startDateLabel == null)
				startDateLabel = new UILabel ();
			startDateLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			startDateLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			startDateLabel.TextColor = Colors.DARK_GREY_COLOR;
			startDateLabel.TextAlignment = UITextAlignment.Left;
			startDateLabel.BackgroundColor = Colors.Clear;
			startDateLabel.Lines = 0;
			startDateLabel.SizeToFit ();
			AddSubview (startDateLabel);

			if (startDateTitleLabel == null)
				startDateTitleLabel = new UILabel ();
			startDateTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth /2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
			startDateTitleLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD,(nfloat)Constants.SMALL_FONT_SIZE);
			startDateTitleLabel.TextAlignment = UITextAlignment.Left;
			startDateTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			startDateTitleLabel.BackgroundColor = Colors.Clear;
			startDateTitleLabel.Lines = 0;
			startDateTitleLabel.Text="startDate".tr();
			startDateTitleLabel.SizeToFit ();
			AddSubview (startDateTitleLabel);

			yPos += (float)startDateTitleLabel.Frame.Height + fieldPadding;

			if (amountVisible) {
				if (amountLabel == null) {
					amountLabel = new UILabel ();
					amountLabel.Text = "$" + AccumAmountUsed.ToString();
				}
				amountLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				amountLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
				amountLabel.TextAlignment = UITextAlignment.Left;
				amountLabel.TextColor = Colors.DARK_GREY_COLOR;
				amountLabel.BackgroundColor = Colors.Clear;
				amountLabel.Lines = 0;
				amountLabel.SizeToFit ();
				AddSubview (amountLabel);

				if (amountTitleLabel == null)
					amountTitleLabel = new UILabel ();
				amountTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				amountTitleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
				amountTitleLabel.TextAlignment = UITextAlignment.Left;
				amountTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
				amountTitleLabel.BackgroundColor = Colors.Clear;
				amountTitleLabel.Lines = 0;
				amountTitleLabel.Text = "amountToDate".tr ();
				amountTitleLabel.SizeToFit ();
				AddSubview (amountTitleLabel);

				amountLabel.Hidden = false;
				amountTitleLabel.Hidden = false;


				yPos += (float)amountTitleLabel.Frame.Height + fieldPadding;
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
				occurencesLabel.Frame = new CGRect (resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				occurencesLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
				occurencesLabel.TextAlignment = UITextAlignment.Left;
				occurencesLabel.TextColor = Colors.DARK_GREY_COLOR;
				occurencesLabel.BackgroundColor = Colors.Clear;
				occurencesLabel.Lines = 2;
				occurencesLabel.SizeToFit ();
				occurencesLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview (occurencesLabel);

				if (occurencesTitleLabel == null)
					occurencesTitleLabel = new UILabel ();
				occurencesTitleLabel.Frame = new CGRect (sidePadding, yPos, contentWidth / 2, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
				occurencesTitleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
				occurencesTitleLabel.TextAlignment = UITextAlignment.Left;
				occurencesTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
				occurencesTitleLabel.BackgroundColor = Colors.Clear;
				occurencesTitleLabel.Lines = 2;
				occurencesTitleLabel.Text = "occurencesToDate".tr ();
				occurencesTitleLabel.SizeToFit ();
				occurencesTitleLabel.AdjustsFontSizeToFitWidth = true;
				AddSubview (occurencesTitleLabel);

				occurencesLabel.Hidden = false;
				occurencesTitleLabel.Hidden = false;

				yPos += (float)occurencesTitleLabel.Frame.Height + fieldPadding;

			} else {

				if (occurencesTitleLabel != null) {
					occurencesTitleLabel.Hidden = true;
				}

				if (occurencesLabel != null) {
					occurencesLabel.Hidden = true;
				}
                //this.Bounds = new CGRect(0, 0,  this.Bounds.Width, this.Bounds.Height + (fieldPadding * 1));

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
					var set = this.CreateBindingSet<ClaimLimitationsTableCell, ClaimPlanLimitationGSC>();
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


