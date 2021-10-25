using System;
using CoreGraphics;
using Foundation;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS
{
    [Foundation.Register("EligibilityCheckParticipantCFOTableCell")]
    public class EligibilityCheckParticipantCFOTableCell : MvxDeleteTableViewCell
    {
        public UILabel participantTitle, treatment1Title;

        public UILabel participantLabel, treatment1Label;
        protected UIView cellBackingView;

        protected UIView bottomBorder;

        public EligibilityCheckParticipantCFOTableCell() : base() { }
        public EligibilityCheckParticipantCFOTableCell(IntPtr handle) : base(handle) { }


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

            if (cellBackingView == null)
                cellBackingView = new UIView();
            cellBackingView.Frame = new CGRect(0, 0, (float)this.Frame.Width, (float)this.Frame.Height);
            cellBackingView.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleWidth;
            cellBackingView.ContentMode = UIViewContentMode.TopLeft;
            BackgroundView = cellBackingView;

            if (participantTitle == null)
                participantTitle = new UILabel();
            participantTitle.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)participantTitle.Frame.Height);
            participantTitle.SizeToFit();
            participantTitle.Frame = new CGRect(leftXPos, yPos, leftWidth, (float)participantTitle.Frame.Height);
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
            //AddSubview (treatment1Title);

            if (treatment1Label == null)
                treatment1Label = new UILabel();
            treatment1Label.Frame = new CGRect(rightXPos, yPos, rightWidth, (float)treatment1Label.Frame.Height);
            treatment1Label.SizeToFit();
            treatment1Label.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.ELIGIBILITY_TABLE_FONT_SIZE);
            treatment1Label.BackgroundColor = Colors.Clear;
            treatment1Label.Lines = 0;
            treatment1Label.AdjustsFontSizeToFitWidth = true;
            AddSubview(treatment1Label);

            if (bottomBorder == null)
                this.bottomBorder = new DottedLine();
            this.bottomBorder.Frame = new CGRect(Constants.DRUG_LOOKUP_SIDE_PADDING, (float)this.Frame.Height - 1, (float)this.Frame.Width - Constants.DRUG_LOOKUP_SIDE_PADDING, 1);
            AddSubview(bottomBorder);

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

            participantLabel.TextColor = treatment1Label.TextColor;
            participantTitle.TextColor = treatment1Title.TextColor = treatment1Label.TextColor;
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
                    var set = this.CreateBindingSet<EligibilityCheckParticipantCFOTableCell, ParticipantEligibilityResult>();
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

                    if (value.OrthoticEligibilityDate != null && value.OrthoticEligibilityDate != DateTime.MinValue)
                        treatment1Label.Text = value.OrthoticEligibilityDateFormatted + " " + (value.OrthoticEligibilityStatus != null ? value.OrthoticEligibilityStatus : "");
                    else
                    {
                        treatment1Label.Text = !string.IsNullOrWhiteSpace(value.RecallExamEligibilityStatus)
                            ? value.RecallExamEligibilityStatus
                            : LocalizableBrand.UnableToDetermineEligibility.FormatWithBrandKeywords(LocalizableBrand.PhoneNumber);
                    }
#if FPPM
                        treatment1Label.Text = LocalizableBrand.UnableToDetermineEligibility
                            .FormatWithBrandKeywords(LocalizableBrand.PhoneNumberFPPM);
#endif
                }

            }
		}
	}
}

