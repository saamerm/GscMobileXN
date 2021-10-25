using System;
using System.Globalization;
using Foundation;
using MobileClaims.Core.Converters;
using MobileClaims.Core.ViewModels.DirectDeposit;
using MobileClaims.iOS.Converters;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MobileClaims.iOS.Views.DirectDeposit
{
    public partial class Step2TableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("Step2TableViewCell");
        public static readonly UINib Nib = UINib.FromName("Step2TableViewCell", NSBundle.MainBundle);
        

        public UIButton GoToStep3Button => SaveAndContinueButton;

        public Step2TableViewCell()
        {
            InitializeBinding();
        }

        protected Step2TableViewCell(IntPtr handle)
            : base(handle)
        {
            InitializeBinding();
        }

        public static Step2TableViewCell Create()
        {
            return (Step2TableViewCell)Nib.Instantiate(null, null)[0];
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            TransitNumberTextField.TextColor = UIColor.FromRGB(0, 174, 241);
            TransitNumberTextField.ValueColor = UIColor.FromRGB(43, 43, 43);
            TransitNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
            TransitNumberTextField.MaxLength = 5;

            AccountNumberTextField.TextColor = UIColor.FromRGB(141, 198, 63);
            AccountNumberTextField.ValueColor = UIColor.FromRGB(43, 43, 43);
            AccountNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
            AccountNumberTextField.MaxLength = 18;

            BankNumberTextField.TextColor = UIColor.FromRGB(102, 44, 147);
            BankNumberTextField.ValueColor = UIColor.FromRGB(43, 43, 43);
            BankNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
            BankNumberTextField.MaxLength = 3;

            SetTextColor();
            SetSampleChequeImage();

        }

        private void InitializeBinding()
        {
            var nullableValueConverter = new NullableValueConverter();
            var invertedBoolValueConverter = new BoolOppositeValueConverter();
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<Step2TableViewCell, DirectDepositStep2Model>();
                
                set.Bind(EnterBankInfoLabel).To(model => model.EnterBankingInfoMessage);

                set.Bind(TransitNumberTextField).For(x => x.Text).To(model => model.TransitNumberTitle);
                set.Bind(TransitNumberTextField.TextField).To(vm => vm.TransitNumber).WithConversion(nullableValueConverter);
                set.Bind(TransitNumberErrorLabel).For(x => x.Text).To(vm => vm.TransitNumberErrorText);

                set.Bind(BankNumberTextField).For(x => x.Text).To(model => model.BankNumberTitle);
                set.Bind(BankNumberTextField.TextField).To(vm => vm.BankNumber).WithConversion(nullableValueConverter);
                set.Bind(BankNumberErrorLabel).For(x => x.Text).To(vm => vm.BankNumberErrorText);

                set.Bind(AccountNumberTextField).For(x => x.Text).To(model => model.AccountNumberTitle);
                set.Bind(AccountNumberTextField.TextField).To(vm => vm.AccountNumber).WithConversion(nullableValueConverter);
                set.Bind(AccountNumberErrorLabel).For(x => x.Text).To(vm => vm.AccountNumberErrorText);

                set.Bind(SaveAndContinueButton).For("Title").To(model => model.SaveAndContinueTitle);
                set.Apply();
            });
          
        }

        private void SetTextColor()
        {
            TransitNumberErrorLabel.Lines = 0;
            TransitNumberErrorLabel.LineBreakMode = UILineBreakMode.WordWrap;
            TransitNumberErrorLabel.TextColor = Colors.DARK_RED;
            TransitNumberErrorLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            TransitNumberErrorLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            BankNumberErrorLabel.Lines = 0;
            BankNumberErrorLabel.LineBreakMode = UILineBreakMode.WordWrap;
            BankNumberErrorLabel.TextColor = Colors.DARK_RED;
            BankNumberErrorLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            BankNumberErrorLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            AccountNumberErrorLabel.Lines = 0;
            AccountNumberErrorLabel.LineBreakMode = UILineBreakMode.WordWrap;
            AccountNumberErrorLabel.TextColor = Colors.DARK_RED;
            AccountNumberErrorLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, 14);
            AccountNumberErrorLabel.TranslatesAutoresizingMaskIntoConstraints = false;
        }

        private void SetSampleChequeImage()
        {
            if (!CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Contains("en"))
            {
                SampleChequeImageView.Image = UIImage.FromBundle("SampleChequeFr");
            }
            else
            {
                SampleChequeImageView.Image = UIImage.FromBundle("SampleCheque");
            }
        }
    }
}
