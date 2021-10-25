using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MobileClaims.Core.ViewModels;
using MobileClaims.Core.Entities;
using MobileClaims.Core.Services;

using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using MupApps.MvvmCross.Plugins.ControlsNavigation.Touch;

namespace MobileClaims.iOS
{
	[Register("DrugLookupResultsControl")]
	public class DrugLookupResultsControl : MvxTouchControl
	{
		private DrugLookupResultsViewModel _model;

		protected DrugInfo drugInfo;

		protected UILabel planParticipantTitleLabel;
		protected UITextView planParticipantLabel;
		protected UILabel drugInformationLabel;
		protected UILabel notesLabel;

		protected UIButton downloadButton;
		protected UIButton emailButton;

		protected UIWebView notesField;

		protected InformationHeadingAndDetailsComponent drugNameDetails;
		protected InformationHeadingAndDetailsComponent dinDetails;
		protected InformationHeadingAndDetailsComponent coveredDetails;
		protected InformationHeadingAndDetailsComponent reimbursmentDetails;
		protected InformationHeadingAndDetailsComponent specialAuthDetails;

		protected UIView buttonContainer;

		protected UIScrollView informationScrollView;

		protected bool isNameLookup;

		private const float BUTTON_CONTAINER_HEIGHT = 60;
		private const float BUTTON_WIDTH = 140;
		private const float BUTTON_HEIGHT = 30;

		private MvxFluentBindingDescriptionSet<DrugLookupResultsControl, Core.ViewModels.DrugLookupResultsViewModel> set;

		private bool componentsReady;

		protected bool controllerOpening;

		public DrugLookupResultsControl() : base(null,null)
		{
			//Initialize();
			this.EmptyControlBehaviour = MupApps.MvvmCross.Plugins.ControlsNavigation.EmptyControlBehaviours.Hide;
		}
		private RectangleF _frame;
		public RectangleF Frame
		{
			get
			{
				return _frame;
			}
			set
			{
				_frame = value;
				this.View.Frame = _frame;
				this.View.SetNeedsLayout ();
			}
		}

		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View = new UIView () { BackgroundColor = Constants.BACKGROUND_COLOR };
			base.NavigationItem.Title = "drugInformation".tr();

			_model = (DrugLookupResultsViewModel)(this.ViewModel);

			informationScrollView = new UIScrollView ();
			informationScrollView.BackgroundColor = UIColor.Clear;
			informationScrollView.Layer.CornerRadius = Constants.CORNER_RADIUS;
			View.AddSubview (informationScrollView);

			//Instantiating buttons early for binding
			emailButton	 = new UIButton (UIButtonType.RoundedRect);
			downloadButton = new UIButton (UIButtonType.RoundedRect);

			set = this.CreateBindingSet <DrugLookupResultsControl, Core.ViewModels.DrugLookupResultsViewModel>();
			set.Bind (this).For (v => v.busy).To (vm => vm.Busy);
			set.Bind (this).For (v => v.sentSpecialAuth).To (vm => vm.SentSpecialAuth);
			set.Bind (this).For (v => v.specialAuthError).To (vm => vm.SpecialAuthError);
			//set.Bind (emailButton).To (vm => vm.ExecuteSendSpecialAuthorizationCommand);
			set.Bind (downloadButton).To (vm => vm.DownloadSpecialAuthorizationCommand);
			set.Apply ();
		}

		public void layoutComponents ()
		{
			_model = (DrugLookupResultsViewModel)(this.ViewModel);
			drugInfo = _model.Drug;

			planParticipantTitleLabel = new UILabel();
			planParticipantTitleLabel.Text = "planParticipant".tr();
			planParticipantTitleLabel.TextColor = UIColor.Black;
			planParticipantTitleLabel.TextAlignment = UITextAlignment.Left;
			informationScrollView.AddSubview (planParticipantTitleLabel);

			planParticipantLabel = new UITextView();
			planParticipantLabel.Text = _model.Participant.FullName;
			planParticipantLabel.TextColor = UIColor.Black;
			planParticipantLabel.ContentInset = new UIEdgeInsets (5, Constants.DRUG_LOOKUP_LEFT_TEXT_INSET, 0, 0);
			planParticipantLabel.Font = UIFont.SystemFontOfSize (14.0f);
			planParticipantLabel.ScrollEnabled = false;
			planParticipantLabel.BackgroundColor = UIColor.White;
			planParticipantLabel.TextAlignment = UITextAlignment.Left;
			informationScrollView.AddSubview (planParticipantLabel);

			drugInformationLabel = new UILabel();
			drugInformationLabel.Text = "drugInformation".tr();
			drugInformationLabel.TextColor = UIColor.Black;
			drugInformationLabel.TextAlignment = UITextAlignment.Left;
			informationScrollView.AddSubview (drugInformationLabel);

			drugNameDetails = new InformationHeadingAndDetailsComponent (true, "drugNameAndStrength".tr(), drugInfo.Name);
			informationScrollView.AddSubview (drugNameDetails);

			dinDetails = new InformationHeadingAndDetailsComponent (true, "drugDINFieldText".tr(), drugInfo.DIN.ToString());
			informationScrollView.AddSubview (dinDetails);

			coveredDetails = new InformationHeadingAndDetailsComponent (true, "drugCovered".tr(), drugInfo.Covered? "Covered" : "Not Covered");
			informationScrollView.AddSubview (coveredDetails);

			reimbursmentDetails = new InformationHeadingAndDetailsComponent (true, "reimbursement".tr(), drugInfo.Reimbursement);
			informationScrollView.AddSubview (reimbursmentDetails);

			specialAuthDetails = new InformationHeadingAndDetailsComponent (false, "specialAuthRequired".tr(), drugInfo.SpecialAuthRequired ? "YES" : "NO");
			informationScrollView.AddSubview (specialAuthDetails);

			if (drugInfo.SpecialAuthRequired) {

				buttonContainer = new UIView();
				buttonContainer.BackgroundColor = UIColor.White;
				buttonContainer.Layer.MasksToBounds = true;
				informationScrollView.AddSubview (this.buttonContainer);

				downloadButton.SetTitle ("downloadAndView".tr(), UIControlState.Normal);
				downloadButton.Layer.BorderColor = Constants.LIGHT_BLUE_COLOR.CGColor;
				downloadButton.Layer.BorderWidth = 1.0f;
				downloadButton.Font  = UIFont.SystemFontOfSize (12.0f);
				downloadButton.Layer.CornerRadius = Constants.CORNER_RADIUS;
				buttonContainer.AddSubview (this.downloadButton);

				emailButton.SetTitle ("sendByEmail".tr(), UIControlState.Normal);
				emailButton.Layer.BorderColor = Constants.LIGHT_BLUE_COLOR.CGColor;
				emailButton.Layer.BorderWidth = 1.0f;
				emailButton.TouchUpInside += sendClicked;
				emailButton.Font = UIFont.SystemFontOfSize (12.0f);
				emailButton.Layer.CornerRadius = Constants.CORNER_RADIUS;
				buttonContainer.AddSubview (this.emailButton); 
			}

			notesLabel = new UILabel();
			notesLabel.Text = "notes".tr();
			notesLabel.TextColor = UIColor.Black;
			notesLabel.TextAlignment = UITextAlignment.Left;
			informationScrollView.AddSubview (notesLabel);

			notesField = new UIWebView();
			notesField.LoadHtmlString (drugInfo.Notes, null);
			notesField.BackgroundColor = UIColor.White;
			informationScrollView.AddSubview (notesField);

			componentsReady = true;
			View.SetNeedsLayout ();
		}

		public override void ViewDidLayoutSubviews ()
		{
			if (!componentsReady)
				return;

			float statusBarHeight = 20; //Status bar height acting peculiar in landscape. TODO: Figure better way to get status bar height. Tried: UIApplication.SharedApplication.StatusBarFrame.Size.Height
			float viewwidth = base.View.Bounds.Width;
			float startY = base.NavigationController.NavigationBar.Frame.Height + statusBarHeight;
			float contentWidth = viewwidth - Constants.DRUG_LOOKUP_SIDE_PADDING * 2;
			float viewHeight = base.View.Bounds.Height - startY;

			//TODO: Scroll views behaving strangely. Added tweaks here to fix. Remove eventually
			informationScrollView.Frame = new RectangleF (0, startY, viewwidth , viewHeight);


			planParticipantTitleLabel.Frame = new RectangleF (Constants.DRUG_LOOKUP_SIDE_PADDING, 0, contentWidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			planParticipantLabel.Frame = new RectangleF (0, planParticipantTitleLabel.Frame.Y + planParticipantTitleLabel.Frame.Height, viewwidth, Constants.DRUG_LOOKUP_FIELD_HEIGHT);
			drugInformationLabel.Frame = new RectangleF (Constants.DRUG_LOOKUP_SIDE_PADDING, planParticipantLabel.Frame.Y + planParticipantLabel.Frame.Height + Constants.DRUG_LOOKUP_TOP_PADDING, viewwidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			drugNameDetails.Frame = new RectangleF (0, drugInformationLabel.Frame.Y + drugInformationLabel.Frame.Height, viewwidth, drugNameDetails.Frame.Height);
			dinDetails.Frame = new RectangleF (0, drugNameDetails.Frame.Y + 70, viewwidth, dinDetails.Frame.Height); 
			coveredDetails.Frame = new RectangleF (0, dinDetails.Frame.Y + 70, viewwidth, dinDetails.Frame.Height); 
			reimbursmentDetails.Frame = new RectangleF (0, coveredDetails.Frame.Y + 70, viewwidth, dinDetails.Frame.Height); 
			specialAuthDetails.Frame = new RectangleF (0, reimbursmentDetails.Frame.Y + 70, viewwidth, specialAuthDetails.Frame.Height); 

			var notesY = specialAuthDetails.Frame.Y + specialAuthDetails.Frame.Height + Constants.DRUG_LOOKUP_TOP_PADDING;

			if ((buttonContainer != null) && (emailButton != null)) {
				buttonContainer.Frame = new RectangleF (0, specialAuthDetails.Frame.Y + + 70, viewwidth, BUTTON_CONTAINER_HEIGHT); 
				downloadButton.Frame = new RectangleF (Constants.DRUG_LOOKUP_SIDE_PADDING, BUTTON_CONTAINER_HEIGHT /2 - BUTTON_HEIGHT/2, BUTTON_WIDTH, BUTTON_HEIGHT);
				emailButton.Frame = new RectangleF (viewwidth - BUTTON_WIDTH - Constants.DRUG_LOOKUP_SIDE_PADDING, BUTTON_CONTAINER_HEIGHT /2 - BUTTON_HEIGHT/2, BUTTON_WIDTH, BUTTON_HEIGHT);
				notesY = buttonContainer.Frame.Y + buttonContainer.Frame.Height + Constants.DRUG_LOOKUP_TOP_PADDING;
			}

			notesLabel.Frame = new RectangleF (Constants.DRUG_LOOKUP_SIDE_PADDING, notesY, viewwidth, Constants.DRUG_LOOKUP_LABEL_HEIGHT);
			notesField.SizeToFit ();
			notesField.Frame = new RectangleF (0, notesLabel.Frame.Y + notesLabel.Frame.Height, viewwidth, Math.Max(notesField.ScrollView.ContentSize.Height + 50, Constants.DRUG_LOOKUP_TOP_PADDING));

			informationScrollView.ContentSize = new SizeF (contentWidth, notesField.Frame.Y + notesField.Frame.Height + 70);

		}

		void sendClicked(object s, EventArgs ea)
		{
			int buttonClicked = -1; 

			UIAlertViewDelegate alertDelegate = new UIAlertViewDelegate ();
			UIAlertView alert = new UIAlertView ("Send Special Authorization Form", "", alertDelegate, "OK", null);
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

			//TODO: There must be a better way to do this.
			alert.Clicked += (sender, buttonArgs) =>  { buttonClicked = buttonArgs.ButtonIndex; };
			alert.Show ();
			while (buttonClicked == -1)
			{
				NSRunLoop.Current.RunUntil(NSDate.FromTimeIntervalSinceNow (0.5));
			}

			if (buttonClicked == 0)
			{
				UITextField textField = alert.GetTextField(0);
				System.Console.WriteLine (textField.Text);
				_model.SpecialAuthEMail = textField.Text;
				_model.ExecuteSendSpecialAuthorizationCommand.Execute (null);

			}

		}

		#region Properties
		private bool _busy = true;
		public bool busy
		{
			get
			{
				return _busy;
			}
			set
			{
				_busy = value;
				if (!_busy && !componentsReady) {
					layoutComponents ();
				}

			}
		}

		private bool _sentSpecialAuth = false;
		public bool sentSpecialAuth
		{
			get
			{
				return _sentSpecialAuth;
			}
			set
			{
				_sentSpecialAuth = value;

			}
		}

		private string _specialAuthError = "";
		public string specialAuthError
		{
			get
			{
				return _specialAuthError;
			}
			set
			{
				_specialAuthError = value;
				if (!string.IsNullOrEmpty(_specialAuthError) && !sentSpecialAuth) {
					UIAlertView alert = new UIAlertView ("Error", _specialAuthError, new UIAlertViewDelegate(), "OK", null);
					alert.Show ();
				}

			}
		}
		#endregion

	}
}