using System;
using UIKit;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;

namespace MobileClaims.iOS
{
    public class ProvinceListSubComponent : UIView
    {
        public UIPopoverController popoverController;
        private MvxViewController _parentController;

        public UILabel detailsLabel;
        public UIView listContainerBackground;

        private float COMPONENT_WIDTH = 200;
        private float COMPONENT_HEIGHT = 200;
        private float _labelLeftMargin = 10f;
        public float LabelLeftMargin
        {
            get { return _labelLeftMargin; }
            set
            {
                _labelLeftMargin = value;
            }
        }
        private float _labelHeight = 40f;
        public float LabelHeight
        {
            get { return _labelHeight; }
            set
            {
                _labelHeight = value;
            }
        }

        public ClaimListModal listController;//just a tableview with done button   

        public ProvinceListSubComponent(MvxViewController parentController, string labelPlaceHolder)
        {
            this.BackgroundColor = Colors.BACKGROUND_COLOR;

            _parentController = parentController;

            listController = new ClaimListModal();

            listController.View = new UIView(new CGRect(0, 0, 5, 5));
            listController.View.BackgroundColor = Colors.LightGrayColor;

            listContainerBackground = new UIView();
            listContainerBackground.BackgroundColor = Colors.LightGrayColor;
            listContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            listContainerBackground.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            this.AddSubview(listContainerBackground);

            detailsLabel = new UILabel();
            detailsLabel.BackgroundColor = Colors.Clear;
            detailsLabel.TextAlignment = UITextAlignment.Left;
            detailsLabel.Text = labelPlaceHolder;
            detailsLabel.TextColor = UIColor.Red;
            AddSubview(detailsLabel);

            listController.tableView = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Plain);
            listController.tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
            listController.tableView.TableHeaderView = new UIView();
            listController.tableView.SeparatorColor = Colors.Clear;
            listController.tableView.ShowsVerticalScrollIndicator = true;

            listController.View.AddSubview(listController.tableView);

            if (Constants.IsPhone())
            {
                listController.dismissButton = new GSButton();
                listController.dismissButton.SetTitle("done".tr(), UIControlState.Normal);
                listController.View.AddSubview(listController.dismissButton);
                listController.dismissButton.TouchUpInside += HandleDismissButton;
            }
            else
            {
                popoverController = new UIPopoverController(listController);
            }

        }

        void HandleDismissButton(object sender, EventArgs e)
        {
            listController.DismissViewController(true, (Action)null);
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            CGPoint newPoint = (CGPoint)((touches.AnyObject as UITouch).LocationInView(this));

            setHighlighted(false);
            if (Constants.IsPhone())
            {
                UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow; //on clicking popup button

                listController.ModalInPopover = true;
                listController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
                keyWindow.RootViewController.PresentViewController((UIViewController)listController, true, (Action)null);

            }
            else
            {
                try
                {
                    float adjustmentForParentHeight = 50;
                    float popoverX = (float)(this.Frame.X + this.Frame.Width / 2 + this.Frame.Width / 4);
                    float popoverY = (float)(this.Frame.Y + this.Frame.Height / 2 + adjustmentForParentHeight * 2);

                    popoverController.SetPopoverContentSize((CGSize)new CGSize(listController.tableView.Frame.Width, COMPONENT_HEIGHT), false);
                    popoverController.PresentFromRect((CGRect)new CGRect(popoverX, popoverY, 1, 1), (UIView)_parentController.View, UIPopoverArrowDirection.Any, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            float contentWidth = (float)this.Frame.Width;
            listContainerBackground.Frame = new CGRect(0, 0, contentWidth, LabelHeight);
            if (!Constants.IsPhone())
            {
                listController.tableView.Frame = new CGRect(0, 0, COMPONENT_WIDTH, COMPONENT_HEIGHT);
            }
            float detailsX = (float)listContainerBackground.Frame.X;

            detailsLabel.Frame = new CGRect(LabelLeftMargin, 0, contentWidth, LabelHeight);
        }

        public void setHighlighted(bool isHighlighted)
        {
            if (isHighlighted)
            {
                listContainerBackground.Layer.BorderColor = Colors.ERROR_COLOR.CGColor;
            }
            else
            {
                listContainerBackground.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
            }
        }
    }
}

