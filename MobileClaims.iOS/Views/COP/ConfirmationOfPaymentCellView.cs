using System;
using Foundation;
using MobileClaims.Core.Entities;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.COP
{
    public partial class ConfirmationOfPaymentCellView : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ConfirmationOfPaymentCellView");

        public ConfirmationOfPaymentCellView()
        {
            InitializeBinding();
            SetBackground();
        }

        protected ConfirmationOfPaymentCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
            SetBackground();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            SetResourceStrings();
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            SetHighlighted(selected);
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            SetFonts();
            SetTextColor(Colors.DARK_GREY_COLOR);
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ConfirmationOfPaymentCellView, TopCardViewData>();

                set.Bind(DateLabel).To(x => x.ServiceDate);
                set.Bind(ClaimFormIdLabel).To(x => x.ClaimForm);
                set.Bind(BenefitTypeLabel).To(x => x.ServiceDescription);
                set.Bind(ClaimedAmountLabel).To(x => x.ClaimedAmount);

                set.Apply();
            });
        }

        private void SetResourceStrings()
        {
            ActionRequiredLabel.Text = "ActiveClaim_ActionRequired".tr();
            CurrencyLabel.Text = "COP_ClaimedAmount".tr();
        }

        private void SetTextColor(UIColor color)
        {
            ActionRequiredLabel.TextColor = Colors.DARK_RED;
            DateLabel.TextColor = color;
            ClaimFormIdLabel.TextColor = color;
            BenefitTypeLabel.TextColor = color;
            CurrencyLabel.TextColor = color;
            ClaimedAmountLabel.TextColor = color;
        }

        private void SetFonts()
        {
            ActionRequiredLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
            DateLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ClaimFormIdLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            BenefitTypeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.TEXT_FIELD_SUB_HEADING_SIZE);
            CurrencyLabel.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, Constants.HEADING_HCSA_FONT_SIZE);
            ClaimedAmountLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, Constants.SMALL_FONT_SIZE);
        }

        private void SetHighlighted(bool selected)
        {
            if (selected)
            {
                SetContentBackground(Colors.HIGHLIGHT_COLOR);
                SetTextColor(Colors.BACKGROUND_COLOR);
                ActionRequiredLabel.TextColor = Colors.BACKGROUND_COLOR;
            }
            else
            {
                SetContentBackground(Colors.LightGrayColor);
                SetTextColor(Colors.DARK_GREY_COLOR);
            }
        }

        private void SetBackground()
        {
            SetContentBackground(Colors.LightGrayColor);
            ContentView.Layer.BorderColor = Colors.DARK_RED.CGColor;
            ContentView.Layer.BorderWidth = Constants.CellBorderWith;
            this.UserInteractionEnabled = true;
            if (SelectedBackgroundView != null)
            {
                this.SelectedBackgroundView.Layer.BorderColor = Colors.DARK_RED.CGColor;
                this.SelectedBackgroundView.Layer.BorderWidth = Constants.CellBorderWith;
            }
        }

        private void SetContentBackground(UIColor backgroundColor)
        {
            ContentView.BackgroundColor = backgroundColor;
        }
    }
}