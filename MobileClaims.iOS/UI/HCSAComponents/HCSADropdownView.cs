using System;
using UIKit;
using CoreGraphics;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Platforms.Ios.Views;

namespace MobileClaims.iOS
{
	public class HCSADropdownView: UIView
	{
		public UILabel selectedTypeLabel;
		public ClaimListModal listController;//just a tableview with done button
		public UIButton btnOnClickTypes; 
		private MvxViewController _parentController;
		public UIPopoverController popoverController;//ClaimDetailsListSubComponent


		private float COMPONENT_WIDTH = 320;
        private float _cOMPONENT_HEIGHT = 280;
        public float COMPONENT_HEIGHT
        {
            get
            {
                return _cOMPONENT_HEIGHT;
            }
            set
            {
                _cOMPONENT_HEIGHT = value; 
            }
        }

			public HCSADropdownView (MvxViewController parentController)
		{
			_parentController = parentController;

			this.BackgroundColor = Colors.LightGrayColor;
			this.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;
			this.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;//

			selectedTypeLabel = new UILabel ();
			selectedTypeLabel.LineBreakMode = UILineBreakMode.WordWrap;
			selectedTypeLabel.Lines = 4;
			selectedTypeLabel.BackgroundColor = Colors.Clear;
			selectedTypeLabel.TextAlignment = UITextAlignment.Left;
			if(Constants.IsPhone())
			selectedTypeLabel.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.DROPDOWN_FONT_SIZE );
			else 
				selectedTypeLabel.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)Constants.DROPDOWN_FONT_SIZE-1);				
			selectedTypeLabel.TextColor = Colors.DARK_GREY_COLOR;
			AddSubview (selectedTypeLabel);

			btnOnClickTypes = new UIButton ();
			btnOnClickTypes.BackgroundColor = Colors.Clear;
			AddSubview (btnOnClickTypes);
			btnOnClickTypes.TouchUpInside += OnClickTypesButton;


			listController = new ClaimListModal ();
			listController.View = new UIView (new CGRect (0, 0, 5, 5));
			listController.View.BackgroundColor = Colors.LightGrayColor;

			listController.tableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			listController.tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
			listController.tableView.TableHeaderView = new UIView ();
			listController.tableView.SeparatorColor = Colors.Clear;
			listController.tableView.ShowsVerticalScrollIndicator = true;
			listController.View.AddSubview (listController.tableView);


		
			////
			if (Constants.IsPhone ()) {
				listController.dismissButton = new GSButton ();
				listController.dismissButton.SetTitle ("done".tr (), UIControlState.Normal);
				listController.View.AddSubview (listController.dismissButton);
				listController.dismissButton.TouchUpInside += HandleDismissButton;
			} else
			{
				
				popoverController = new UIPopoverController (listController);
			}
			////

			SetUpLayoutConstraints ();

		}

		public void setSelectedRow(NSIndexPath path) {

			if (listController != null && listController.tableView != null) {
                try
                {
                    listController.tableView.SelectRow(path, true, UITableViewScrollPosition.Middle);
                    if (listController.tableView.Source != null && listController.tableView.Source.GetType() == typeof(DismmssVCTableViewSource))
					{
					DismmssVCTableViewSource source = (DismmssVCTableViewSource)listController.tableView.Source;
						source.selectedRow = path;
					}
                }
                catch { }
			}
		}

		void SetUpLayoutConstraints()
		{
			this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			float padding = 10;
			this.AddConstraints(

				selectedTypeLabel.AtLeftOf(this,Constants.DROPDOWN_LABEL_PADDING),
				selectedTypeLabel.AtRightOf(this,Constants.DROPDOWN_LABEL_PADDING),
				selectedTypeLabel.AtTopOf(this, padding),
				selectedTypeLabel.AtBottomOf(this,padding),

				btnOnClickTypes.AtLeftOf(this, 0),
				btnOnClickTypes.AtRightOf(this, 0),
				btnOnClickTypes.AtTopOf(this, 0),
				btnOnClickTypes.AtBottomOf(this,0)

			);

			if (!Constants.IsPhone()) {
				listController.View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

//				listController.tableView.Frame = new CGRect(0,0, COMPONENT_WIDTH, COMPONENT_HEIGHT);
		
				listController.View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
				listController.View.AddConstraints (
					listController.tableView.AtLeftOf(listController.View, 0),
				listController.tableView.AtTopOf(listController.View, 0),
				listController.tableView.Height().EqualTo(COMPONENT_HEIGHT),
				listController.tableView.Width().EqualTo(COMPONENT_WIDTH)
				);
			}
		}

        public void ClosePopup()
        {
            listController.DismissViewController(true, (Action)null);
        }

		void OnClickTypesButton (object sender, EventArgs e)
		{
			
		// open pop up

			if (Constants.IsPhone ()) {

				UIWindow keyWindow = UIApplication.SharedApplication.KeyWindow;
				listController.ModalInPopover = true;
				listController.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
				keyWindow.RootViewController.PresentViewController((UIViewController)listController, true, (Action)null);


			} else {

					float adjustmentForParentHeight = 50;
					float popoverX = (float)(this.Frame.X + this.Frame.Width/2 + this.Frame.Width/4);
				float popoverY = (float)(this.Frame.Y + this.Frame.Height);//This is wont work for HCSA -> /2 + adjustmentForParentHeight);
				popoverController.SetPopoverContentSize( (CGSize)new CGSize(COMPONENT_WIDTH, COMPONENT_HEIGHT), false);
                popoverController.PresentFromRect((CGRect)new CGRect(popoverX, popoverY, 1, 1), (UIView)_parentController.View, (UIPopoverArrowDirection)UIPopoverArrowDirection.Up, true);
			
//			      	listController.tableView.Frame = new CGRect(0,0, COMPONENT_WIDTH, COMPONENT_HEIGHT);

			}
		}

		void HandleDismissButton (object sender, EventArgs e)
		{
			listController.DismissViewController (true, (Action)null);
		}
	}
}

