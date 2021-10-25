using System;
using CoreGraphics;
using UIKit;

namespace MobileClaims.iOS
{
	public class TextFieldWithLabel : UIView
	{
		private const float TITLE_FIELD_PADDING = 120;
		private const float TITLE_PADDING = 10;

		protected UILabel textFieldTitle;
		public UITextField textField;

		public TextFieldWithLabel (string titleText, string textPlaceholder)
		{
			this.BackgroundColor = Colors.LightGrayColor;
			this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
			this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;

			this.textFieldTitle = new UILabel();
			this.textFieldTitle.Text = titleText;
			this.textFieldTitle.BackgroundColor = Colors.Clear;
			this.textFieldTitle.TextColor = Colors.HIGHLIGHT_COLOR;
			this.textFieldTitle.TextAlignment = UITextAlignment.Left;
			textFieldTitle.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.TEXT_FIELD_LABEL_SIZE);
			this.AddSubview(this.textFieldTitle);

			this.textField = new CorrectedTextField ();
			this.textField.Placeholder = textPlaceholder;
			this.textField.KeyboardType = UIKeyboardType.Default;
			this.textField.AutocorrectionType = UITextAutocorrectionType.No;
			this.textField.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			this.textField.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			this.textField.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.TEXT_FIELD_FONT_SIZE);
			this.textField.ShouldReturn += (tf) => { 
				tf.ResignFirstResponder();
				return true; 
			};

			this.AddSubview (textField);
		}

		public string text
		{
			get
			{
				return textField.Text;
			}
			set
			{
				textField.Text = value;
			}
		}

		public override void LayoutSubviews ()
		{
			this.textFieldTitle.SizeToFit ();

			this.textFieldTitle.Frame = new CGRect (TITLE_PADDING, 0, (float)textFieldTitle.Frame.Width, (float)this.Frame.Height);

			float textFieldX = (float)textFieldTitle.Frame.X + (float)textFieldTitle.Frame.Width + TITLE_PADDING;

			this.textField.Frame = new CGRect (textFieldX, 0, (float)this.Frame.Width - textFieldX, (float)this.Frame.Height);
		
		}
	}
}

