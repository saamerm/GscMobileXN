using System;
using UIKit;
using Cirrious.FluentLayouts.Touch;

namespace MobileClaims.iOS
{
    /// <summary>
    /// copy from ClaimDetailsToggleSubComponent and change to using fluentlayout
    /// </summary> 
    public class ClaimHistoryToggleSubComponent : ClaimDetailsSubComponent
    {  
        public ClaimForTxtLabel typeName;
        public UISwitch toggleSwitch;
        public UIView switchContainer; 
        public float componetHeight=50f;
        private float swithHeight=30f;
        private float typeNameWidthPercent=0.83f;  
        private float topMargin=10f;

        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler VisibilityToggled;

        protected string _titleString;

        protected UIView disableOverlayView;

        protected bool _isEnabled = true;


        public ClaimHistoryToggleSubComponent(string  benefitName)
        {    
            this.UserInteractionEnabled = true; 
            typeName = new ClaimForTxtLabel();
            typeName.Text = benefitName;
            Add(typeName);

            switchContainer = new UIView();
            switchContainer.UserInteractionEnabled = true; 
            Add(switchContainer);

            toggleSwitch = new UISwitch();
            toggleSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            toggleSwitch.ValueChanged += HandleSwitch;
            toggleSwitch.UserInteractionEnabled = true;
            Add(toggleSwitch);

            errorButton = new UIButton ();
            errorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            errorButton.AdjustsImageWhenHighlighted = true;
            this.AddSubview (errorButton);

            disableOverlayView = new UIView ();
            disableOverlayView.BackgroundColor = Colors.LightGrayColor;
            disableOverlayView.Alpha = 0.5f;
           // switchContainer.AddSubview(toggleSwitch); 

            this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            switchContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            this.AddConstraints(
                this.Height().EqualTo(componetHeight),

                typeName.AtTopOf(this, topMargin),
                typeName.WithSameLeft(this),
                typeName.WithRelativeWidth(this, typeNameWidthPercent),
                typeName.Height().EqualTo(swithHeight),

//                switchContainer.WithSameCenterY(typeName),//these code will cause the toggleswith not interactive with user
//                switchContainer.WithRelativeWidth(this, (1 - typeNameWidthPercent)).Minus(2f),
//                switchContainer.ToRightOf(typeName),
//
//                toggleSwitch.WithSameCenterY(switchContainer),  
//                toggleSwitch.Height().EqualTo(swithHeight), 
//                toggleSwitch.Right().EqualTo().RightOf(switchContainer),
//                toggleSwitch.Width().EqualTo(50f)
//                

                toggleSwitch.AtTopOf(this,topMargin), 
                toggleSwitch.Width().EqualTo(50f),//must use fixed with instead of percentage
                toggleSwitch.Height().EqualTo (swithHeight), 
                toggleSwitch.AtRightOf(this,2f)//must has 2f adjustment 

               
            );

            hideError ();
          
        }


        public void setIsEnabled (bool isEnabled, bool animated)
        {
            float animationDuration = animated ? Constants.TOGGLE_ANIMATION_DURATION : 0;

            if (_isEnabled == isEnabled)
                return;

            _isEnabled = isEnabled;

            if (isEnabled) {
                UIView.Animate (Constants.TOGGLE_ANIMATION_DURATION, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
                    () => {
                    disableOverlayView.Alpha = 0;},
                    () => {
                    disableOverlayView.RemoveFromSuperview ();
                }
                );

                toggleSwitch.UserInteractionEnabled = errorButton.UserInteractionEnabled = true;
            } else {
                disableOverlayView.Alpha = 0;
                this.AddSubview (disableOverlayView);
                UIView.Animate (animationDuration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
                    () => {
                    disableOverlayView.Alpha = 0.5f;},
                    () => {

                }
                );
                toggleSwitch.UserInteractionEnabled = errorButton.UserInteractionEnabled = false;
            }
        }

        public void showError (string error_string =  null)
        {
            if (error_string != null)
                ErrorString = error_string;

            if(titleLabel != null)
                titleLabel.TextColor = Colors.ERROR_COLOR;

            errorButton.Alpha = 1;
            errorButton.UserInteractionEnabled = true;
        }

        public void hideError()
        {
            if (titleLabel != null)
                titleLabel.TextColor = Colors.DARK_GREY_COLOR;

            errorButton.Alpha = 0;
            errorButton.UserInteractionEnabled = false;
        }
        void HandleSwitch (object sender, EventArgs e)
        { 
            if (this.VisibilityToggled != null)
            {
                this.VisibilityToggled(this, EventArgs.Empty);
            }
        }  
    }
}

