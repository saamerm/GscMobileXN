using System;
using CoreGraphics;
using System.Collections.Generic;
using CoreFoundation;
using UIKit;
using Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Binding.ValueConverters;
using Cirrious.CrossCore;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
	[Foundation.Register("LocateServiceProviderLocatedProvidersView")]
    public class LocateServiceProviderLocatedProvidersView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		protected UILabel[] labels;
		protected UILabel firstLabel;
		protected UILabel secondLabel;
		protected UILabel thirdLabel;
		protected UILabel rangeLabel;
		protected UILabel providerHeaderLabel;
		protected UIBarButtonItem doneButton;
		protected NSMutableArray buttonsArray;
		protected UITableView providerTableView;
		protected UIScrollView scrollContainer;
		//protected MiniTabBar tabBar;

		private MvxSubscriptionToken _selectedproviderchanged;
		private IMvxMessenger _messenger;

		private const float Y_ORIGIN = 100;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();
            View = new GSCBaseView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.Title = "findProvider".tr();
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;

			_messenger = Mvx.Resolve<IMvxMessenger>();

			LocateServiceProviderLocatedProvidersViewModel _model = (LocateServiceProviderLocatedProvidersViewModel)(this.ViewModel);

			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Constants.HIGHLIGHT_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Constants.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
			}

			doneButton = new UIBarButtonItem();
			doneButton.Style = UIBarButtonItemStyle.Plain;
			doneButton.Clicked += DoneClicked;
			doneButton.Title = "locateProvidersDone".tr();
			UITextAttributes attributes = new UITextAttributes ();
			attributes.Font = UIFont.FromName (Constants.AVENIR_STD_REGULAR, (nfloat)Constants.NAV_BAR_BUTTON_SIZE);
			doneButton.SetTitleTextAttributes (attributes, UIControlState.Normal);
			if (Constants.IsPhone())
				base.NavigationItem.RightBarButtonItem = doneButton;

			scrollContainer = new UIScrollView ();
			scrollContainer.BackgroundColor = Constants.BACKGROUND_COLOR;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);

			providerHeaderLabel = new UILabel();
			providerHeaderLabel.Text = "searchProviderSearchedLabel".tr ();
			providerHeaderLabel.TextColor = Constants.DARK_GREY_COLOR;
			providerHeaderLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.HEADING_FONT_SIZE);
			providerHeaderLabel.TextAlignment = UITextAlignment.Left;
			providerHeaderLabel.BackgroundColor = UIColor.Clear;
			providerHeaderLabel.Lines = 0;
			providerHeaderLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (providerHeaderLabel);

			firstLabel = new UILabel();
			firstLabel.TextColor =Constants.DARK_GREY_COLOR;
			firstLabel.TextAlignment = UITextAlignment.Left;
			firstLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
			firstLabel.BackgroundColor = UIColor.Clear;
			firstLabel.Lines = 0;
			firstLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (firstLabel);

			secondLabel = new UILabel();
			secondLabel.TextColor = Constants.DARK_GREY_COLOR;
			secondLabel.TextAlignment = UITextAlignment.Left;
			secondLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
			secondLabel.BackgroundColor = UIColor.Clear;
			secondLabel.Lines = 0;
			secondLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (secondLabel);

			thirdLabel = new UILabel();
			thirdLabel.TextColor = Constants.DARK_GREY_COLOR;
			thirdLabel.TextAlignment = UITextAlignment.Left;
			thirdLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
			thirdLabel.BackgroundColor = UIColor.Clear;
			thirdLabel.Lines = 0;
			thirdLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (thirdLabel);

			rangeLabel = new UILabel();
			rangeLabel.TextColor = Constants.DARK_GREY_COLOR;
			rangeLabel.TextAlignment = UITextAlignment.Left;
			rangeLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE);
			rangeLabel.BackgroundColor = UIColor.Clear;
			rangeLabel.Lines = 0;
			rangeLabel.LineBreakMode = UILineBreakMode.WordWrap;
			scrollContainer.AddSubview (rangeLabel);

			providerTableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			providerTableView.RowHeight = Constants.SINGLE_SELECTION_LOCATED_PROVIDERS_CELL_HEIGHT;
			providerTableView.TableHeaderView = new UIView ();
			providerTableView.SeparatorColor = UIColor.Clear;
			providerTableView.ShowsVerticalScrollIndicator = false;
			providerTableView.ScrollEnabled = false;
			MvxDeleteTableViewSource providerSource = new MvxDeleteTableViewSource (_model,providerTableView,"ProviderLocatedProvidersTableViewCell",typeof(ProviderLocatedProvidersTableViewCell));
			providerTableView.Source = providerSource;
			scrollContainer.AddSubview (providerTableView);

			var set = this.CreateBindingSet<LocateServiceProviderLocatedProvidersView, LocateServiceProviderLocatedProvidersViewModel> ();
			set.Bind (providerSource).To (vm => vm.ServiceProviders);
			if (Constants.IsPhone ()) {
				this.CreateBinding (providerSource).For (s => s.SelectionChangedCommand).To<LocateServiceProviderLocatedProvidersViewModel> (vm => vm.ShowMapViewCommand).Apply ();
			} else {
				this.CreateBinding (providerSource).For (s => s.SelectionChangedCommand).To<LocateServiceProviderLocatedProvidersViewModel> (vm => vm.ShowMapViewWithoutNavigatingForIOSCommand).Apply ();
			}
            set.Bind (this).For (v => v.SelectedProvider).To (vm => vm.SelectedProvider).Mode(Cirrious.MvvmCross.Binding.MvxBindingMode.OneWay);
			set.Apply ();

			providerTableView.ReloadData ();

			UpdateSearchLabels ();

            if (!Constants.IsPhone() && _model.ServiceProviders.Count > 0)
            {
                ServiceProvider serviceProvider = ((ServiceProvider)_model.ServiceProviders[0]);
                _model.ShowMapViewWithoutNavigatingCommand.Execute(serviceProvider);
                if (Constants.IS_OS_VERSION_OR_LATER(8, 0))
                    this.PerformSelector(new ObjCRuntime.Selector("selectRow"), null, 1.0);
                else
                    selectFirstRow();               
            } 
              

			if (!Constants.IsPhone ()) {
				_selectedproviderchanged = _messenger.Subscribe<Core.Messages.SelectedServiceProviderChanged>((message) =>
					{
						IProviderLookupService service = Mvx.Resolve<IProviderLookupService>();
						if(_model.SelectedProvider != service.SelectedServiceProvider && providerTableView != null)
						{
							_model.SelectedProvider = service.SelectedServiceProvider;
							NSIndexPath indexPath = NSIndexPath.FromRowSection ((nint)_model.ServiceProviders.IndexOf(_model.SelectedProvider), (nint)0);
							providerTableView.SelectRow (indexPath, true, UITableViewScrollPosition.Top );
						}
					});
			}

		}

        [Export("selectRow")]
        void selectFirstRow()
        {
            NSIndexPath indexPath = NSIndexPath.FromRowSection(0, 0);
            providerTableView.SelectRow(indexPath, false, UITableViewScrollPosition.Top);            
        }  
		public override void ViewDidLayoutSubviews ()
		{
            base.ViewDidLayoutSubviews ();

            float startY = ViewContentYPositionPadding;
            //if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            //{
            //    startY = Constants.IsPhone() ? Constants.CLAIMS_DETAILS_COMPONENT_PADDING : Constants.NAV_BUTTON_SIZE_IPAD;
            //}
            //else
            //{
            //    startY = Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING; 
            //}

            float contentY = 0.0f;
            float contentWidth = ViewContainerWidth - Constants.CONTENT_SIDE_PADDING * 2;
            float padding = 15;


            float yPos = 0;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                yPos = startY;    
            }
            else
            {
                yPos = startY + Constants.DRUG_LOOKUP_TOP_PADDING;
            }

            LocateServiceProviderLocatedProvidersViewModel _model = (LocateServiceProviderLocatedProvidersViewModel)(this.ViewModel);

            scrollContainer.Frame = new CGRect (0, 0, ViewContainerWidth, ViewContainerHeight);

            providerHeaderLabel.Frame = new CGRect (Constants.CONTENT_SIDE_PADDING,yPos, contentWidth, (float)providerHeaderLabel.Frame.Height);
            providerHeaderLabel.SizeToFit ();
            yPos += (float)providerHeaderLabel.Frame.Height + 5;//  + (Constants.IS_OS_VERSION_OR_LATER(11, 0) ? startY : 0);

            firstLabel.Frame = new CGRect (Constants.CONTENT_SIDE_PADDING, yPos, contentWidth, (float)firstLabel.Frame.Height);
            firstLabel.SizeToFit ();
            yPos += (float)firstLabel.Frame.Height+ padding ;

            secondLabel.Frame = new CGRect (Constants.CONTENT_SIDE_PADDING, yPos, contentWidth, (float)secondLabel.Frame.Height);
            secondLabel.SizeToFit ();
            yPos += (float)secondLabel.Frame.Height + padding;

            thirdLabel.Frame = new CGRect (Constants.CONTENT_SIDE_PADDING, yPos, contentWidth, (float)thirdLabel.Frame.Height);
            thirdLabel.SizeToFit ();
            yPos += (float)thirdLabel.Frame.Height + padding;

            rangeLabel.Frame = new CGRect(Constants.CONTENT_SIDE_PADDING, yPos, contentWidth, (float)rangeLabel.Frame.Height);
            rangeLabel.SizeToFit ();
            yPos += (float)rangeLabel.Frame.Height;

            if (_model.ServiceProviders != null) {
                float listY = (float)rangeLabel.Frame.Y + Constants.SINGLE_SELECTION_TOP_TABLE_PADDING;
                float listHeight = _model.ServiceProviders.Count * (Constants.SINGLE_SELECTION_LOCATED_PROVIDERS_CELL_HEIGHT ) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;
                providerTableView.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, listY, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING*2, listHeight);
                yPos += listHeight;
            }

            scrollContainer.ContentSize = new CGSize (ViewContainerWidth, yPos + GetBottomPadding(Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING, 10));

		}
	
		private void UpdateSearchLabels()
		{
			var _model = (LocateServiceProviderLocatedProvidersViewModel)(this.ViewModel);
			switch(_model.SearchTerms.LocationType)
			{
			case "Address":
				firstLabel.Text = "locateProviderAddressLabel".tr () + "colon".tr() + " " + _model.SearchTerms.Address;
				break;
			case "Postal Code":
				firstLabel.Text = "locateProviderPostalCodeLabel".tr () + "colon".tr() + " " + _model.SearchTerms.PostalCode;
				break;
			default:
				firstLabel.Text = "locateProviderMyCurrentLocationLabel".tr ();
				break;
			}

			secondLabel.Text = "serviceProviderTypeLabel".tr() + "colon".tr() + " " + _model.SearchTerms.ProviderType;

			switch(_model.SearchTerms.SearchType) {
			case "Phone Number":
				thirdLabel.Text = "locateProviderPhoneNumberLabel".tr () + "colon".tr() + " " + _model.SearchTerms.Phone;
				break;
			case "Last Name":
				thirdLabel.Text = "locateProviderLastNameLabel".tr () + "colon".tr() + " " + _model.SearchTerms.LastName;
				break;
			case "Business Name":
				thirdLabel.Text = "locateProviderBusinessNameLabel".tr () + "colon".tr() + " " + _model.SearchTerms.BusinessName;
				break;
			case "City":
				thirdLabel.Text = "locateProviderCityLabel".tr () + "colon".tr() + " " + _model.SearchTerms.City;
				break;
			default:
				thirdLabel.Text = " "; //"locateProviderAllProvidersLabel".tr ();
				break;
			}

			rangeLabel.Text = "locateProviderRangeLabel".tr () + "colon".tr() + " " + _model.SearchTerms.Radius + "km";

			firstLabel.Text = firstLabel.Text.ToUpper ();
			secondLabel.Text = secondLabel.Text.ToUpper ();
			thirdLabel.Text = thirdLabel.Text.ToUpper ();
			rangeLabel.Text = rangeLabel.Text.ToUpper ();
			View.SetNeedsLayout ();
		}

		private void DoneClicked (object sender, EventArgs ea) {
			LocateServiceProviderResultViewModel _model = (LocateServiceProviderResultViewModel)(this.ViewModel);
			_model.DoneCommand.Execute(null);
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
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING + Constants.DRUG_LOOKUP_TOP_PADDING;
            }
        }

        private ServiceProvider _selectedProvider;
		public ServiceProvider SelectedProvider
		{
			get { return _selectedProvider; }
			set { 
				if (_selectedProvider != value) {
					_selectedProvider = value; 
				}

			}
		}

	}

	[Foundation.Register("ProviderLocatedProvidersTableViewCell")]
	public class ProviderLocatedProvidersTableViewCell : SingleSelectionThreeTitlesTableViewCell
	{
		public ProviderLocatedProvidersTableViewCell () : base () {}
		public ProviderLocatedProvidersTableViewCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ProviderLocatedProvidersTableViewCell,ServiceProvider>();
					set.Bind(this.label).To(item => item.DoctorName).WithConversion("StringCase").OneWay();
					set.Bind(this.label2).To(item => item.Address).WithConversion("StringCase").OneWay();
					set.Bind(this.label3).To(item => item.FormattedAddress).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}
}

