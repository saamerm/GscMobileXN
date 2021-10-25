using System;
using Foundation;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit
{
    public partial class Step1TableViewCell : MvxTableViewCell
    {
        private bool _isDirectDepositAuthorized;
        private string _authorizedDirectDepositText;

        public static readonly NSString Key = new NSString("Step1TableViewCell");
        public static readonly UINib Nib = UINib.FromName("Step1TableViewCell", NSBundle.MainBundle);

        public UIButton Button => SelectAuthorizeDirectDepositButton;
        public UIImageView RoundImage => RoundImageView;

        public bool IsDirectDepositAuthorized
        {
            get => _isDirectDepositAuthorized;
            set
            {
                _isDirectDepositAuthorized = value;
                Highlight(_isDirectDepositAuthorized);
            }
        }

        public string AuthorizeDirectDepositText
        {
            get => _authorizedDirectDepositText;
            set
            {
                _authorizedDirectDepositText = value;
                AuthorizeDirectDepositLabel.Text = _authorizedDirectDepositText;
                SetNeedsLayout();
            }
        }

        //static Step1TableViewCell()
        //{
        //    Nib = UINib.FromName("Step1TableViewCell", NSBundle.MainBundle);
        //}

        public Step1TableViewCell()
        {
            InitializeBinding();
        }

        protected Step1TableViewCell(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public static Step1TableViewCell Create()
        {
            return (Step1TableViewCell)Nib.Instantiate(null, null)[0];
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            SetNormalState();

            AuthorizeDirectDepositLabel.SetLabel(Constants.NUNITO_SEMIBOLD, 14.0f, UIColor.FromRGB(27, 27, 27));
            ContainerView.Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            ContainerView.Layer.BorderWidth = 1.0f;
        }

        internal void Highlight(bool highlight)
        {
            RoundImageView.Highlighted = highlight;
            if (highlight)
            {
                AuthorizeDirectDepositLabel.TextColor = Colors.BACKGROUND_COLOR;
                ContainerView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            }
            else
            {
                SetNormalState();
            }
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<Step1TableViewCell, DirectDepositStep1Model>();
                set.Bind(this).For(x => x.AuthorizeDirectDepositText).To(model => model.OptInForDirectDeposit);
                set.Bind(this).For(x => x.IsDirectDepositAuthorized).To(model => model.IsDirectDepositAuthorized);
                set.Apply();
            });
        }

        private void SetNormalState()
        {
            AuthorizeDirectDepositLabel.TextColor = UIColor.FromRGB(27, 27, 27);
            ContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;
        }
    }
}
