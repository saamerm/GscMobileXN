using System;
using UIKit;
using MobileClaims.Core.Messages;
using Foundation;
using CoreGraphics;
using MvvmCross;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Plugin.Messenger;

namespace MobileClaims.iOS
{
	public class GSCBaseView : MvxView
	{
		public UIScrollView baseScrollContainer;

		public UIView baseContainer;
		public UIView activityIndicatorContainer;

		public UIActivityIndicatorView indicator;

		protected IMvxMessenger _messenger;
		protected MvxSubscriptionToken _iscurrentlybusy;

		protected UITapGestureRecognizer tapRecognizer;

		private bool keyboardShown;

		public delegate void EventHandler(object sender, EventArgs e);
		public event EventHandler ViewTapped;

		public GSCBaseView ()
		{
			baseContainer = new UIView ();
			AddSubview (baseContainer);

			baseScrollContainer = new UIScrollView ();

			_messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

			activityIndicatorContainer = new UIView ();
			AddSubview (activityIndicatorContainer);

			indicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
            indicator.BackgroundColor = Colors.DARK_GREY_COLOR;
			activityIndicatorContainer.AddSubview (indicator);
			indicator.HidesWhenStopped = false;

			indicator.StopAnimating ();
			indicator.Alpha = 0;

			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.DidChangeFrameNotification, OrientationChanged);
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardShown);
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyboardHidden);
		

			Action action = () => { 
				if (this.ViewTapped != null)
				{
					this.ViewTapped(this, EventArgs.Empty);
				}
			};

			tapRecognizer = new UITapGestureRecognizer (action);
			tapRecognizer.NumberOfTapsRequired = 1;
			tapRecognizer.CancelsTouchesInView = false;

		}

		public void startLoading()
		{
			indicator.StartAnimating ();

			baseContainer.UserInteractionEnabled = false;

			UIView.Animate (0.25, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
				() => {
					indicator.Alpha = 1;},
				() => {

				}
			);
		}

		public void stopLoading(double _duration = 0.25)
		{
			baseContainer.UserInteractionEnabled = true;

			UIView.Animate (_duration, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
				() => {
					indicator.Alpha = 0;},
				() => {
					indicator.StopAnimating ();
				}
			);

		}

		public void subscribeToBusyIndicator()
		{
			if (!Busy) {
				stopLoading (0);
			}

			_iscurrentlybusy = _messenger.Subscribe<BusyIndicator>((message) =>
				{
					if(message.Busy){
						Busy = true;
					}else{
						Busy = false;
					}
				});
		}

		public void unsubscribeFromBusyIndicator()
		{
            try
            {
			    _messenger.Unsubscribe<BusyIndicator>(_iscurrentlybusy);
            }
            catch(Exception ex)
            {
            }
		}

		protected void KeyboardShown (NSNotification notification)
		{
			NSDictionary info = notification.UserInfo;

			keyboardShown = true;
			baseScrollContainer.AddGestureRecognizer(tapRecognizer);

			CGSize keySize = ((NSValue)info[UIKeyboard.FrameBeginUserInfoKey]).RectangleFValue.Size;

			//CGSize kbSize = [[info objectForKey:UIKeyboardFrameBeginUserInfoKey] CGRectValue].size;
//			UIEdgeInsets contentInsets = UIEdgeInsetsMake(0.0, 0.0, kbS, 0.0);
//			scrollView.contentInset = contentInsets;
//			scrollView.scrollIndicatorInsets = contentInsets;

			float keyHeight;

			if (Helpers.IsInLandscapeMode()) {
                // iOS 8 is orientation aware
                if (Constants.IS_OS_VERSION_OR_LATER(8, 0))
                    keyHeight = (float)keySize.Height;
                else
				    keyHeight = (float)keySize.Width;
			} else {
                keyHeight = (float)keySize.Height;
			}

			UIEdgeInsets contentInsets = new UIEdgeInsets (0, 0, keyHeight - Helpers.BottomNavHeight(), 0);

			UIView.Animate (0.25, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
				() => {
					baseScrollContainer.ContentInset = contentInsets;
					baseScrollContainer.ScrollIndicatorInsets = contentInsets;},
				() => {

				}
			);

		}

		protected void KeyboardHidden (NSNotification notification)
		{
			UIEdgeInsets contentInsets = new UIEdgeInsets (0, 0, 0, 0);

			keyboardShown = false;

			baseScrollContainer.RemoveGestureRecognizer(tapRecognizer);

			UIView.Animate (0.25, 0, (UIViewAnimationOptions)UIViewAnimationOptions.CurveEaseInOut,
				() => {
					baseScrollContainer.ContentInset = contentInsets;
					baseScrollContainer.ScrollIndicatorInsets = contentInsets;},
				() => {

				}
			);
		}

		protected void OrientationChanged (NSNotification notification)
		{
			if (keyboardShown) {

				NSDictionary info = notification.UserInfo;

				CGSize keySize = ((NSValue)info[UIKeyboard.FrameBeginUserInfoKey]).RectangleFValue.Size;

				//CGSize kbSize = [[info objectForKey:UIKeyboardFrameBeginUserInfoKey] CGRectValue].size;
				//			UIEdgeInsets contentInsets = UIEdgeInsetsMake(0.0, 0.0, kbS, 0.0);
				//			scrollView.contentInset = contentInsets;
				//			scrollView.scrollIndicatorInsets = contentInsets;

				float keyHeight;

				if (Helpers.IsInLandscapeMode()) {
                    // iOS 8 is orientation aware
                    if (Constants.IS_OS_VERSION_OR_LATER(8, 0))
                        keyHeight = (float)keySize.Height;
                    else
                        keyHeight = (float)keySize.Width;
				} else {
                    keyHeight = (float)keySize.Height;
				}

				UIEdgeInsets contentInsets = new UIEdgeInsets (0, 0, keyHeight - Helpers.BottomNavHeight(), 0);

				baseScrollContainer.ContentInset = contentInsets;
				baseScrollContainer.ScrollIndicatorInsets = contentInsets;
			}
		}

		private bool _busy = true;
		public bool Busy
		{
			get
			{
				return _busy;
			}
			set
			{
				_busy = value;
				if (!_busy) {
					InvokeOnMainThread ( () => {
						stopLoading ();
					});
				} else {
					InvokeOnMainThread ( () => {
						startLoading ();
					});
				}

			}
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();


            //baseContainer.BackgroundColor = UIColor.LightGray;
            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                Constants.SetSafe((float)SafeAreaInsets.Bottom);

                baseScrollContainer.Frame = new CGRect(SafeAreaInsets.Left, SafeAreaInsets.Top, Frame.Width - SafeAreaInsets.Right * 2, Frame.Height - SafeAreaInsets.Bottom);
                baseContainer.Frame = new CGRect(SafeAreaInsets.Left, SafeAreaInsets.Top, Frame.Width - SafeAreaInsets.Right * 2, Frame.Height - SafeAreaInsets.Bottom);
            } 
            else 
            {
                baseContainer.Frame = (CGRect)this.Frame;
                baseScrollContainer.Frame = (CGRect)this.Frame;
            }

            indicator.Frame = (CGRect)this.Frame;
		}
	}
}

