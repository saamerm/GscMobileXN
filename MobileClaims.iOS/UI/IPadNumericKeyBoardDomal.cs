using System;
using UIKit;
using Cirrious.FluentLayouts.Touch;

namespace MobileClaims.iOS
{
    public class IPadNumericKeyBoardDomal :UIViewController
    {  

        public string Result;
        public KeyBoardNoButton buttZero;
        public KeyBoardNoButton buttOne;
        public KeyBoardNoButton buttTwo;
        public KeyBoardNoButton buttThree;
        public KeyBoardNoButton buttFour;
        public KeyBoardNoButton buttFive;
        public KeyBoardNoButton buttSix;
        public KeyBoardNoButton buttSeven;
        public KeyBoardNoButton buttEight;
        public KeyBoardNoButton buttNine;
        public KeyBoardButtonGrey buttPeriod;
        public KeyBoardButtonGrey buttDelete; 
       
        private float _keyBoardWidth=400f;
        public float KeyBoardWidth 
        {
            get{ return _keyBoardWidth;}
            set{ _keyBoardWidth = value;}
        }

        private float _keyBoardHeight=300f;
        public float KeyBoardHeight
        {
            get{ return _keyBoardHeight;}
            set{ _keyBoardHeight = value; }
        }

        public delegate void EventHandler (string sender, EventArgs e);
        public EventHandler RaiseInputTextChange;  

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LayoutComponents();
            SetConstraints(); 

        } 

        private void LayoutComponents()
        {
            View.BackgroundColor = Colors.BACKGROUND_COLOR;
            buttOne = new KeyBoardNoButton();
            buttOne.SetTitle("1", UIControlState.Normal);
            buttOne.TouchUpInside += ButtonOne_TouchUpInside;
            View.AddSubview(buttOne);

            buttTwo = new KeyBoardNoButton();
            buttTwo.SetTitle("2", UIControlState.Normal);
            buttTwo.TouchUpInside +=ButtonTwo_TouchUpInside;
            View.AddSubview(buttTwo);

            buttThree = new KeyBoardNoButton();
            buttThree.SetTitle("3", UIControlState.Normal);
            buttThree.TouchUpInside +=ButtonThree_TouchUpInside;
            View.AddSubview(buttThree);

            buttFour = new KeyBoardNoButton();
            buttFour.SetTitle("4", UIControlState.Normal);
            buttFour.TouchUpInside +=ButtonFour_TouchUpInside;
            View.AddSubview(buttFour);

            buttFive = new KeyBoardNoButton();
            buttFive.SetTitle("5", UIControlState.Normal);
            buttFive.TouchUpInside +=ButtonFive_TouchUpInside;
            View.AddSubview(buttFive);

            buttSix = new KeyBoardNoButton();
            buttSix.SetTitle("6", UIControlState.Normal);
            buttSix.TouchUpInside +=ButtonSix_TouchUpInside;
            View.AddSubview(buttSix);

            buttSeven = new KeyBoardNoButton();
            buttSeven.SetTitle("7", UIControlState.Normal);
            buttSeven.TouchUpInside +=ButtonSeven_TouchUpInside;
            View.AddSubview(buttSeven);

            buttEight = new KeyBoardNoButton();
            buttEight.SetTitle("8", UIControlState.Normal);
            buttEight.TouchUpInside +=ButtonEight_TouchUpInside;
            View.AddSubview(buttEight);

            buttNine = new KeyBoardNoButton();
            buttNine.SetTitle("9", UIControlState.Normal);
            buttNine.TouchUpInside +=ButtonNine_TouchUpInside;
            View.AddSubview(buttNine);

            buttZero = new KeyBoardNoButton();
            buttZero.SetTitle("0", UIControlState.Normal);
            buttZero.TouchUpInside +=ButtonZero_TouchUpInside;
            View.AddSubview(buttZero);

            buttPeriod = new KeyBoardButtonGrey();
            buttPeriod.SetTitle(Constants.IPadNumericKeyBoardDot, UIControlState.Normal);
            buttPeriod.TouchUpInside +=ButtonPeriod_TouchUpInside;
            View.AddSubview(buttPeriod);

            buttDelete = new KeyBoardButtonGrey();
            buttDelete.SetTitle("delete", UIControlState.Normal);
            buttDelete.TouchUpInside +=ButtonDelete_TouchUpInside;
            View.AddSubview(buttDelete); 

        }

        private void SetConstraints()
        {
            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.AddConstraints(
                buttOne.AtTopOf (View),
                buttOne.AtLeftOf(View),
                buttOne.Width().EqualTo (KeyBoardWidth /3),
                buttOne.Height ().EqualTo (KeyBoardHeight /4),

                buttTwo.ToRightOf (buttOne),
                buttTwo.WithSameCenterY(buttOne),
                buttTwo.WithSameWidth(buttOne),
                buttTwo.WithSameHeight(buttOne), 

                buttThree.ToRightOf(buttTwo),
                buttThree.WithSameCenterY (buttTwo),
                buttThree.WithSameWidth(buttTwo),
                buttThree .WithSameHeight(buttTwo),

                buttFour.WithSameLeft(buttOne),
                buttFour.Below(buttOne),
                buttFour.WithSameWidth(buttOne),
                buttFour.WithSameHeight(buttOne),

                buttFive.ToRightOf(buttFour),
                buttFive.WithSameCenterY(buttFour),
                buttFive.WithSameWidth(buttFour),
                buttFive.WithSameHeight(buttFour),

                buttSix.ToRightOf (buttFive),
                buttSix.WithSameCenterY(buttFive),
                buttSix.WithSameWidth(buttFive),
                buttSix.WithSameHeight(buttFive),

                buttSeven.WithSameLeft(buttFour),
                buttSeven.Below(buttFour),
                buttSeven.WithSameWidth(buttFour),
                buttSeven.WithSameHeight(buttFour),

                buttEight.ToRightOf(buttSeven),
                buttEight.WithSameCenterY (buttSeven),
                buttEight.WithSameWidth(buttSeven),
                buttEight .WithSameHeight(buttSeven),

                buttNine.ToRightOf(buttEight),
                buttNine.WithSameCenterY (buttEight),
                buttNine.WithSameWidth(buttEight),
                buttNine .WithSameHeight(buttEight),

                buttPeriod.WithSameLeft(buttSeven),
                buttPeriod.Below(buttSeven),
                buttPeriod.WithSameWidth(buttSeven),
                buttPeriod.WithSameHeight(buttSeven),

                buttZero.ToRightOf(buttPeriod),
                buttZero.WithSameCenterY (buttPeriod),
                buttZero.WithSameWidth(buttPeriod),
                buttZero .WithSameHeight(buttPeriod),

                buttDelete.ToRightOf(buttZero),
                buttDelete.WithSameCenterY (buttZero),
                buttDelete.WithSameWidth(buttZero),
                buttDelete .WithSameHeight(buttZero) 
            );
        }

        #region private helper methods---button click

        private void ButtonOne_TouchUpInside(object sender, EventArgs e)
        {  
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("1", e);
        } 
        
        private void ButtonTwo_TouchUpInside(object sender, EventArgs e)
        {
             if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("2", e);
        }

        private void ButtonThree_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("3", e);
        }

        private void ButtonFour_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("4", e);
        }

        private void ButtonFive_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("5", e);
        }

        private void ButtonSix_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("6", e);
        }

        private void ButtonSeven_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("7", e);
        }

        private void ButtonEight_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("8", e);
        }

        private void ButtonNine_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("9", e);
        }

        private void ButtonZero_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("0", e);
        }

        private void ButtonDelete_TouchUpInside(object sender, EventArgs e)
        { 
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange("-", e);
        }

        private void ButtonPeriod_TouchUpInside(object sender, EventArgs e)
        {
            if(this.RaiseInputTextChange !=null)
                RaiseInputTextChange(Constants.IPadNumericKeyBoardDot, e);
        }
        #endregion
    }
}

