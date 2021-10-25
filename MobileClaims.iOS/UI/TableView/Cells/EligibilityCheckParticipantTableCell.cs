using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("EligibilityCheckParticipantTableCell")]
    public class EligibilityCheckParticipantTableCell : MvxDeleteTableViewCell
    {
        public UILabel participantTitle, treatment1Title, treatment2Title, treatment3Title;

        public UILabel participantLabel, treatment1Label, treatment2Label, treatment3Label;
        protected UIView cellBackingView;

        protected UIView bottomBorder;

        public EligibilityCheckParticipantTableCell() : base() { }
        public EligibilityCheckParticipantTableCell(IntPtr handle) : base(handle) { }


        public override void LayoutSubviews()
        {
            CreateLayout();
            base.LayoutSubviews();
        }

        private float redrawCount = 0;
        public override void CreateLayout()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;

            float sidePadding = Constants.CONTENT_SIDE_PADDING;
            float participantAreaHeight = 50;
            float statusAreaHeight = 55;
            float rightXPos = (float)(Frame.Width / 5 * 2 + 5);
            float leftXPos = sidePadding;
            float rightWidth = (float)(Frame.Width / 5 * 3 - sidePadding);
            float leftWidth = (float)(Frame.Width / 5 * 2 - sidePadding);

            float fieldPadding = 20;
            float contentWidth = (float)this.Frame.Width - sidePadding * 2;
            float resultPos = Constants.DRUG_LOOKUP_SIDE_PADDING + contentWidth / 2 + fieldPadding;
            float topPadding = 15;
            float yPos = topPadding;

            this.ClearsContextBeforeDrawing = true;

            if (cellBackingView == null)
                cellBackingView = new UIView();
            cellBackingView.Frame = new CGRect(0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
            cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
            cellBackingView.ContentMode = UIViewContentMode.TopLeft;
            BackgroundView = cellBackingView;

            if (participantTitle == null)
                participantTitle = new UILabel();
            participantTitle.Frame = new CGRect(leftXPos, participantAreaHeight - (float)participantTitle.Frame.Height, leftWidth, (float)participantTitle.Frame.Height);
            participantTitle.SizeToFit();
            participantTitle.Frame = new CGRect(leftXPos, participantAreaHeight - (float)participantTitle.Frame.Height, leftWidth, (float)participantTitle.Frame.Height);
            participantTitle.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            participantTitle.BackgroundColor = Colors.Clear;
            participantTitle.Lines = 0;
            participantTitle.TextAlignment = UITextAlignment.Left;
            participantTitle.Text = "depNumParticipant".tr();
            participantTitle.AdjustsFontSizeToFitWidth = true;
            AddSubview(participantTitle);

            if (participantLabel == null)
                participantLabel = new UILabel();
            participantLabel.Frame = new CGRect(rightXPos, (float)participantTitle.Frame.Y, rightWidth, (float)participantLabel.Frame.Height);
            participantLabel.SizeToFit();
            participantLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.ELIGIBILITY_TABLE_FONT_SIZE);
            participantLabel.BackgroundColor = Colors.Clear;
            participantLabel.Lines = 0;
            participantLabel.AdjustsFontSizeToFitWidth = true;
            AddSubview(participantLabel);

            yPos += participantAreaHeight;

            if (treatment1Title == null)
                treatment1Title = new UILabel();
            treatment1Title.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)treatment1Title.Frame.Height);
            treatment1Title.SizeToFit();
            treatment1Title.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)treatment1Title.Frame.Height);
            treatment1Title.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            treatment1Title.BackgroundColor = Colors.Clear;
            treatment1Title.Lines = 0;
            treatment1Title.Text = "treatment1".tr();
            treatment1Title.TextAlignment = UITextAlignment.Left;
            AddSubview(treatment1Title);

            if (treatment1Label == null)
                treatment1Label = new UILabel();
            treatment1Label.Frame = new CGRect(rightXPos, yPos, rightWidth, (float)treatment1Label.Frame.Height);
            treatment1Label.SizeToFit();
            treatment1Label.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.ELIGIBILITY_TABLE_FONT_SIZE);
            treatment1Label.BackgroundColor = Colors.Clear;
            treatment1Label.Lines = 0;
            treatment1Label.AdjustsFontSizeToFitWidth = true;
            AddSubview(treatment1Label);

            yPos += statusAreaHeight;

            if (treatment2Title == null)
                treatment2Title = new UILabel();
            treatment2Title.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)treatment2Title.Frame.Height);
            treatment2Title.SizeToFit();
            treatment2Title.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)treatment2Title.Frame.Height);
            treatment2Title.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            treatment2Title.BackgroundColor = Colors.Clear;
            treatment2Title.Lines = 0;
            treatment2Title.Text = "treatment2".tr();
            treatment2Title.TextAlignment = UITextAlignment.Left;
            AddSubview(treatment2Title);

            if (treatment2Label == null)
                treatment2Label = new UILabel();
            treatment2Label.Frame = new CGRect(rightXPos, yPos, rightWidth, (float)treatment2Label.Frame.Height);
            treatment2Label.SizeToFit();
            treatment2Label.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.ELIGIBILITY_TABLE_FONT_SIZE);
            treatment2Label.BackgroundColor = Colors.Clear;
            treatment2Label.Lines = 0;
            treatment2Label.AdjustsFontSizeToFitWidth = true;
            AddSubview(treatment2Label);

            yPos += statusAreaHeight;

            if (treatment3Title == null)
                treatment3Title = new UILabel();
            treatment3Title.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)treatment3Title.Frame.Height);
            treatment3Title.SizeToFit();
            treatment3Title.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)treatment3Title.Frame.Height);
            treatment3Title.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            treatment3Title.BackgroundColor = Colors.Clear;
            treatment3Title.Lines = 0;
            treatment3Title.Text = "treatment3".tr();
            treatment3Title.TextAlignment = UITextAlignment.Left;
            treatment3Title.AdjustsFontSizeToFitWidth = true;
            AddSubview(treatment3Title);

            if (treatment3Label == null)
                treatment3Label = new UILabel();
            treatment3Label.Frame = new CGRect(rightXPos, yPos, rightWidth, (float)treatment3Label.Frame.Height);
            treatment3Label.SizeToFit();
            treatment3Label.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.ELIGIBILITY_TABLE_FONT_SIZE);
            treatment3Label.BackgroundColor = Colors.Clear;
            treatment3Label.Lines = 0;
            treatment3Label.AdjustsFontSizeToFitWidth = true;
            AddSubview(treatment3Label);

            yPos += statusAreaHeight;

            if (bottomBorder == null)
                this.bottomBorder = new DottedLine();
            this.bottomBorder.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)this.Frame.Height - 1, (float)this.Frame.Width - Constants.DRUG_LOOKUP_SIDE_PADDING, 1);
            AddSubview(bottomBorder);

            //			sizeFont (treatment1Label);
            //			sizeFont (treatment2Label);
            //			sizeFont (treatment3Label);

            if (redrawCount < 2)
            {
                redrawCount++;
                this.SetNeedsLayout();
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            SetHighlighted(true, false);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            SetHighlighted(this.Selected, false);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            SetHighlighted(this.Selected, false);
        }

        //		protected void sizeFont(UILabel fontLabel)
        //		{
        //
        //			if (fontLabel.Text != null) {
        //
        //				UIFont sizedFont = UIFont.FromName (Constants.NUNITO_BOLD, Constants.GS_BUTTON_FONT_SIZE);
        //
        //				int i;
        //
        //				for (i = (int)Constants.GS_BUTTON_FONT_SIZE; i > 8; i = i - 2) {
        //					sizedFont = UIFont.FromName (Constants.NUNITO_BOLD, i);
        //
        //					SizeF constraintSize = new SizeF (260.0f, float.MaxValue);
        //
        //					SizeF labelSize = fontLabel.StringSize (fontLabel.Text, sizedFont, constraintSize, UILineBreakMode.WordWrap);
        //
        //					if (labelSize.Width <= this.Frame.Width)
        //						break;
        //				}
        //
        //				fontLabel.Font = sizedFont;
        //
        //			}
        //
        //		}

        public override void SetSelected(bool selected, bool animated)
        {
            SetHighlightColors(selected);
            base.SetSelected(selected, animated);
        }

        public override void SetHighlighted(bool highlighted, bool animated)
        {
            SetHighlightColors(highlighted);
            base.SetHighlighted(highlighted, animated);
        }

        private void SetHighlightColors(bool selected)
        {
            if (selected)
            {
                treatment1Label.TextColor = Colors.SINGLE_SELECTION_LABEL_HIGHLIGHT_COLOR;
                cellBackingView.BackgroundColor = Colors.Clear;
            }
            else
            {
                treatment1Label.TextColor = Colors.SINGLE_SELECTION_LABEL_COLOR;
                cellBackingView.BackgroundColor = Colors.Clear;
            }

            treatment2Label.TextColor = treatment3Label.TextColor = participantLabel.TextColor = treatment1Label.TextColor;
            treatment1Title.TextColor = treatment2Title.TextColor = treatment3Title.TextColor = participantTitle.TextColor = treatment1Label.TextColor;
        }

        public override bool ShowsDeleteButton()
        {
            return false;
        }

        public virtual bool PersistsSelection()
        {
            return false;
        }

        public override void WillTransitionToState(UITableViewCellState mask)
        {
            //set delete button frame to account for cell margins
            if ((mask & UITableViewCellState.ShowingDeleteConfirmationMask) == UITableViewCellState.ShowingDeleteConfirmationMask && ShowsDeleteButton())
            {
                UIView target = this.Subviews[this.Subviews.Length - 1];
                base.deleteFrame = new CGRect((float)target.Frame.Width - (float)this.Frame.Width, (float)participantTitle.Frame.Y, (float)target.Frame.Width, (float)participantTitle.Frame.Height);
                base.WillTransitionToState(mask);
            }
        }

        public override void InitializeBindings()
        {
            this.DelayBind(() =>
                {
                    var set = this.CreateBindingSet<EligibilityCheckParticipantTableCell, ParticipantEligibilityResult>();
                    //					set.Bind(this.participantLabel).To(item => item.ParticipantFullName).WithConversion("StringCase").OneWay();
                    //					set.Bind(this.treatment1Label).To(item => item.PolishingEligibilityStatus).WithConversion("EligibilityStatus").OneWay();
                    //					set.Bind(this.treatment2Label).To(item => item.OrthoticEligibilityStatus).WithConversion("EligibilityStatus").OneWay();
                    //					set.Bind(this.treatment3Label).To(item => item.RecallExamEligibilityStatus).WithConversion("EligibilityStatus").OneWay();
                    set.Bind(this).For(v => v.ParticipantEligibilityResult).To(item => item);
                    set.Apply();
                });
        }

        private ParticipantEligibilityResult _participantEligibilityResult;
        public ParticipantEligibilityResult ParticipantEligibilityResult
        {
            get
            {
                return _participantEligibilityResult;
            }
            set
            {
                _participantEligibilityResult = value;
                if (value != null)
                {

                    participantLabel.Text = value.ParticipantNumber + ":" + " " + value.ParticipantFullName;

                    if (value.PolishingEligibilityDate != null && value.PolishingEligibilityDate != DateTime.MinValue)
                    {
                        treatment3Label.Text = value.PolishingEligibilityDateFormatted + " " + (value.PolishingEligibilityStatus != null ? value.PolishingEligibilityStatus : "");
                    }
                    else
                    {
                        treatment3Label.Text = !string.IsNullOrWhiteSpace(value.PolishingEligibilityStatus)
                            ? value.PolishingEligibilityStatus
#if FPPM
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumberFPPM);
#else
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumber);
#endif
                    }


                    if (value.ScalingEligibilityDate != null && value.ScalingEligibilityDate != DateTime.MinValue)
                    {
                        treatment2Label.Text = value.ScalingEligibilityDateFormatted + " " + (value.ScalingEligibilityStatus != null ? value.ScalingEligibilityStatus : "");
                    }
                    else
                    {
                        treatment2Label.Text = !string.IsNullOrWhiteSpace(value.ScalingEligibilityStatus)
                            ? value.ScalingEligibilityStatus
#if FPPM
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumberFPPM);
#else
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumber);
#endif
                    }

                    if (value.RecallExamEligibilityDate != null && value.RecallExamEligibilityDate != DateTime.MinValue)
                    {
                        treatment1Label.Text = value.RecallExamEligibilityDateFormatted + " " + (value.RecallExamEligibilityStatus != null ? value.RecallExamEligibilityStatus : "");
                    }
                    else
                    {
                        treatment1Label.Text = !string.IsNullOrWhiteSpace(value.RecallExamEligibilityStatus)
                            ? value.RecallExamEligibilityStatus
#if FPPM
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumberFPPM);
#else
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumber);
#endif
                    }

                }

            }
        }
    }
}

