using Foundation;
using MobileClaims.Core.Models;
using System;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public partial class ClaimSummaryHeader : MvxBindableCollectionReusableView
    {
        private float _labelFontSize;
        private float _valueFontSize;
        private float _sectionHeaderFontSize;

        public static readonly NSString Key = new NSString("ClaimSummaryHeader");
        public static readonly UINib Nib;
               
        static ClaimSummaryHeader()
        {
            Nib = UINib.FromName("ClaimSummaryHeader", NSBundle.MainBundle);
        }

        protected ClaimSummaryHeader(IntPtr handle)
            : base(handle)
        {
            InitializeBindings();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            
            _labelFontSize = Constants.ClaimSummaryInfoLabelFontSize;
            _valueFontSize = Constants.ClaimSummaryInfoValueFontSize;
            _sectionHeaderFontSize = Constants.ClaimSummarySectionHeaderFontSize;

            var labelColor = new UIColor(0.55f, 0.56f, 0.57f, 1f);
            var valueColor = Colors.Black;

            HeaderTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, _sectionHeaderFontSize, valueColor);
            GSCNumberLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _labelFontSize, labelColor);
            ClaimFormNumberLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _labelFontSize, labelColor);
            SubmissionDateLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _labelFontSize, labelColor);

            GSCNumberValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            ClaimFormNumberValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            SubmissionDateValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
        }

        private void InitializeBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimSummaryHeader, AuditClaimSummaryHeader>();
                set.Bind(GSCNumberValueLabel).To(x => x.GscNumber);
                set.Bind(ClaimFormNumberValueLabel).To(x => x.ClaimFormNumber);
                set.Bind(SubmissionDateValueLabel).To(x => x.SubmissionDate);
                set.Apply();
            });
        }
    }
}