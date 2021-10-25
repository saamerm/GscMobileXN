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
using Cirrious.MvvmCross.Binding.Touch.Views;
using MobileClaims.iOS.Views;
using MobileClaims.iOS.UI;

namespace MobileClaims.iOS
{
	[Foundation.Register("ProviderLookupProviderTypeView")]
    public class ProviderLookupProviderTypeView : GSCBaseViewPaddingController, IGSCBaseViewImplementor
	{
		protected UIScrollView scrollContainer;
		protected UITableView providerTableView;
		protected UIScrollView providerScrollView;
		protected UILabel drugLookupModelSelectionLabel;
		protected UILabel providerLabel;

		private const float Y_ORIGIN = 100;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad ();

			ProviderLookupProviderTypeViewModel _model = (ProviderLookupProviderTypeViewModel)ViewModel;

			View = new GSCBaseView() { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationController.NavigationBarHidden = false;
			base.NavigationItem.Title = "locateProviders".tr();
			base.NavigationItem.HidesBackButton = true;
			if(Constants.IS_OS_7_OR_LATER())
				this.AutomaticallyAdjustsScrollViewInsets = false;
				

			if (Constants.IS_OS_7_OR_LATER()) {
				base.NavigationController.NavigationBar.TintColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
				base.NavigationController.View.BackgroundColor = Constants.BACKGROUND_COLOR;
			} else {
				base.NavigationController.NavigationBar.BackgroundColor = Constants.BACKGROUND_COLOR;
			}

			scrollContainer = new UIScrollView ();
			scrollContainer.BackgroundColor = Constants.BACKGROUND_COLOR;
            ((GSCBaseView)View).baseContainer.AddSubview(scrollContainer);


			providerTableView = new UITableView(new CGRect(0,0,0,0), UITableViewStyle.Plain);
			providerTableView.RowHeight = Constants.SINGLE_SELECTION_CELL_HEIGHT;
			providerTableView.TableHeaderView = new UIView ();
			providerTableView.SeparatorColor = UIColor.Clear;
			providerTableView.ShowsVerticalScrollIndicator = true;
			MvxDeleteTableViewSource providerSource = new MvxDeleteTableViewSource (_model,providerTableView,"ProviderLookupProviderTypeTableViewCell",typeof(ProviderLookupProviderTypeTableViewCell));
			providerTableView.Source = providerSource;
			scrollContainer.AddSubview (providerTableView);

			var set = this.CreateBindingSet<ProviderLookupProviderTypeView, ProviderLookupProviderTypeViewModel> ();
			set.Bind (providerSource).To (vm => vm.ServiceProviderTypes);
			this.CreateBinding (providerSource).For (s => s.SelectionChangedCommand).To<ProviderLookupProviderTypeViewModel> (vm => vm.NextStepCommand).Apply ();
			set.Apply ();

			providerTableView.ReloadData ();

//			drugLookupModelSelectionLabel = new UILabel();
//			drugLookupModelSelectionLabel.Text = "drugLookupModelSelectionLabel".tr();
//			drugLookupModelSelectionLabel.BackgroundColor = UIColor.Clear;
//			drugLookupModelSelectionLabel.TextColor = UIColor.DarkGray;
//			drugLookupModelSelectionLabel.TextAlignment = UITextAlignment.Left;
//			drugLookupModelSelectionLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, Constants.LG_HEADING_FONT_SIZE);
//			scrollContainer.AddSubview(drugLookupModelSelectionLabel);

			providerLabel = new UILabel();
			providerLabel.Text = "makeYourSelection".tr() + "colon".tr();
			providerLabel.TextColor = Constants.DARK_GREY_COLOR;
			providerLabel.Font = UIFont.FromName (Constants.AVENIR_STD_ROMAN, (nfloat)Constants.SUB_HEADING_FONT_SIZE );
			providerLabel.TextAlignment = UITextAlignment.Left;
			providerLabel.BackgroundColor = UIColor.Clear;
			scrollContainer.AddSubview (providerLabel);

			_model.PropertyChanged += (sender, args) =>
			{
				switch (args.PropertyName)
				{
				case "ServiceProviderTypes":
					SetFrames();
					break;
				default:
					break;
				}
			};
		}

		public override void ViewDidLayoutSubviews ()
		{
            base.ViewDidLayoutSubviews();
			SetFrames ();
		}

		bool hasAppeared = false;
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (!Constants.IsPhone () && hasAppeared) {
				SplitViewController svc = (SplitViewController)base.SplitViewController;
				UINavigationController nc = svc.getRightPane ();
				while (nc.TopViewController.GetType () != typeof(SimpleMessageView)) {

					if( nc.ViewControllers[ nc.ViewControllers.Length -2].GetType() == typeof(SimpleMessageView)){
						nc.PopViewController (true);
					}else{
						nc.PopViewController (false);
					}

				}
			}

			hasAppeared = true;
		}

		private void SetFrames()
		{
			ProviderLookupProviderTypeViewModel _model = (ProviderLookupProviderTypeViewModel)ViewModel;
			//float startY = base.NavigationController.NavigationBar.Frame.Height + 20;
			//float startY = Constants.IS_OS_7_OR_LATER () ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            //float viewWidth = (float)((GSCBaseView)View).baseContainer.Bounds.Width;
            //float viewHeight = (float)((GSCBaseView)View).baseContainer.Bounds.Height - Helpers.BottomNavHeight();
            float itemPadding = Constants.CLAIMS_DETAILS_COMPONENT_PADDING;
            float yPos = ViewContentYPositionPadding;
            //float extraPos = startY;

			//drugLookupModelSelectionLabel.Frame = new RectangleF (Constants.BUTTON_SIDE_PADDING, startY, viewWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

			//yPos += Constants.DRUG_LOOKUP_LABEL_HEIGHT;
            providerLabel.Frame = new CGRect (Constants.BUTTON_SIDE_PADDING, yPos, ViewContainerWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);

			yPos += Constants.SINGLE_SELECTION_TOP_TABLE_PADDING;
			if (_model.ServiceProviderTypes != null) {
				float listHeight = _model.ServiceProviderTypes.Count * (Constants.SINGLE_SELECTION_CELL_HEIGHT ) + Constants.SINGLE_SELECTION_VERTICAL_CELL_PADDING;
                providerTableView.Frame = new CGRect (Constants.SINGLE_SELECTION_TOP_TABLE_PADDING, yPos, ViewContainerWidth - Constants.SINGLE_SELECTION_TOP_TABLE_PADDING*2, listHeight);
				yPos += listHeight + 15;
			}

			scrollContainer.Frame = new CGRect (0, 0, ViewContainerWidth, ViewContainerHeight);
            scrollContainer.ContentSize = new CGSize (ViewContainerWidth,yPos + GetBottomPadding());
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
                return (float)((GSCBaseView)View).baseContainer.Bounds.Height - Constants.NAV_HEIGHT;
            }
            else
            {
                return (float)base.View.Bounds.Height - Constants.NAV_HEIGHT;
            }
        }

        float IGSCBaseViewImplementor.ViewContentYPositionPadding()
        {
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                return 0;
            }
            else
            {
                return Constants.IS_OS_7_OR_LATER() ? Constants.IOS_7_TOP_PADDING : Constants.IOS_6_TOP_PADDING;
            }
        }
    }

	[Foundation.Register("ProviderLookupProviderTypeTableViewCell")]
	public class ProviderLookupProviderTypeTableViewCell : SingleSelectionTableViewCell
	{
		public ProviderLookupProviderTypeTableViewCell () : base () {}
		public ProviderLookupProviderTypeTableViewCell (IntPtr handle) : base (handle) {}

		public override void InitializeBindings()
		{
			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ProviderLookupProviderTypeTableViewCell,ServiceProviderType>();
					set.Bind(this.label).To(item => item.Type).WithConversion("StringCase").OneWay();
					set.Apply();
				});
		}
	}
}

