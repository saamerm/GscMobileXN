using System;
using CoreGraphics;
using System.Collections.Generic;
using System.Windows.Input;
using CoreFoundation;
using UIKit;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using Cirrious.MvvmCross.Touch.Views;
//using Cirrious.MvvmCross.Dialog.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MapKit;
using CoreLocation;
using MobileClaims.Core;
using MobileClaims.iOS.Views;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
	[Foundation.Register("LocateServiceProviderChooseSearchTypeView")]
    public class LocateServiceProviderChooseSearchTypeView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		private LocateServiceProviderChooseSearchTypeViewModel _model;

		protected UIScrollView scrollContainer;
		protected UITableView tableView;
		protected UITableView locationTypeTableView;

		protected UILabel locationScrollLabel, makeSelectionLabel;
		protected UILabel searchScrollLabel,narrowLabel;
		protected GSButton continueButton;

		private float BUTTON_WIDTH = Constants.IsPhone() ? 225 : 250;
		private float BUTTON_HEIGHT = Constants.IsPhone() ? 40 : 50;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
            View = new GSCBaseView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.Title = "findProvider".tr();
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Constants.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Constants.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
			}

			scrollContainer = new UIScrollView ();
			scrollContainer.BackgroundColor = Constants.BACKGROUND_COLOR;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

			locationScrollLabel = new UILabel();
			locationScrollLabel.Text = "searchBy".tr()+"colon".tr();
			locationScrollLabel.TextColor = Constants.DARK_GREY_COLOR;
			locationScrollLabel.TextAlignment = UITextAlignment.Left;
			locationScrollLabel.Font =UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
			locationScrollLabel.BackgroundColor = UIColor.Clear;
			scrollContainer.AddSubview (locationScrollLabel);
		
			makeSelectionLabel = new UILabel();
			makeSelectionLabel.Text = "makeYourSelection".tr();
			makeSelectionLabel.TextColor = Constants.DARK_GREY_COLOR;
			makeSelectionLabel.TextAlignment = UITextAlignment.Left;
			makeSelectionLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			makeSelectionLabel.BackgroundColor = UIColor.Clear;
			scrollContainer.AddSubview (makeSelectionLabel);

			searchScrollLabel = new UILabel();
			searchScrollLabel.Text = "narrowYourSearch".tr() + "colon".tr();
			searchScrollLabel.TextColor = Constants.DARK_GREY_COLOR;
			searchScrollLabel.TextAlignment = UITextAlignment.Left;
			searchScrollLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.LG_HEADING_FONT_SIZE);
			searchScrollLabel.BackgroundColor = UIColor.Clear;
			scrollContainer.AddSubview (searchScrollLabel);

			narrowLabel = new UILabel();
			narrowLabel.Text = "optional".tr(); 
			narrowLabel.TextColor = Constants.DARK_GREY_COLOR;
			narrowLabel.TextAlignment = UITextAlignment.Left;
			narrowLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			narrowLabel.BackgroundColor = UIColor.Clear;
			scrollContainer.AddSubview (narrowLabel);

			continueButton = new GSButton ();
			continueButton.SetTitle ("continue".tr ().ToUpper(), UIControlState.Normal);
			if (Constants.IsPhone())
				scrollContainer.AddSubview (continueButton);


			tableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			tableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
			tableView.TableHeaderView = new UIView ();
			tableView.SeparatorColor = UIColor.Clear;
			tableView.ShowsVerticalScrollIndicator = true;
			MvxDeleteTableViewSource providerSource = new MvxDeleteTableViewSource (_model,tableView,"ProviderChooseSearchTypeTableViewCell",typeof(ProviderChooseSearchTypeTableViewCell));
			tableView.Source = providerSource;
			scrollContainer.AddSubview (tableView);

			locationTypeTableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			locationTypeTableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
			locationTypeTableView.TableHeaderView = new UIView ();
			locationTypeTableView.SeparatorColor = UIColor.Clear;
			locationTypeTableView.ShowsVerticalScrollIndicator = true;
			MvxDeleteTableViewSource locationSource = new MvxDeleteTableViewSource (_model,locationTypeTableView,"ProviderChooseLocationTypeTableViewCell",typeof(ProviderChooseLocationTypeTableViewCell));
			locationTypeTableView.Source = locationSource;
			scrollContainer.AddSubview (locationTypeTableView);

			//bindings
			var set = this.CreateBindingSet<LocateServiceProviderChooseSearchTypeView, LocateServiceProviderChooseSearchTypeViewModel> ();
			set.Bind (providerSource).To (vm => vm.SearchTypes);  
			set.Bind (locationSource).To (vm => vm.LocationTypes);
			this.CreateBinding (providerSource).For (s => s.SelectionChangedCommand).To<LocateServiceProviderChooseSearchTypeViewModel> (vm => vm.SetSearchTypeCommand).Apply ();
			this.CreateBinding (locationSource).For (lts => lts.SelectionChangedCommand).To<LocateServiceProviderChooseSearchTypeViewModel> (vm => vm.SetLocationTypeCommand).Apply ();
			if (Constants.IsPhone ())
				set.Bind (continueButton).To (vm => vm.FindProviderCommand);

			set.Bind (providerSource.SelectedItem).To (vm => vm.SelectedSearchType).TwoWay ();
			set.Bind (locationSource.SelectedItem).To (vm => vm.SelectedLocationType).TwoWay ();
			set.Apply ();
			//end bindings

			_model = (LocateServiceProviderChooseSearchTypeViewModel)ViewModel;

			tableView.ReloadData ();
			tableView.SelectRow (NSIndexPath.FromRowSection((nint)0,(nint)0), false, UITableViewScrollPosition.Top);
			_model.SelectedSearchType = _model.SearchTypes [0];

			locationTypeTableView.ReloadData ();
			locationTypeTableView.SelectRow (NSIndexPath.FromRowSection((nint)0,(nint)0), false, UITableViewScrollPosition.Top);
			_model.SelectedLocationType = _model.LocationTypes [0];

			if (!Constants.IsPhone ())
				_model.ShowLocateServiceProvider ();
		}

		bool hasAppeared = false;
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (!Constants.IsPhone() && hasAppeared)
            {
                SplitViewController svc = (SplitViewController)base.SplitViewController;
                UINavigationController nc = svc.getRightPane();
                while (nc.TopViewController.GetType() != typeof(LocateServiceProviderView))
                {
                    if ((nc.ViewControllers[nc.ViewControllers.Length - 2].GetType() == typeof(LocateServiceProviderView)) && !Constants.IS_OS_VERSION_OR_LATER(8, 0))
                    {
                        nc.PopViewController(true);
                    }
                    else
                    {
                        nc.PopViewController(false);
                    }
                }
                View.SetNeedsLayout();
            }
            hasAppeared = true;
        }

		private bool hasSelected = false;
		public override void ViewDidLayoutSubviews ()
		{
            base.ViewDidLayoutSubviews();

            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float startY = ViewContentYPositionPadding;
            // float extraPos = Constants.IS_OS_7_OR_LATER () ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            //float viewWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
			float contentWidth = ViewContainerWidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
	
			//position location list content
			locationScrollLabel.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING+10, Constants.DRUG_LOOKUP_TOP_PADDING + startY, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			makeSelectionLabel.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING+10, (float)locationScrollLabel.Frame.Y + (float)locationScrollLabel.Frame.Height, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			float listY = (float)makeSelectionLabel.Frame.Y + Constants.SINGLE_SELECTION_TOP_TABLE_PADDING;
			float listHeight = _model.LocationTypes.Count * (Constants.SINGLE_SELECTION_CELL_HEIGHT + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING);
			locationTypeTableView.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, listY, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING*2, listHeight);

			//position search list content
			searchScrollLabel.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING+10, (float)locationTypeTableView.Frame.Y + (float)locationTypeTableView.Frame.Height + 20, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			narrowLabel.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING+10, (float)searchScrollLabel.Frame.Y + (float)searchScrollLabel.Frame.Height, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			listHeight = _model.SearchTypes.Count * Constants.SINGLE_SELECTION_CELL_HEIGHT  + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;
			tableView.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, (float)narrowLabel.Frame.Y + Constants.DRUG_LOOKUP_LABEL_HEIGHT, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING*2, listHeight);

            continueButton.Frame = new CGRect (ViewContainerWidth/2 - BUTTON_WIDTH/2, (float)tableView.Frame.Height + (float)tableView.Frame.Y + 15, BUTTON_WIDTH, BUTTON_HEIGHT);
            scrollContainer.Frame = new CGRect (0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize(ViewContainerWidth, (float)continueButton.Frame.Y + (float)continueButton.Frame.Height + GetBottomPadding(Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING, 15));
		
			if (tableView.VisibleCells.Length > 0 && !hasSelected) {
				locationTypeTableView.CellAt (NSIndexPath.FromRowSection ((nint)0, (nint)0)).SetSelected (true, false);
				tableView.CellAt (NSIndexPath.FromRowSection ((nint)0, (nint)0)).SetSelected (true, false);
				hasSelected = true;
			}
		
		}

        public float GetViewContainerWidth()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Width;    
            }
            else
            {
                return (float)base.View.Bounds.Width;
            }
        }

        public float GetViewContainerHeight()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            }
            else
            {
                return (float)base.View.Bounds.Height - Helpers.BottomNavHeight();
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            }
            else
            {
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            }
        }
    }

	[Foundation.Register("ProviderChooseSearchTypeTableViewCell")]
	public class ProviderChooseSearchTypeTableViewCell : SingleSelectionTableViewCell
	{
		public ProviderChooseSearchTypeTableViewCell () : base () {}
		public ProviderChooseSearchTypeTableViewCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ProviderChooseSearchTypeTableViewCell,SearchType>();
					set.Bind(this.label).To(item => item.TypeName).WithConversion("ProviderSearchType");
					set.Apply();
				});
		}
	}

	[Foundation.Register("ProviderChooseLocationTypeTableViewCell")]
	public class ProviderChooseLocationTypeTableViewCell : SingleSelectionTableViewCell
	{
		public ProviderChooseLocationTypeTableViewCell () : base () {}
		public ProviderChooseLocationTypeTableViewCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ProviderChooseLocationTypeTableViewCell,LocationType>();
					set.Bind(this.label).To(item => item.TypeName).WithConversion("ProviderSearchType");
					set.Apply();
				});
		}
	}
}

