using System;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.Entities;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Dashboard
{
    public partial class RecentClaimCellView : MvxCollectionViewCell
    {
        private float _actionRequiredFontSize;
        private float _claimTypeFontSize;
        private float _claimSubmissionDateFontSize;
        private float _claimAmountFontSize;

        public static readonly NSString Key = new NSString("RecentClaimCellView");
        public static UINib Nib;

        static RecentClaimCellView()
        {
            Nib = UINib.FromName(Key, NSBundle.MainBundle);
        }

        protected RecentClaimCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
            SetBackground();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _actionRequiredFontSize = Constants.IsPhone() ? Constants.SMALL_FONT_SIZE : Constants.HEADING_FONT_SIZE;
            _claimTypeFontSize = Constants.IsPhone() ? Constants.TEXT_FIELD_SUB_HEADING_SIZE : Constants.DROPDOWN_FONT_SIZE;
            _claimSubmissionDateFontSize = Constants.IsPhone() ? Constants.HEADING_FONT_SIZE : Constants.TEXT_FIELD_SUB_HEADING_SIZE;
            _claimAmountFontSize = Constants.IsPhone() ? Constants.HEADING_FONT_SIZE : Constants.TEXT_FIELD_SUB_HEADING_SIZE;

            ActionRequiredLabel.SetLabel(Constants.NUNITO_BLACK, _actionRequiredFontSize, Colors.DARK_RED);
            ClaimTypeLabel.SetLabel(Constants.NUNITO_BLACK, _claimTypeFontSize, Colors.DarkGrayColor);
            ClaimAmountLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _claimAmountFontSize, Colors.DarkGrayColor);
            ClaimSubmissionDateLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _claimSubmissionDateFontSize, Colors.DarkGrayColor);
        }

        public override bool Highlighted
        {
            get => base.Highlighted;
            set
            {
                base.Highlighted = value;
                ToggleContainerBackgroundColor(Highlighted);
            }
        }
        private void SetBackground()
        {
            ContentView.BackgroundColor = Colors.LightGrayColor;
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
                ContentView.BackgroundColor = Colors.LightGrayColor;
                ToggleLabelTextColorWhenHighlighted(Colors.DarkGrayColor);
            }
        }

        private void ToggleLabelTextColorWhenHighlighted(UIColor fontColor)
        {
            ClaimTypeLabel.TextColor = fontColor;
            ClaimAmountLabel.TextColor = fontColor;
            ClaimSubmissionDateLabel.TextColor = fontColor;
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<RecentClaimCellView, DashboardRecentClaim>();

                var boolOppositeValueConverter = new BoolOppositeValueConverter();
                var boolToDefaultValueConverter = new BoolToDefaultStringValueConverter();
                var boolToDefaultFloatValueConverter = new BoolToDefaultFloatValueConverter();

                set.Bind(ContentView.Layer).For(x => x.BorderWidth).To(x => x.ActionRequired).WithConversion(boolToDefaultFloatValueConverter, Constants.CellBorderWith);
                set.Bind(ActionRequiredLabel).To(x => x.ActionRequired).WithConversion(boolToDefaultValueConverter, "ActiveClaim_ActionRequired".tr());
                set.Bind(ActionRequiredLabel).For(x => x.Hidden).To(x => x.ActionRequired).WithConversion(boolOppositeValueConverter, null);
                set.Bind(ClaimTypeLabel).To(x => x.ServiceDescription);
                set.Bind(ClaimSubmissionDateLabel).To(x => x.ServiceDate);
                set.Bind(ClaimAmountLabel).To(x => x.ClaimedAmount);

                set.Apply();
            });
        }
    }
}