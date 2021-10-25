using System;
using Foundation;
using MobileClaims.Core.Entities;
using MobileClaims.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimConfirmation
{
    public partial class ClaimDetailConfirmationCellView : MvxCollectionViewCell
    {
        private UIStringAttributes _labelTextAttributes;
        private UIStringAttributes _valueLabelTextAttributes;

        public static readonly NSString Key = new NSString("ClaimDetailConfirmationCellView");
        public static readonly UINib Nib;

        static ClaimDetailConfirmationCellView()
        {
            Nib = UINib.FromName("ClaimDetailConfirmationCellView", NSBundle.MainBundle);
        }

        protected ClaimDetailConfirmationCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            _labelTextAttributes = SetLabel(Constants.NUNITO_REGULAR, Constants.ClaimSummaryInfoLabelFontSize, Colors.LightGrayColorForLabelValuePair);
            _valueLabelTextAttributes = SetLabel(Constants.NUNITO_BOLD, Constants.ClaimSummaryInfoValueFontSize, Colors.Black);
        }

        private UIStringAttributes SetLabel(string fontName, float fontSize, UIColor fontColor)
        {
            var attributes = new UIStringAttributes
            {
                ForegroundColor = fontColor,
                Font = UIFont.FromName(fontName, fontSize),
                ParagraphStyle = new NSMutableParagraphStyle()
                {
                    LineSpacing = Constants.AuditListInstructionLineSpacing,
                    LineHeightMultiple = Constants.AuditListInstructionLineSpacing
                }
            };
            return attributes;
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimDetailConfirmationCellView, ClaimQuestionAnswerPair>();
                set.Bind(ClaimDetailLabel)
                    .For(x=>x.AttributedText)
                    .To(vm => vm.Question)
                    .WithConversion("StringToAttributedString", _labelTextAttributes);
                set.Bind(ClaimDetailValueLabel)
                    .For(x => x.AttributedText)
                    .To(vm => vm.Answer)
                    .WithConversion("StringToAttributedString", _valueLabelTextAttributes);
                set.Apply();
            });
        }
    }
}