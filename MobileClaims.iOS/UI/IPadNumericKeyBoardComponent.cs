using System;
using UIKit;
using CoreGraphics;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Platforms.Ios.Views;


namespace MobileClaims.iOS
{
    public class IPadNumericKeyBoardComponent: UIView
    {
        private UIPopoverController popoverController;
        private MvxViewController parentController;

        public UILabel InputText;
        public IPadNumericKeyBoardDomal NumericKeyBoard;

        public IPadNumericKeyBoardComponent(MvxViewController parent)
        {
            parentController = parent;

            InputText = new UILabel();
            InputText.TextColor = Colors.DARK_GREY_COLOR; 
            InputText.Layer.BorderColor = Colors.DARK_GREEN_COLOR.CGColor;
            InputText.Layer.BorderWidth = 1f; 
            InputText.BackgroundColor = Colors.LightGrayColor;
            this.AddSubview(InputText);

            NumericKeyBoard = new IPadNumericKeyBoardDomal(); 
            NumericKeyBoard.RaiseInputTextChange  += NumericKeyBoard_InputEvent;

            this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            this.AddConstraints(
                InputText.AtTopOf(this),
                InputText.AtLeftOf(this),
                InputText.Width().EqualTo(150f),
                InputText.Height().EqualTo(30f)  
            );
        
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            if (!Constants.IsPhone())
            {
                if(popoverController ==null|| popoverController .ContentViewController ==null)
                {
                    popoverController = new UIPopoverController(NumericKeyBoard);
             
                }
                float scrollOffset = 0;
                float scrollStartY = 0;
                if(parentController != null && parentController.View != null && parentController.View.GetType() == typeof(GSCFluentLayoutBaseView))
                {
                    scrollOffset = (float)((GSCFluentLayoutBaseView)parentController.View).baseScrollContainer.ContentOffset.Y;
                    scrollStartY = (float)((GSCFluentLayoutBaseView)parentController.View).baseScrollContainer.Frame.Y; 
                }

                float popoverX = (float)this.Frame.X + (float)this.Frame.Width/2 + (float)this.Frame.Width/4;
                float popoverY = (float)this.Frame.Y + (float)this.Frame.Height+ scrollStartY- scrollOffset;
                popoverController.SetPopoverContentSize(new CGSize(NumericKeyBoard.KeyBoardWidth, NumericKeyBoard.KeyBoardHeight), false);
                popoverController.PresentFromRect(new CGRect(popoverX, popoverY,1,1), parentController.View, UIPopoverArrowDirection.Up, true);

            }
            base.TouchesBegan(touches, evt);
        }


        private void NumericKeyBoard_InputEvent(string sender, EventArgs e)
        {
            if (sender != null)
            {
                if (!(sender == "-"))
                {  
                    string currentText = (InputText.Text == null) ? string.Empty : InputText.Text;
                    if (!string.IsNullOrEmpty(currentText))
                    {  
                        if (sender != Constants.IPadNumericKeyBoardDot)
                        {
                            InputText.Text += sender;
                        }
                        else
                        {
                            if (!currentText.Contains(Constants.IPadNumericKeyBoardDot))
                                InputText.Text += sender;
                        }
                    }
                    else
                    {
                        InputText.Text += sender;
                    }
                }
                else
                {
                    string Result = InputText.Text;
                    if (!string.IsNullOrEmpty(Result))
                    {
                        int longOfStr = Result.Length;
                        if (longOfStr > 0)
                        {
                            Result = Result.Substring(0, longOfStr - 1);
                        } 
                    }
                    InputText.Text = Result;
                } 
            }
        }
    }
}

