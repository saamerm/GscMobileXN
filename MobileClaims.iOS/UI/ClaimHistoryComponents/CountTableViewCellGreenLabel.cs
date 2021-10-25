using System;
using UIKit; 

namespace MobileClaims.iOS
{
    public class CountTableViewCellGreenLabel : UILabel
    {
		public bool setToParent = false;
        private float cornerRadiusValue;
        private bool _isButtSelected;
        public bool IsButtSelected
        {
            get
            { 
                return _isButtSelected;
            }
            set
            {
                _isButtSelected = value;
                SetHighlight();
            }
        }
        
        private bool _isButtSelectable;
        public bool IsButtSelectable
         {
             get
            { 
                return _isButtSelectable;
            }
            set
            {
                _isButtSelectable = value;
                SetHighlight ();
            }
        } 
        public CountTableViewCellGreenLabel()
        { 
            SetConerRadiusValue();
            this.Layer.MasksToBounds =true; 
            this.Layer.CornerRadius = cornerRadiusValue;
			this.Lines = 2;
            this.TextAlignment = UITextAlignment.Center;
            this.Font=UIFont.FromName (Constants.NUNITO_SEMIBOLD, (nfloat)Constants.SMALL_FONT_SIZE);
            this.Layer.BorderWidth = 1f;
           // this.TitleEdgeInsets = new UIEdgeInsets (0, 1, 0, 0); 
        } 

        private void SetConerRadiusValue()
        {
            cornerRadiusValue = 9f;
            string currLang = System.Globalization.CultureInfo.CurrentUICulture.Name.ToString();

            if (currLang.Contains("fr") || currLang.Contains("Fr"))
            {
                if (Helpers.IsInPortraitMode())
                    cornerRadiusValue = 17f;
            }
        }

        public void SetHighlight()
        {
            if (IsButtSelected) {
                this.BackgroundColor = Colors.BACKGROUND_COLOR;
                this.TextColor = Colors.Black;
                this.Layer.BorderColor = Colors.Clear.CGColor;
            } 
            else
            {
                if (IsButtSelectable)
                {
                    this.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                    this.TextColor = Colors.BACKGROUND_COLOR;
                    this.Layer.BorderColor = Colors.Clear.CGColor;
                }
                else
                {
                    this.BackgroundColor =  Colors.VeryLightGrayColor; 
                    this.TextColor = Colors.DARK_GREY_COLOR;  
                    this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
                }
            }
        }  
    }
}

