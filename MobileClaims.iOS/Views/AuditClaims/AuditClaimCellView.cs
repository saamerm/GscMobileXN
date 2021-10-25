using System;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.AuditClaims
{
    public partial class AuditClaimCellView : MvxCollectionViewCell
    {
        private float _actionRequiredFontSize;
        private float _claimTypeFontSize;
        private float _claimSubmissionDateFontSize;

        public static readonly NSString Key = new NSString("AuditClaimCellView");
        public static readonly UINib Nib;

        public override bool Highlighted
        {
            get => base.Highlighted;
            set
            {
                base.Highlighted = value;
                ToggleContainerBackgroundColor(Highlighted);
            }
        }

        public string SubmissionDateLabelText
        {
            set => SubmissionDateLabel.Text = value;
        }

        public string DueDateLabelText
        {
            set => DueDateLabel.Text = value;
        }

        static AuditClaimCellView()
        {
            Nib = UINib.FromName("AuditClaimCellView", NSBundle.MainBundle);
        }

        protected AuditClaimCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
            SetBackground();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _actionRequiredFontSize = Constants.AuditClaimCellActionRequiredFontSize;
            _claimTypeFontSize = Constants.AuditClaimCellDescriptionFontSize;
            _claimSubmissionDateFontSize = Constants.AuditClaimCellGeneralFontSize;

            ActionRequiredLabel.SetLabel(Constants.NUNITO_BLACK, _actionRequiredFontSize, Colors.DARK_RED);
            ClaimFormNumberLabel.SetLabel(Constants.NUNITO_REGULAR, _claimSubmissionDateFontSize, Colors.DarkGrayColor);
            SubmissionDateLabel.SetLabel(Constants.NUNITO_REGULAR, _claimSubmissionDateFontSize, Colors.DarkGrayColor);
            SubmissionDateValueLabel.SetLabel(Constants.NUNITO_REGULAR, _claimSubmissionDateFontSize, Colors.DarkGrayColor);
            DueDateLabel.SetLabel(Constants.NUNITO_REGULAR, _claimSubmissionDateFontSize, Colors.DarkGrayColor);
            DueDateValueLabel.SetLabel(Constants.NUNITO_REGULAR, _claimSubmissionDateFontSize, Colors.DarkGrayColor);
            ClaimTypeLabel.SetLabel(Constants.NUNITO_BLACK, _claimTypeFontSize, Colors.DarkGrayColor);

            ContentView.Layer.BorderWidth = Constants.CellBorderWith;
            ActionRequiredLabel.Text = "ActiveClaim_ActionRequired".tr();
        }

        private void SetBackground()
        {
            ContentView.BackgroundColor = Colors.DARK_GREY_COLOR_WITH_ALPHA;
            ContentView.Layer.BorderColor = Colors.DARK_RED.CGColor;
            this.UserInteractionEnabled = true;
        }

        private void ToggleContainerBackgroundColor(bool highlighted)
        {
            if (highlighted)
            {
                ContentView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                ToggleLabelTextColorWhenHighlighted(Colors.BACKGROUND_COLOR);
            }
            else
            {
                ContentView.BackgroundColor = Colors.DARK_GREY_COLOR_WITH_ALPHA;
                ToggleLabelTextColorWhenHighlighted(Colors.DarkGrayColor);
            }
        }

        private void ToggleLabelTextColorWhenHighlighted(UIColor fontColor)
        {
            ClaimFormNumberLabel.TextColor = fontColor;
            SubmissionDateLabel.TextColor = fontColor;
            SubmissionDateValueLabel.TextColor = fontColor;
            DueDateLabel.TextColor = fontColor;
            DueDateValueLabel.TextColor = fontColor;
            ClaimTypeLabel.TextColor = fontColor;
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<AuditClaimCellView, MyAlertsAuditClaim>();

                set.Bind(ClaimFormNumberLabel).To(x => x.ClaimFormId);
                set.Bind(SubmissionDateValueLabel).To(x => x.ClaimSubmissionDate);
                set.Bind(DueDateValueLabel).To(x => x.ClaimDueDate);
                set.Bind(ClaimTypeLabel).To(x => x.ServiceDescription);

                set.Apply();
            });
        }
    }
}