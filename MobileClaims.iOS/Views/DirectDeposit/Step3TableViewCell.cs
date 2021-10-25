using System;
using Foundation;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit
{
    public partial class Step3TableViewCell : MvxTableViewCell
    {
        private bool _isSubscribedForNotifications;
        public static readonly NSString Key = new NSString("Step3TableViewCell");
        public static readonly UINib Nib = UINib.FromName("Step3TableViewCell", NSBundle.MainBundle);

        public UIButton ReceiveNotificationButton => SelectReceiveNotificationButton;

        public UIButton DoNotReceiveNotificationButto => SelectDoNotReceiveNotification;

        public bool IsSubscribedForNotifications
        {
            get => _isSubscribedForNotifications;
            set
            {
                _isSubscribedForNotifications = value;
                Highlight(_isSubscribedForNotifications);
            }
        }

        public Step3TableViewCell()
        {
            InitializeBinding();
        }

        protected Step3TableViewCell(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public static Step3TableViewCell Create()
        {
            return (Step3TableViewCell)Nib.Instantiate(null, null)[0];
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ReceiveNotificationLabel.SetLabel(Constants.NUNITO_SEMIBOLD, 14.0f, UIColor.FromRGB(27, 27, 27));
            DoNotReceiveNotificationLabel.SetLabel(Constants.NUNITO_SEMIBOLD, 14.0f, UIColor.FromRGB(27, 27, 27));

            ReceiveNotificationContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;
            ReceiveNotificationContainerView.Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            ReceiveNotificationContainerView.Layer.BorderWidth = 1.0f;

            DoNotReceiveNotificationContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;
            DoNotReceiveNotificationContainerView.Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            DoNotReceiveNotificationContainerView.Layer.BorderWidth = 1.0f;
        }

        internal void Highlight(bool highlight)
        {
            ReceiveNotificationRoundImageView.Highlighted = highlight;
            DoNotReceiveNotificationRoundImageView.Highlighted = !highlight;

            if (highlight)
            {
                ReceiveNotificationLabel.TextColor = Colors.BACKGROUND_COLOR;
                ReceiveNotificationContainerView.BackgroundColor = Colors.HIGHLIGHT_COLOR;

                DoNotReceiveNotificationLabel.TextColor = UIColor.FromRGB(27, 27, 27);
                DoNotReceiveNotificationContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;
            }
            else
            {
                ReceiveNotificationLabel.TextColor = UIColor.FromRGB(27, 27, 27);
                ReceiveNotificationContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;

                DoNotReceiveNotificationLabel.TextColor = Colors.BACKGROUND_COLOR;
                DoNotReceiveNotificationContainerView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
            }
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<Step3TableViewCell, DirectDepositStep3Model>();

                set.Bind(ReceiveNotificationLabel).For(x => x.Text).To(model => model.OptInForEmailNotification);
                set.Bind(DoNotReceiveNotificationLabel).For(x => x.Text).To(model => model.OptOutOfEmailNotification);
                set.Bind(this).For(x => x.IsSubscribedForNotifications).To(model => model.IsOptedForNotification);

                set.Apply();
            });
        }

        //private void SetNormalState()
        //{
        //    AuthorizeDirectDepositLabel.TextColor = UIColor.FromRGB(27, 27, 27);
        //    ContainerView.BackgroundColor = Colors.BACKGROUND_COLOR;
        //}
    }
}
