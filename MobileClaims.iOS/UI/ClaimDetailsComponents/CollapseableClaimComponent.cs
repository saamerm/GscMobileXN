 using System;
using UIKit;
using Foundation;
using CoreGraphics;
 using MobileClaims.Core.Services;
 using MvvmCross;
 using MvvmCross.Platforms.Ios.Views;
using Microsoft.AppCenter.Analytics;

 namespace MobileClaims.iOS
{
	public class CollapseableClaimComponent : MvxViewController
	{
		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler VisibilityToggled;

		public bool SubVisible;

		public UISwitch toggleSwitch;
		protected UILabel headingTitleLabel;

		protected UIView headingStage;
		protected UIView toggleStageMask;
		protected UIView toggleStage;

		protected float headingHeight = 0;
		protected float toggleStageMaskHeight;

		protected NSMutableArray subComponentsArray;

		protected bool openWithOff;

		public CollapseableClaimComponent ()
		{

		}

		public override void ViewWillAppear(bool animated)
		{
			Analytics.TrackEvent("PageView: " + NibName);
			base.ViewWillAppear(animated);
		}

		public override void ViewDidLoad ()
		{

			headingStage = new UIView ();
			toggleStageMask = new UIView ();
			toggleStage = new UIView ();

			headingStage.BackgroundColor = Colors.Clear;
			toggleStageMask.BackgroundColor = Colors.BACKGROUND_COLOR;
			toggleStage.BackgroundColor = Colors.Clear;

			this.View.AddSubview (toggleStageMask);
			this.View.AddSubview (toggleStage);
			this.View.AddSubview (headingStage);

			headingTitleLabel = new UILabel ();
			headingTitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
			headingTitleLabel.Lines = 3;
			headingTitleLabel.TextColor = Colors.DARK_GREY_COLOR;
			headingTitleLabel.Font = UIFont.FromName (Constants.NUNITO_BOLD, (nfloat)Constants.HEADING_FONT_SIZE );
			headingTitleLabel.Text = "This is filler text for the title label. Filler text for the title label.";
			headingStage.AddSubview (headingTitleLabel);

			toggleStage.Layer.Mask = toggleStageMask.Layer;

			toggleSwitch = new UISwitch ();
			toggleSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
			toggleSwitch.ValueChanged += HandleSwitch;

			headingStage.AddSubview (toggleSwitch);

			toggleStageMaskHeight = CurrentHeight - headingHeight;

			subComponentsArray = new NSMutableArray ();

            var rehydrationservice = Mvx.IoCProvider.Resolve<IRehydrationService>();
            if (rehydrationservice != null)
            {
                rehydrationservice.Rehydrating = false;
            }
		}

		protected virtual void HandleSwitch (object sender, EventArgs e)
		{
            CGRect toggleStageMaskFrame = (CGRect)toggleStageMask.Frame;
            CGRect toggleStageFrame = (CGRect)toggleStage.Frame;
            CGRect componentFrame = (CGRect)this.View.Frame;

			bool shouldOpen = openWithOff ? !toggleSwitch.On : toggleSwitch.On;

			if (shouldOpen) {
				SubVisible = true;
				toggleStageMaskFrame.Height = CurrentHeight - headingHeight;
				toggleStageMaskHeight = CurrentHeight - headingHeight;
				componentFrame.Height = CurrentHeight;
			} else {
				SubVisible = false;
				toggleStageMaskFrame.Height = 0;
				toggleStageMaskHeight = 0;
				componentFrame.Height = headingHeight;
			}

			sendVisibilityToggled ();

			UIView.Animate (Constants.TOGGLE_ANIMATION_DURATION, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
				() => {
					toggleStageMask.Frame = toggleStageMaskFrame ;
					toggleStage.Frame = toggleStageFrame ;
					this.View.Frame = componentFrame;
				},
				() => {
					//COMPLETION
				}
			);
		}

		protected void sendVisibilityToggled()
		{
			if (this.VisibilityToggled != null)
			{
				this.VisibilityToggled(this, EventArgs.Empty);
			}
		}
			
		public virtual float CurrentHeight
		{
			get{
				float _currentHeight = 0;

				bool shouldOpen = openWithOff ? !toggleSwitch.On : toggleSwitch.On;


				if (shouldOpen && subComponentsArray != null && ((uint)subComponentsArray.Count > 0)) {

					float yPOS = headingHeight;
					for (var i = 0; i < (uint)subComponentsArray.Count; i++) {
						ClaimDetailsSubComponent participantMenuItem = subComponentsArray.GetItem<ClaimDetailsSubComponent>((nuint)i);
						yPOS += participantMenuItem.ComponentHeight + Constants.CLAIMS_DETAILS_SUB_ITEM_PADDING;
					}
					_currentHeight = yPOS;
				} else {
					_currentHeight = headingHeight;
				}

				return _currentHeight;

			}
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

			bool shouldOpen = openWithOff ? !toggleSwitch.On : toggleSwitch.On;

			float contentWidth = (float)this.View.Frame.Width;

			float innerPadding = 18;

			float yPOS = 0;

			float headingPadding = Constants.CLAIMS_DETAILS_ITEM_V_PADDING;

            CGRect toggleFrame = (CGRect)toggleSwitch.Frame;

			headingTitleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, headingPadding, contentWidth - toggleFrame.Width - innerPadding, (float)headingTitleLabel.Frame.Height); 
			headingTitleLabel.SizeToFit ();
			headingTitleLabel.Frame = new CGRect (Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING, headingPadding, contentWidth - toggleFrame.Width - innerPadding, (float)headingTitleLabel.Frame.Height); 

			headingHeight = headingPadding * 2 + (float)headingTitleLabel.Frame.Height;

			toggleStageMaskHeight = shouldOpen ? CurrentHeight - headingHeight : 0;

			headingStage.Frame = new CGRect (0, 0, contentWidth, headingHeight);
			toggleStageMask.Frame = new CGRect (0, 0, contentWidth, toggleStageMaskHeight);

			toggleFrame.X = contentWidth - toggleFrame.Width - Constants.CLAIMS_DETAILS_ITEM_LEFT_PADDING;
			toggleFrame.Y = headingHeight / 2 - toggleFrame.Height / 2;
			toggleSwitch.Frame = toggleFrame;

			for (var i = 0; i < (uint)subComponentsArray.Count; i++) {

				ClaimDetailsSubComponent participantMenuItem = subComponentsArray.GetItem<ClaimDetailsSubComponent> ((nuint)i);
				participantMenuItem.Frame = new CGRect (0, yPOS, contentWidth, participantMenuItem.ComponentHeight);
				yPOS += participantMenuItem.ComponentHeight + Constants.CLAIMS_DETAILS_SUB_ITEM_PADDING;
			}

			toggleStage.Frame = new CGRect (0, headingHeight, contentWidth, yPOS);

		}

	}
}

