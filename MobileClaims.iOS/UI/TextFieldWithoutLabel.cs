using CoreGraphics;
using UIKit;

namespace MobileClaims.iOS
{
	public class TextFieldWithoutLabel : UIView
	{
		private const float TEXT_FIELD_PADDING = 5;

		public UITextField textField;

		public TextFieldWithoutLabel (string textPlaceholder)
		{
			this.BackgroundColor = Colors.BACKGROUND_COLOR;
			this.Layer.CornerRadius = Constants.CORNER_RADIUS;

			this.textField = new UITextField ();
			this.textField.Placeholder = textPlaceholder;
			this.textField.VerticalAlignment = UIControlContentVerticalAlignment.Center;
			this.textField.KeyboardType = UIKeyboardType.Default;
			this.textField.AutocorrectionType = UITextAutocorrectionType.No;
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
			this.textField.Frame = new CGRect (TEXT_FIELD_PADDING, 0, (float)this.Frame.Width - TEXT_FIELD_PADDING * 2, (float)this.Frame.Height);
		}
	}
}

