using System;
using Foundation;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit
{
    public partial class DirectDepositParentCellView : MvxTableViewHeaderFooterView
    {
        private bool _isExpand;
        private bool _isStepCompleted;
        private string _stepNumber;
        private string _stepName;
        public static readonly NSString Key = new NSString("DirectDepositParentCellView");
        public static readonly UINib Nib = UINib.FromName("DirectDepositParentCellView", NSBundle.MainBundle);

        public UIButton Button => SelectButton;

        public bool IsStepCompleted
        {
            get => _isStepCompleted;
            set
            {
                _isStepCompleted = value; 
                if (_isStepCompleted)
                {
                    RoundImageView.Image = UIImage.FromBundle("RoundCheckboxSelected");
                }
                else
                {
                    RoundImageView.Image = UIImage.FromBundle("RoundCheckBoxUnselected");
                }
            }
        }

        public bool IsExpand
        {
            get => _isExpand;
            set
            {
                _isExpand = value;
                if (_isExpand)
                {
                    ExpandImageView.Image = UIImage.FromBundle("ArrowUpGrayMedium");
                    // BottomBorder.BackgroundColor = Colors.BACKGROUND_COLOR;
                }
                else
                {
                    ExpandImageView.Image = UIImage.FromBundle("ArrowDownGrayMedium");
                    //  BottomBorder.BackgroundColor = UIColor.FromRGB(242, 242, 242);
                }
            }
        }

        public string StepNumber
        {
            get => _stepNumber;
            set
            {
                _stepNumber = value;
                SetNeedsLayout();
            }
        }

        public string StepName
        {
            get => _stepName;
            set
            {
                _stepName = value;
                SetNeedsLayout();
            }
        }

        public DirectDepositParentCellView()
        {
            InitializeBinding();
        }

        protected DirectDepositParentCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetStepName();
        }

        internal void ShowBottomBorder()
        {
            if (!IsExpand)
            {
                BottomBorder.BackgroundColor = BottomBorder.BackgroundColor = UIColor.FromRGB(242, 242, 242);
            }
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<DirectDepositParentCellView, DirectDepositStep>();
                set.Bind(this).For(x => x.StepName).To(model => model.StepName);
                set.Bind(this).For(x => x.StepNumber).To(model => model.StepNumber);
                set.Bind(this).For(x => x.IsExpand).To(model => model.IsExpanded);
                set.Bind(this).For(x => x.IsStepCompleted).To(model => model.IsStepCompleted);
                set.Apply();
            });
        }

        private void SetStepName()
        {
            if (string.IsNullOrWhiteSpace(StepName) || string.IsNullOrWhiteSpace(StepNumber))
            {
                return;
            }

            var stepTextAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.Black,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
            };

            var stepNameTextAttributes = new UIStringAttributes
            {
                ForegroundColor = Colors.HIGHLIGHT_COLOR,
                Font = UIFont.FromName(Constants.NUNITO_BOLD, 14),
            };

            var mutableAttributeString = new NSMutableAttributedString($"{StepNumber} {StepName}");

            mutableAttributeString.SetAttributes(stepTextAttributes,
                new NSRange(0, StepNumber.Length + 1));

            mutableAttributeString.SetAttributes(stepNameTextAttributes,
              new NSRange(StepNumber.Length + 1, StepName.Length));

            StepNameLabel.AttributedText = mutableAttributeString;
        }
    }
}