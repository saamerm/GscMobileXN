using System;
using Foundation;
using MobileClaims.Core.Models;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.Claims
{
    public partial class ClaimSummaryFooter : MvxBindableCollectionReusableView
    {
        private float _labelFontSize;
        private float _valueFontSize;
        private float _sectionHeaderFontSize;

        public static readonly NSString Key = new NSString("ClaimSummaryFooter");
        public static readonly UINib Nib;

        static ClaimSummaryFooter()
        {
            Nib = UINib.FromName("ClaimSummaryFooter", NSBundle.MainBundle);
        }

        protected ClaimSummaryFooter(IntPtr handle)
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

            FooterTitleLabel.SetLabel(Constants.LEAGUE_GOTHIC, _sectionHeaderFontSize, valueColor);
            AuditDueDateLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _labelFontSize, labelColor);
            AuditInformationLabel.SetLabel(Constants.NUNITO_SEMIBOLD, _labelFontSize, labelColor);

            AuditDueDateValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
            AuditInformationValueLabel.SetLabel(Constants.NUNITO_BOLD, _valueFontSize, valueColor);
        }

        private void InitializeBindings()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimSummaryFooter, AuditClaimSummaryFooter>();
                set.Bind(FooterTitleLabel).To(x => x.AuditDetailsLabel);
                set.Bind(AuditDueDateLabel).To(x => x.AuditDueDateLabel);
                set.Bind(AuditDueDateValueLabel).To(x => x.AuditDueDate);
                set.Bind(AuditInformationLabel).To(x => x.AuditInformationLabel);
                set.Bind(AuditInformationValueLabel).To(x => x.AuditInformation);
                set.Apply();
            });
        }
    }
}