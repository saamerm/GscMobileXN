using System;
using UIKit;
using CoreGraphics;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    [Foundation.Register("EligibilityLimitationsTableCell")]
    public class EligibilityLimitationsTableCell : MvxTableViewCell
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

        public EligibilityLimitationsTableCell() : base()
        {
            CreateLayout();
            InitializeBindings();
        }
        public EligibilityLimitationsTableCell(IntPtr handle) : base(handle)
        {
            CreateLayout();
            InitializeBindings();
        }

        public override void LayoutSubviews()
        {
            CreateLayout();
            base.LayoutSubviews();
        }

        public void CreateLayout()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;

            float fieldPadding = 25;
            float innerPadding = 10;
            float sidePadding = Constants.DRUG_LOOKUP_SIDE_PADDING;
            float contentWidth = (float)this.Frame.Width;
          
           
            float topPadding = 15;
            float yPos = 0;

            var titleLabelWidth = Constants.IsPhone() ? contentWidth * 0.55f : contentWidth * 0.45f;
            var fieldLabelWidth = Constants.IsPhone() ? contentWidth * 0.45f : contentWidth * 0.55f;

            var resultPos = titleLabelWidth + innerPadding;
            var resultWidth = fieldLabelWidth - sidePadding - innerPadding;
            var labelWidth = titleLabelWidth - sidePadding;

            if (cellBackingView == null)
                cellBackingView = new UIView();
            cellBackingView.Frame = new CGRect(0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
            cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
            cellBackingView.ContentMode = UIViewContentMode.TopLeft;
            BackgroundView = cellBackingView;

            if (limitationDescriptionLabel == null)
                limitationDescriptionLabel = new UILabel();
            limitationDescriptionLabel.Frame = new CGRect(resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            limitationDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoLabelFontSize);
            limitationDescriptionLabel.TextColor = Colors.Black;
            limitationDescriptionLabel.TextAlignment = UITextAlignment.Left;
            limitationDescriptionLabel.BackgroundColor = Colors.Clear;
            limitationDescriptionLabel.Lines = 0;
            limitationDescriptionLabel.SizeToFit();
            AddSubview(limitationDescriptionLabel);

            if (benefitDescriptionLabel == null)
                benefitDescriptionLabel = new UILabel();
            benefitDescriptionLabel.Frame = new CGRect(sidePadding, yPos, labelWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            benefitDescriptionLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            benefitDescriptionLabel.TextAlignment = UITextAlignment.Left;
            benefitDescriptionLabel.TextColor = Colors.LightGrayColorForLabelValuePair;
            benefitDescriptionLabel.BackgroundColor = Colors.Clear;
            benefitDescriptionLabel.ClipsToBounds = false;
            benefitDescriptionLabel.Lines = 0;
            benefitDescriptionLabel.SizeToFit();
            AddSubview(benefitDescriptionLabel);

            yPos += Math.Max((float)limitationDescriptionLabel.Frame.Height, (float)benefitDescriptionLabel.Frame.Height) + fieldPadding;

            if (participantFamilyLabel == null)
                participantFamilyLabel = new UILabel();
            participantFamilyLabel.Frame = new CGRect(resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            participantFamilyLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoLabelFontSize);
            participantFamilyLabel.TextColor = Colors.Black;
            participantFamilyLabel.TextAlignment = UITextAlignment.Left;
            participantFamilyLabel.BackgroundColor = Colors.Clear;
            participantFamilyLabel.Lines = 2;
            participantFamilyLabel.SizeToFit();
            participantFamilyLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(participantFamilyLabel);

            if (participantFamilyTitleLabel == null)
                participantFamilyTitleLabel = new UILabel();
            participantFamilyTitleLabel.Frame = new CGRect(sidePadding, yPos, labelWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            participantFamilyTitleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            participantFamilyTitleLabel.TextAlignment = UITextAlignment.Left;
            participantFamilyTitleLabel.TextColor = Colors.LightGrayColorForLabelValuePair;
            participantFamilyTitleLabel.BackgroundColor = Colors.Clear;
            participantFamilyTitleLabel.Lines = 2;
            participantFamilyTitleLabel.Text = "particpantFamily".tr();
            participantFamilyTitleLabel.SizeToFit();
            participantFamilyTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(participantFamilyTitleLabel);

            if (!participantFamilyLabel.Hidden)
                yPos += (float)participantFamilyTitleLabel.Frame.Height + fieldPadding;

            if (startDateLabel == null)
                startDateLabel = new UILabel();
            startDateLabel.Frame = new CGRect(resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            startDateLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoLabelFontSize);
            startDateLabel.TextColor = Colors.Black;
            startDateLabel.TextAlignment = UITextAlignment.Left;
            startDateLabel.BackgroundColor = Colors.Clear;
            startDateLabel.Lines = 2;
            startDateLabel.SizeToFit();
            startDateLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(startDateLabel);

            if (startDateTitleLabel == null)
                startDateTitleLabel = new UILabel();
            startDateTitleLabel.Frame = new CGRect(sidePadding, yPos, labelWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            startDateTitleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            startDateTitleLabel.TextAlignment = UITextAlignment.Left;
            startDateTitleLabel.TextColor = Colors.LightGrayColorForLabelValuePair;
            startDateTitleLabel.BackgroundColor = Colors.Clear;
            startDateTitleLabel.Lines = 2;
            startDateTitleLabel.Text = "startDate".tr();
            startDateTitleLabel.SizeToFit();
            startDateTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(startDateTitleLabel);

            yPos += (float)startDateTitleLabel.Frame.Height + fieldPadding;


            if (amountLabel == null)
                amountLabel = new UILabel();

            amountLabel.Frame = new CGRect(resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            amountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoLabelFontSize);
            amountLabel.TextAlignment = UITextAlignment.Left;
            amountLabel.TextColor = Colors.Black;
            amountLabel.BackgroundColor = Colors.Clear;
            amountLabel.Lines = 2;
            amountLabel.Text = "$" + AccumAmountUsed.ToString();
            amountLabel.SizeToFit();
            amountLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(amountLabel);

            if (amountTitleLabel == null)
                amountTitleLabel = new UILabel();
            amountTitleLabel.Frame = new CGRect(sidePadding, yPos, labelWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            amountTitleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            amountTitleLabel.TextAlignment = UITextAlignment.Left;
            amountTitleLabel.TextColor = Colors.LightGrayColorForLabelValuePair;
            amountTitleLabel.BackgroundColor = Colors.Clear;
            amountTitleLabel.Lines = 2;
            amountTitleLabel.Text = "amountToDate".tr();
            amountTitleLabel.SizeToFit();
            amountTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(amountTitleLabel);

            if (!amountLabel.Hidden)
                yPos += (float)amountTitleLabel.Frame.Height + fieldPadding;


            if (occurencesLabel == null)
                occurencesLabel = new UILabel();

            occurencesLabel.Frame = new CGRect(resultPos, yPos, resultWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            occurencesLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoLabelFontSize);
            occurencesLabel.TextAlignment = UITextAlignment.Left;
            occurencesLabel.TextColor = Colors.Black;
            occurencesLabel.BackgroundColor = Colors.Clear;
            occurencesLabel.Text = AccumUnitsUsed.ToString();
            occurencesLabel.Lines = 2;
            occurencesLabel.SizeToFit();
            occurencesLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(occurencesLabel);

            if (occurencesTitleLabel == null)
                occurencesTitleLabel = new UILabel();
            occurencesTitleLabel.Frame = new CGRect(sidePadding, yPos, labelWidth, Constants.SINGLE_SELECTION_LABEL_FONT_SIZE);
            occurencesTitleLabel.Font = UIFont.FromName(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize);
            occurencesTitleLabel.TextAlignment = UITextAlignment.Left;
            occurencesTitleLabel.TextColor = Colors.LightGrayColorForLabelValuePair;
            occurencesTitleLabel.BackgroundColor = Colors.Clear;
            occurencesTitleLabel.Lines = 2;
            occurencesTitleLabel.Text = "occurencesToDate".tr();
            occurencesTitleLabel.SizeToFit();
            occurencesTitleLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(occurencesTitleLabel);

            if (!occurencesLabel.Hidden)
                yPos += (float)occurencesTitleLabel.Frame.Height + fieldPadding;


        }

        public virtual bool ShowsDeleteButton()
        {
            return false;
        }


        public virtual void InitializeBindings()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<EligibilityLimitationsTableCell, ClaimPlanLimitationGSC>();
                    set.Bind(this.benefitDescriptionLabel).To(item => item.BenefitDescription);
                    set.Bind(this.limitationDescriptionLabel).To(item => item.LimitationDescription);
                    set.Bind(this.participantFamilyLabel).To(item => item.AppliesTo).OneWay();
                    set.Bind(this.startDateLabel).To(item => item.AccumStartDate).WithConversion("DateToString");
                    set.Bind(this).For(v => v.AccumAmountUsed).To(vm => vm.AccumAmountUsed);
                    set.Bind(this).For(v => v.AccumUnitsUsed).To(vm => vm.AccumUnitsUsed);
                    set.Bind(amountLabel).For(v => v.Hidden).To(vm => vm.IsAccumAmountUsedVisibleForEligibility).WithConversion("BoolOpposite");
                    set.Bind(amountTitleLabel).For(v => v.Hidden).To(vm => vm.IsAccumAmountUsedVisibleForEligibility).WithConversion("BoolOpposite");
                    set.Bind(occurencesLabel).For(v => v.Hidden).To(vm => vm.IsAccumUnitsUsedVisibleForEligibility).WithConversion("BoolOpposite");
                    set.Bind(occurencesTitleLabel).For(v => v.Hidden).To(vm => vm.IsAccumUnitsUsedVisibleForEligibility).WithConversion("BoolOpposite");
                    set.Bind(participantFamilyLabel).For(v => v.Hidden).To(vm => vm.IsParticipantFamilyLabelVisibleForEligibility).WithConversion("BoolOpposite");
                    set.Bind(participantFamilyTitleLabel).For(v => v.Hidden).To(vm => vm.IsParticipantFamilyLabelVisibleForEligibility).WithConversion("BoolOpposite");
                    set.Apply();

                });

            this.SetNeedsLayout();
        }

        private double _accumUnitsUsed = 0;
        public double AccumUnitsUsed
        {
            get
            {
                return _accumUnitsUsed;
            }
            set
            {

                _accumUnitsUsed = value;

                setTotal();
            }

        }

        private double _accumAmountUsed = 0;
        public double AccumAmountUsed
        {
            get
            {
                return _accumAmountUsed;
            }
            set
            {

                _accumAmountUsed = value;

                setTotal();
            }

        }

        public void setTotal()
        {

            if (AccumAmountUsed > 0 && AccumUnitsUsed <= 0)
            {

                amountVisible = true;
                occurencesVisible = true;//false;

            }
            else if (AccumUnitsUsed > 0 && AccumAmountUsed <= 0)
            {

                occurencesVisible = true;
                amountVisible = true;//false;

            }
            else
            {
                amountVisible = occurencesVisible = true;
            }

            amountLabel.Text = "$" + (AccumAmountUsed).ToString();
            occurencesLabel.Text = AccumUnitsUsed.ToString();
        }

    }
}


