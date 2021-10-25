using System;
using System.Collections.ObjectModel;
using Foundation;
using MobileClaims.iOS.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.CellTemplates
{
    public partial class TextFieldValidationCellView : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("TextFieldValidationCellView");
        public static readonly UINib Nib = UINib.FromName("TextFieldValidationCellView", NSBundle.MainBundle);

        static TextFieldValidationCellView()
        {
            Nib = UINib.FromName("TextFieldValidationCellView", NSBundle.MainBundle);
        }

        public TextFieldValidationCellView()
        {
            InitializeBinding();
        }

        protected TextFieldValidationCellView(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public static TextFieldValidationCellView Create()
        {
            return (TextFieldValidationCellView)Nib.Instantiate(null, null)[0];
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            ErrorMessage.SetLabel(Constants.NUNITO_SEMIBOLD, 14.0f, UIColor.FromRGB(27, 27, 27));
            // TODO: Might be needed for the Error validation code
            //.SetLabel(Constants.NUNITO_SEMIBOLD, 14.0f, UIColor.FromRGB(27, 27, 27));
            //ContainerView.Layer.BorderColor = UIColor.FromRGB(0.88f, 0.88f, 0.88f).CGColor;
            //ContainerView.Layer.BorderWidth = 1.0f;
        }

        private void InitializeBinding()
        {
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<TextFieldValidationCellView, string>();
                set.Bind(this).For(x => x.ErrorMessage.Text).To(error => error);

                // TODO: Needs to be implemented for validation
                // set.Bind(this).For(x => x.IsDirectDepositAuthorized).To(model => model.IsDirectDepositAuthorized);
                set.Apply();
            });
        }
    }
}
