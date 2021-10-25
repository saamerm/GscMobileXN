using System;
using Foundation;
using MobileClaims.Core.Models;
using MobileClaims.iOS.Converters;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public partial class ClaimSummaryCellView : MvxCollectionViewCell
    {
        private float _claimCounterFontSize;
        private float _labelFontSize;
        private float _valueFontSize;

        public static readonly NSString Key = new NSString("ClaimSummaryCellView");
        public static readonly UINib Nib;

        static ClaimSummaryCellView()
        {
            Nib = UINib.FromName("ClaimSummaryCellView", NSBundle.MainBundle);
        }

        protected ClaimSummaryCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _claimCounterFontSize = Constants.ClaimSummaryClaimCounterFontSize;
            _labelFontSize = Constants.ClaimSummaryInfoLabelFontSize;
            _valueFontSize = Constants.ClaimSummaryInfoValueFontSize;

            var labelColor = Colors.LightGrayColorForLabelValuePair;
            var valueColor = Colors.Black;

            ServiceDateLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            ServiceDescriptionLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            ClaimedAmountLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            OtherPaidAmountLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            PaidAmountLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            CopayLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);
            ClaimStatusLabel.SetLabel(Constants.NUNITO_REGULAR, _labelFontSize, labelColor);

            ClaimCounterLabel.SetLabel(Constants.NUNITO_BLACK, _claimCounterFontSize, valueColor);
            ServiceDateValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            ServiceDescriptionValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            ClaimedAmountValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            OtherPaidAmountValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            PaidAmountValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            CopayValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            ClaimStatusValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, Colors.DARK_RED);
        }

        private void InitializeBinding()
        {
            var boolToDefaultFloatValueConverter = new ClaimStateToDefaultFloatValueConverter();
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimSummaryCellView, ClaimFormClaimSummary>();
                set.Bind(ClaimCounterLabel).To(x => x.CountValue);

                set.Bind(ServiceDateLabel).To(x => x.ServiceDate);
                set.Bind(ServiceDateValueLabel).To(x => x.ServiceDateValue);

                set.Bind(ServiceDescriptionLabel).To(x => x.ServiceDescription);
                set.Bind(ServiceDescriptionValueLabel).To(x => x.ServiceDescriptionValue);

                set.Bind(ClaimedAmountLabel).To(x => x.ClaimedAmount);
                set.Bind(ClaimedAmountValueLabel).To(x => x.ClaimedAmountValue);

                set.Bind(OtherPaidAmountLabel).To(x => x.OtherPaidAmount);
                set.Bind(OtherPaidAmountValueLabel).To(x => x.OtherPaidAmountValue);

                set.Bind(PaidAmountLabel).To(x => x.PaidAmount);
                set.Bind(PaidAmountValueLabel).To(x => x.PaidAmountValue);

                set.Bind(CopayLabel).To(x => x.Copay);
                set.Bind(CopayValueLabel).To(x => x.CopayValue);

                set.Bind(ClaimStatusLabel).To(x => x.ClaimStatus);
                set.Bind(ClaimStatusValueLabel).To(x => x.ClaimStatusValue);

                set.Apply();
            });
        }

        internal void SetTopConstraints()
        {
            if (string.IsNullOrWhiteSpace(PaidAmountValueLabel.Text) && string.IsNullOrWhiteSpace(PaidAmountLabel.Text))
            {
                PaidAmountContainer.Hidden = true;
            }

            if (string.IsNullOrWhiteSpace(OtherPaidAmountLabel.Text) && string.IsNullOrWhiteSpace(OtherPaidAmountValueLabel.Text))
            {
                OtherPaidAmountContainer.Hidden = true;
            }

            if (string.IsNullOrWhiteSpace(CopayLabel.Text) && string.IsNullOrWhiteSpace(CopayValueLabel.Text))
            {
                CopayContainer.Hidden = true;
            }

            if (string.IsNullOrWhiteSpace(ClaimStatusLabel.Text) && string.IsNullOrWhiteSpace(ClaimStatusValueLabel.Text))
            {
                ClaimStatusContainer.Hidden = true;
            }
        }
    }
}