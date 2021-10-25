using System;
using System.Collections.ObjectModel;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MobileClaims.Core.Entities.HCSA;
using MobileClaims.Core.Messages;
using MobileClaims.Core.ViewModels.HCSA;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace MobileClaims.iOS.Views.HCSA
{
    [Register("ClaimTypeView")]
    public class ClaimTypeView : GSCBaseViewController, IRehydrating
    {
        protected UILabel lblTypeOfClaim, lblTypeOfExpense;

        protected UILabel lblDescription, lblTypeOfMedicalProfessional, lblDesTitle;
        protected UIView descriptionBackgroundView;

        public UIImageView imageView;

        protected GSButton btnTypeOfClaim, btnTypeOfExpense, btnContinue;
        private HCSADropdownView vewChooseTypeOfClaim, vewChooseTypeOfExpense;
        private DismmssVCTableViewSource tableSourceExpense;
        public bool isiPadlandscape;
        public ClaimTypeViewModel _model;
        protected UIView participantBlockOverlay;
        protected UIScrollView scrollableContainer;
        private HCSADropdownView vewChooseTypeOfMedicalProfessional;
        int ContinueTopPadding, descriptionTopPadding;
        private IMvxMessenger _messenger;
        protected UIButton btnDropdown;
        protected bool isExpanded;
        private float lblDescriptBelowMargin = 10;
        private float tableHeightFor3Items = 160f;
        private float tableHeightFor4Items = 220f;

        public bool Rehydrating
        {
            get;
            set;
        }
        public bool FinishedRehydrating
        {
            get;
            set;
        }

        public ClaimTypeView()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void WillAnimateRotation(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillAnimateRotation(toInterfaceOrientation, duration);

            switch (toInterfaceOrientation)
            {

                case UIInterfaceOrientation.LandscapeLeft:
                    isiPadlandscape = true;
                    SetConstraints();
                    break;

                case UIInterfaceOrientation.LandscapeRight:
                    isiPadlandscape = true;
                    SetConstraints();
                    break;

                case UIInterfaceOrientation.Portrait:
                    isiPadlandscape = false;
                    SetConstraints();
                    break;

                case UIInterfaceOrientation.PortraitUpsideDown:
                    isiPadlandscape = false;
                    SetConstraints();
                    break;
            }
        }

        void HandleViewTapped(object sender, EventArgs e)
        {
            dismissKeyboard();
        }

        void dismissKeyboard()
        {
            this.View.EndEditing(true);
        }

        void checkDescriptionPadding()
        {

            if (_model.IsExpenseTypeVisible)
            { // below TypeOfExp
                descriptionTopPadding = 2;
            }
            else
            { // just below typeOfClaim
                descriptionTopPadding = 1;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _model = (ClaimTypeViewModel)this.ViewModel;
            _model.OnRefusedToChangeType += _model_OnRefusedToChangeType;

            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton(false, false);
            base.NavigationItem.Title = _model.TitleLabel.ToUpper();

            _messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();
            View = new UIView() { BackgroundColor = Colors.BACKGROUND_COLOR };

            checkDescriptionPadding();

            if (_model.IsReferralQuestionVisible)
                ContinueTopPadding = 2;
            else
                ContinueTopPadding = 1;

            _model.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "IsExpenseTypeVisible")
                {
                    checkDescriptionPadding();
                }

                if (e.PropertyName == "IsReferralQuestionVisible")
                {
                    if (_model.IsReferralQuestionVisible)
                    {
                        ContinueTopPadding = 2;// button under referral type
                    }
                    else
                    {
                        ContinueTopPadding = 1;// button under description

                    }
                }
                if (e.PropertyName == "SelectedClaimType")
                {
                    setSelectedClaimType();
                    return;
                }
                else if (e.PropertyName == "SelectedClaimExpenseType")
                {
                    setSelectedExpenseType();
                    return;
                }
                else if (e.PropertyName == "SelectedClaimExpenseType")
                {
                    setSelectedReferralType();
                    return;
                }
                SetConstraints();

            };

            scrollableContainer = new UIScrollView();
            scrollableContainer.ScrollEnabled = true;
            View.AddSubview(scrollableContainer);


            lblTypeOfClaim = new UILabel();
            lblTypeOfClaim.Text = _model.ChooseTypeOfClaimLabel;
            lblTypeOfClaim.BackgroundColor = Colors.Clear;
            lblTypeOfClaim.TextColor = Colors.DARK_GREY_COLOR;
            lblTypeOfClaim.TextAlignment = UITextAlignment.Left;
            lblTypeOfClaim.Lines = 2;
            lblTypeOfClaim.LineBreakMode = UILineBreakMode.WordWrap;
            lblTypeOfClaim.Font = UIFont.FromName(Constants.NUNITO_BLACK, (nfloat)Constants.HEADING_HCSA_FONT_SIZE);

            vewChooseTypeOfClaim = new HCSADropdownView(this);

            lblTypeOfExpense = new UILabel();
            lblTypeOfExpense.Text = _model.ChooseTypeOfExpenseLabel;
            lblTypeOfExpense.BackgroundColor = Colors.Clear;
            lblTypeOfExpense.TextColor = Colors.DARK_GREY_COLOR;
            lblTypeOfExpense.TextAlignment = UITextAlignment.Left;
            lblTypeOfExpense.Lines = 2;
            lblTypeOfExpense.LineBreakMode = UILineBreakMode.WordWrap;
            lblTypeOfExpense.Font = UIFont.FromName(Constants.NUNITO_BLACK, (nfloat)Constants.HEADING_HCSA_FONT_SIZE);

            vewChooseTypeOfExpense = new HCSADropdownView(this);
            vewChooseTypeOfExpense.COMPONENT_HEIGHT = 280f;
            if (!Constants.IsPhone())
            {
                if (_model != null)
                {
                    if (_model.ClaimExpenseTypes != null)
                    {
                        if (_model.ClaimExpenseTypes.Count == 2)
                            vewChooseTypeOfExpense.COMPONENT_HEIGHT = tableHeightFor3Items;
                        if (_model.ClaimExpenseTypes.Count == 3)
                            vewChooseTypeOfExpense.COMPONENT_HEIGHT = tableHeightFor4Items;
                    }
                }
            }
            btnContinue = new GSButton();
            btnContinue.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);// (nfloat)Constants.GREEN_BUTTON_FONT_SIZE);

            descriptionBackgroundView = new UIView();
            descriptionBackgroundView.Layer.BorderWidth = Constants.FIELD_BORDER_SIZE;
            descriptionBackgroundView.Layer.BorderColor = Colors.MED_GREY_COLOR.CGColor;

            lblDescription = new UILabel();
            lblDescription.TextColor = Colors.DARK_GREY_COLOR;
            lblDescription.Lines = 0;
            lblDescription.Font = UIFont.FromName(Constants.NUNITO_SEMIBOLD, (nfloat)Constants.TEXTVIEW_FONT_SIZE);

            if (Constants.IsPhone())
            {
                lblDescription.BackgroundColor = Colors.LightGrayColor;
                descriptionBackgroundView.BackgroundColor = Colors.LightGrayColor;
                lblDescription.TextColor = (isExpanded) ? Colors.DARK_GREY_COLOR : Colors.LightGrayColor;
            }
            else
            {
                lblDescription.BackgroundColor = Colors.BACKGROUND_COLOR;
                descriptionBackgroundView.BackgroundColor = Colors.BACKGROUND_COLOR;
            }

            lblDesTitle = new UILabel();
            lblDesTitle.TextColor = Colors.DARK_GREY_COLOR;
            lblDesTitle.Lines = 1;
            lblDesTitle.Font = UIFont.FromName(Constants.NUNITO_REGULAR, (nfloat)Constants.DROPDOWN_FONT_SIZE);

            lblTypeOfMedicalProfessional = new UILabel();
            lblTypeOfMedicalProfessional.Text = _model.ChooseMedicalProfessionalTypeLabel;
            lblTypeOfMedicalProfessional.BackgroundColor = Colors.Clear;
            lblTypeOfMedicalProfessional.TextColor = Colors.DARK_GREY_COLOR;
            lblTypeOfMedicalProfessional.TextAlignment = UITextAlignment.Left;
            lblTypeOfMedicalProfessional.Lines = 6;
            lblTypeOfMedicalProfessional.LineBreakMode = UILineBreakMode.WordWrap;
            lblTypeOfMedicalProfessional.Font = UIFont.FromName(Constants.NUNITO_BLACK, (nfloat)Constants.HEADING_HCSA_FONT_SIZE);

            vewChooseTypeOfMedicalProfessional = new HCSADropdownView(this);

            scrollableContainer.AddSubview(lblTypeOfClaim);
            scrollableContainer.AddSubview(vewChooseTypeOfClaim);
            scrollableContainer.AddSubview(lblTypeOfExpense);
            scrollableContainer.AddSubview(vewChooseTypeOfExpense);

            descriptionBackgroundView.AddSubview(lblDesTitle);

            //dropdown
            if (Constants.IsPhone())
            {
                isExpanded = false;

                imageView = new UIImageView();
                btnDropdown = new UIButton();

                imageView.Image = UIImage.FromBundle("ArrowDownGray");

                descriptionBackgroundView.AddSubview(imageView);
                descriptionBackgroundView.AddSubview(btnDropdown);



                btnDropdown.TouchUpInside += (sender, e) =>
                {

                    if (isExpanded == false)
                    {
                        isExpanded = true;
                        imageView.Image = UIImage.FromBundle("ArrowUpGray");
                        lblDescription.TextColor = Colors.DARK_GREY_COLOR;

                    }
                    else
                    {
                        isExpanded = false;
                        imageView.Image = UIImage.FromBundle("ArrowDownGray");
                        lblDescription.TextColor = Colors.LightGrayColor;

                    }
                    SetConstraints();
                };

            }

            descriptionBackgroundView.AddSubview(lblDescription);

            scrollableContainer.AddSubview(lblTypeOfMedicalProfessional);
            scrollableContainer.AddSubview(vewChooseTypeOfMedicalProfessional);
            scrollableContainer.AddSubview(descriptionBackgroundView);
            scrollableContainer.AddSubview(btnContinue);

            UIInterfaceOrientation interfaceOrientation = this.InterfaceOrientation;
            switch (interfaceOrientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    isiPadlandscape = true;
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    isiPadlandscape = true;
                    break;
                case UIInterfaceOrientation.Portrait:
                    isiPadlandscape = false;
                    break;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    isiPadlandscape = false;
                    break;
            }

            SetConstraints();

            DismmssVCTableViewSource tableSourceClaim = new DismmssVCTableViewSource(vewChooseTypeOfClaim.popoverController, vewChooseTypeOfClaim.listController.tableView, "TypeOfClaimTableViewCell", typeof(TypeOfClaimTableViewCell));
            tableSourceExpense = new DismmssVCTableViewSource(vewChooseTypeOfExpense.popoverController, vewChooseTypeOfExpense.listController.tableView, "TypeOfExpenseTableViewCell", typeof(TypeOfExpenseTableViewCell));
            DismmssVCTableViewSource tableSourceMedProf = new DismmssVCTableViewSource(vewChooseTypeOfMedicalProfessional.popoverController, vewChooseTypeOfMedicalProfessional.listController.tableView, "MedicalProfessionalTableCell", typeof(MedicalProfessionalTableCell));

            var set = this.CreateBindingSet<ClaimTypeView, ClaimTypeViewModel>();
            set.Bind(this.lblTypeOfClaim).To(vm => vm.ChooseTypeOfClaimLabel).WithConversion("StringCase");

            set.Bind(tableSourceClaim).To(vm => vm.ClaimTypes);
            set.Bind(tableSourceClaim).For(s => s.SelectedItem).To(vm => vm.SelectedClaimType);
            set.Bind(tableSourceClaim).For(s => s.SelectionChangedCommand).To(vm => vm.SelectClaimTypeWithoutNavigatingCommand);
            set.Bind(vewChooseTypeOfClaim.selectedTypeLabel).To(vm => vm.SelectedClaimType.Name);

            set.Bind(this.lblTypeOfExpense).To(vm => vm.ChooseTypeOfExpenseLabel).WithConversion("StringCase");
            set.Bind(lblTypeOfExpense).For(b => b.Hidden).To(vm => vm.IsExpenseTypeVisible).WithConversion("BoolOpposite");
            set.Bind(vewChooseTypeOfExpense).For(b => b.Hidden).To(vm => vm.IsExpenseTypeVisible).WithConversion("BoolOpposite");
            set.Bind(tableSourceExpense).To(vm => vm.ClaimExpenseTypes);
            set.Bind(tableSourceExpense).For(s => s.SelectedItem).To(vm => vm.SelectedClaimExpenseType);
            set.Bind(tableSourceExpense).For(s => s.SelectionChangedCommand).To(vm => vm.SelectExpenseTypeWithoutNavigatingCommand);
            set.Bind(vewChooseTypeOfExpense.selectedTypeLabel).To(vm => vm.SelectedClaimExpenseType.Name);
            set.Bind(this).For(v => v.ClaimExpenseTypesCollection).To(vm => vm.ClaimExpenseTypes);
            set.Bind(descriptionBackgroundView).For(b => b.Hidden).To(vm => vm.IsDescriptionVisible).WithConversion("BoolOpposite");
            set.Bind(lblDescription).To(vm => vm.Description);
            set.Bind(lblDesTitle).To(vm => vm.DescriptionLabel);

            set.Bind(this.lblTypeOfMedicalProfessional).To(vm => vm.ChooseMedicalProfessionalTypeLabel).WithConversion("StringCase");
            set.Bind(lblTypeOfMedicalProfessional).For(b => b.Hidden).To(vm => vm.IsReferralQuestionVisible).WithConversion("BoolOpposite");
            set.Bind(vewChooseTypeOfMedicalProfessional).For(b => b.Hidden).To(vm => vm.IsReferralQuestionVisible).WithConversion("BoolOpposite");
            set.Bind(tableSourceMedProf).To(vm => vm.MedicalProfessionalTypes);
            set.Bind(tableSourceMedProf).For(s => s.SelectedItem).To(vm => vm.SelectedMedicalProfessionalType);
            set.Bind(tableSourceMedProf).For(s => s.SelectionChangedCommand).To(vm => vm.SelectMedicalProfessionalTypeCommand);
            set.Bind(vewChooseTypeOfMedicalProfessional.selectedTypeLabel).To(vm => vm.SelectedMedicalProfessionalType.Text);

            set.Bind(this.btnContinue).For(b => b.Hidden).To(vm => vm.IsContinueButtonVisible).WithConversion("BoolOpposite");
            set.Bind(this.btnContinue).To(vm => vm.ContinueCommand);
            set.Bind(this.btnContinue).For("Title").To(vm => vm.ContinueLabel).WithConversion("StringCase");
            set.Apply();

            btnContinue.TitleLabel.Font = UIFont.FromName(Constants.LEAGUE_GOTHIC, Constants.GREEN_BUTTON_FONT_SIZE);

            vewChooseTypeOfMedicalProfessional.listController.tableView.Source = tableSourceMedProf;
            vewChooseTypeOfMedicalProfessional.listController.tableView.ReloadData();
            vewChooseTypeOfClaim.listController.tableView.Source = tableSourceClaim;
            vewChooseTypeOfClaim.listController.tableView.ReloadData();
            vewChooseTypeOfExpense.listController.tableView.Source = tableSourceExpense;
            vewChooseTypeOfExpense.listController.tableView.ReloadData();

            setSelectedClaimType();
            setSelectedExpenseType();
            setSelectedReferralType();
        }

        private void _model_OnRefusedToChangeType(object sender, EventArgs e)
        {
            vewChooseTypeOfClaim.ClosePopup();
            vewChooseTypeOfExpense.ClosePopup();
            vewChooseTypeOfMedicalProfessional.ClosePopup();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _messenger.Publish<OnClaimDetailsViewModelMessage>(new OnClaimDetailsViewModelMessage(this));
        }

        private void setSelectedClaimType()
        {
            if (_model != null && _model.SelectedClaimType != null && _model.ClaimTypes != null)
            {
                for (int i = 0; i < _model.ClaimTypes.Count; i++)
                {
                    ClaimType detail = _model.ClaimTypes[i];
                    if (detail != null && detail.ID == _model.SelectedClaimType.ID)
                    {
                        NSIndexPath path = NSIndexPath.FromRowSection(i, 0);

                        vewChooseTypeOfClaim.setSelectedRow(path);
                        break;
                    }
                }
            }
        }

        private void setSelectedExpenseType()
        {
            if (_model != null && _model.SelectedClaimExpenseType != null)
            {
                for (int i = 0; i < _model.ClaimExpenseTypes.Count; i++)
                {
                    ExpenseType detail = _model.ClaimExpenseTypes[i];
                    if (detail != null && detail.ID == _model.SelectedClaimExpenseType.ID)
                    {
                        NSIndexPath path = NSIndexPath.FromRowSection(i, 0);
                        try
                        {
                            vewChooseTypeOfExpense.setSelectedRow(path);
                        }
                        catch
                        {
                        }
                        break;
                    }
                }
            }
        }

        private void setSelectedReferralType()
        {
            if (_model != null && _model.SelectedMedicalProfessionalType != null)
            {
                for (int i = 0; i < _model.MedicalProfessionalTypes.Count; i++)
                {
                    HCSAReferralType detail = _model.MedicalProfessionalTypes[i];
                    if (detail != null && detail.Code == _model.SelectedMedicalProfessionalType.Code)
                    {
                        NSIndexPath path = NSIndexPath.FromRowSection(i, 0);
                        vewChooseTypeOfMedicalProfessional.setSelectedRow(path);
                        break;
                    }
                }
            }
        }

        private void SetConstraints()
        {
            float viewWidth = (float)base.View.Frame.Width;
            float viewHeight = (float)base.View.Frame.Height - Helpers.BottomNavHeight();

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            scrollableContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            descriptionBackgroundView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            if (!Constants.IsPhone())
            {
                //ipad

                View.RemoveConstraints(View.Constraints);
                if (ContinueTopPadding == 1)
                {
                    if (descriptionTopPadding == 1)
                    {
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View),// scrollContainerTopMargin),
                            scrollableContainer.WithSameHeight(View),//.Minus(scrollContainerTopMargin),  
                            scrollableContainer.AtLeftOf(View),
                            scrollableContainer.WithSameWidth(View),
                            scrollableContainer.AtBottomOf(View),

                            //							lblTypeOfClaim.AtTopOf (scrollableContainer,100),//50+15
                            lblTypeOfClaim.WithSameCenterY(vewChooseTypeOfClaim),
                            lblTypeOfClaim.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfClaim.Height ().EqualTo (18),
                            lblTypeOfClaim.WithRelativeWidth(scrollableContainer, 0.5f),
                            //							lblTypeOfClaim.Width ().EqualTo (190),

                            vewChooseTypeOfClaim.AtTopOf(scrollableContainer, 95),
                            vewChooseTypeOfClaim.AtRightOf(View, 20),
                            vewChooseTypeOfClaim.ToRightOf(lblTypeOfClaim, 5),
                            //							vewChooseTypeOfClaim.Height ().EqualTo (48),

                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, 55),
                            lblTypeOfExpense.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfExpense.Height ().EqualTo (18),
                            lblTypeOfExpense.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfExpense.Below(vewChooseTypeOfClaim, 50),
                            vewChooseTypeOfExpense.AtRightOf(View, 20),
                            vewChooseTypeOfExpense.ToRightOf(lblTypeOfExpense, 5),
                            //							vewChooseTypeOfExpense.Height ().EqualTo (48),
                            //75+18+60+48 =201

                            descriptionBackgroundView.Below(vewChooseTypeOfClaim, 50),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 25),
                            descriptionBackgroundView.AtRightOf(View, 20),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 10),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            //lblDescription.AtTopOf(descriptionBackgroundView,10),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, 50),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfMedicalProfessional.Width ().EqualTo (190),
                            lblTypeOfMedicalProfessional.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfMedicalProfessional.Below(descriptionBackgroundView, 50),
                            vewChooseTypeOfMedicalProfessional.ToRightOf(lblTypeOfMedicalProfessional, 5),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, 20),
                            //							vewChooseTypeOfMedicalProfessional.Height ().EqualTo (48),

                            btnContinue.Below(descriptionBackgroundView, 25),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 5)

                        );
                    }
                    else
                    {
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View),// scrollContainerTopMargin),
                            scrollableContainer.WithSameHeight(View),//.Minus(scrollContainerTopMargin),  
                            scrollableContainer.AtLeftOf(View),
                            scrollableContainer.WithSameWidth(View),
                            scrollableContainer.AtBottomOf(View),

                            //							lblTypeOfClaim.AtTopOf (scrollableContainer,100),//50+15
                            lblTypeOfClaim.WithSameCenterY(vewChooseTypeOfClaim),
                            lblTypeOfClaim.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfClaim.Height ().EqualTo (18),
                            lblTypeOfClaim.WithRelativeWidth(scrollableContainer, 0.5f),
                            //							lblTypeOfClaim.Width ().EqualTo (190),

                            vewChooseTypeOfClaim.AtTopOf(scrollableContainer, 95),
                            vewChooseTypeOfClaim.AtRightOf(View, 20),
                            vewChooseTypeOfClaim.ToRightOf(lblTypeOfClaim, 5),
                            //							vewChooseTypeOfClaim.Height ().EqualTo (48),

                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, 55),
                            lblTypeOfExpense.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfExpense.Height ().EqualTo (18),
                            lblTypeOfExpense.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfExpense.Below(vewChooseTypeOfClaim, 50),
                            vewChooseTypeOfExpense.AtRightOf(View, 20),
                            vewChooseTypeOfExpense.ToRightOf(lblTypeOfExpense, 5),
                            //							vewChooseTypeOfExpense.Height ().EqualTo (48),
                            //75+18+60+48 =201

                            descriptionBackgroundView.Below(vewChooseTypeOfExpense, 50),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 25),
                            descriptionBackgroundView.AtRightOf(View, 20),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 10),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            //lblDescription.AtTopOf(descriptionBackgroundView,10),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, 50),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfMedicalProfessional.Width ().EqualTo (190),
                            lblTypeOfMedicalProfessional.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfMedicalProfessional.Below(descriptionBackgroundView, 50),
                            vewChooseTypeOfMedicalProfessional.ToRightOf(lblTypeOfMedicalProfessional, 5),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, 20),
                            //							vewChooseTypeOfMedicalProfessional.Height ().EqualTo (48),

                            btnContinue.Below(descriptionBackgroundView, 25),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 5)

                        );
                    }
                }
                else
                {

                    if (descriptionTopPadding == 1)
                    {
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View),// scrollContainerTopMargin),
                            scrollableContainer.WithSameHeight(View),//.Minus(scrollContainerTopMargin),  
                            scrollableContainer.AtLeftOf(View),
                            scrollableContainer.WithSameWidth(View),
                            scrollableContainer.AtBottomOf(View),

                            //							lblTypeOfClaim.AtTopOf (scrollableContainer,100),//50+15
                            lblTypeOfClaim.WithSameCenterY(vewChooseTypeOfClaim),
                            lblTypeOfClaim.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfClaim.Height ().EqualTo (18),
                            lblTypeOfClaim.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfClaim.AtTopOf(scrollableContainer, 95),
                            vewChooseTypeOfClaim.AtRightOf(View, 20),
                            vewChooseTypeOfClaim.ToRightOf(lblTypeOfClaim, 5),
                            //							vewChooseTypeOfClaim.Height ().EqualTo (48),

                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, 65),
                            lblTypeOfExpense.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfExpense.Height ().EqualTo (18),
                            lblTypeOfExpense.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfExpense.Below(vewChooseTypeOfClaim, 50),
                            vewChooseTypeOfExpense.AtRightOf(View, 20),
                            vewChooseTypeOfExpense.ToRightOf(lblTypeOfExpense, 5),
                            //							vewChooseTypeOfExpense.Height ().EqualTo (48),
                            //75+18+60+48 =201

                            descriptionBackgroundView.Below(vewChooseTypeOfClaim, 50),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 25),
                            descriptionBackgroundView.AtRightOf(View, 20),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 10),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            //lblDescription.AtTopOf(descriptionBackgroundView,10),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, 40),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, 25),
                            lblTypeOfMedicalProfessional.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfMedicalProfessional.Below(descriptionBackgroundView, 40),
                            vewChooseTypeOfMedicalProfessional.ToRightOf(lblTypeOfMedicalProfessional, 5),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, 20),
                            //							vewChooseTypeOfMedicalProfessional.Height ().EqualTo (48),

                            btnContinue.Below(lblTypeOfMedicalProfessional, 15),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 5)

                        );
                    }
                    else
                    {
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View),// scrollContainerTopMargin),
                            scrollableContainer.WithSameHeight(View),//.Minus(scrollContainerTopMargin),  
                            scrollableContainer.AtLeftOf(View),
                            scrollableContainer.WithSameWidth(View),
                            scrollableContainer.AtBottomOf(View),

                            //							lblTypeOfClaim.AtTopOf (scrollableContainer,110),//50+15
                            lblTypeOfClaim.WithSameCenterY(vewChooseTypeOfClaim),
                            lblTypeOfClaim.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfClaim.Height ().EqualTo (18),
                            lblTypeOfClaim.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfClaim.AtTopOf(scrollableContainer, 95),
                            vewChooseTypeOfClaim.AtRightOf(View, 20),
                            vewChooseTypeOfClaim.ToRightOf(lblTypeOfClaim, 5),
                            //							vewChooseTypeOfClaim.Height ().EqualTo (48),

                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, 65),
                            lblTypeOfExpense.AtLeftOf(scrollableContainer, 25),
                            //							lblTypeOfExpense.Height ().EqualTo (18),
                            lblTypeOfExpense.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfExpense.Below(vewChooseTypeOfClaim, 50),
                            vewChooseTypeOfExpense.AtRightOf(View, 20),
                            vewChooseTypeOfExpense.ToRightOf(lblTypeOfExpense, 5),
                            //							vewChooseTypeOfExpense.Height ().EqualTo (48),
                            //75+18+60+48 =201

                            descriptionBackgroundView.Below(vewChooseTypeOfExpense, 50),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, 25),
                            descriptionBackgroundView.AtRightOf(View, 20),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 10),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            //lblDescription.AtTopOf(descriptionBackgroundView,10),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, 40),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, 25),
                            lblTypeOfMedicalProfessional.WithRelativeWidth(scrollableContainer, 0.5f),

                            vewChooseTypeOfMedicalProfessional.Below(descriptionBackgroundView, 40),
                            vewChooseTypeOfMedicalProfessional.ToRightOf(lblTypeOfMedicalProfessional, 5),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, 20),
                            //							vewChooseTypeOfMedicalProfessional.Height ().EqualTo (48),

                            btnContinue.Below(lblTypeOfMedicalProfessional, 15),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 5)

                        );
                    }
                }
            }
            else //iPhone
            {
                if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    SetupConstraintsForIOS11();
                }
                else //iPhone
                {
                    View.RemoveConstraints(View.Constraints);
                    descriptionBackgroundView.RemoveConstraints(View.Constraints);

                    if (isExpanded == true)
                    {


                        if (descriptionTopPadding == 1)
                        {
                            // description below claim

                            if (ContinueTopPadding == 1)
                            {// done button right under description

                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                      lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),//12
                                                                                                               //                               vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),


                                    descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(descriptionBackgroundView, 20),
                                    //btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                            else
                            {// done button right under referral view
                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                      lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),//12
                                                                                                               //                               vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),


                                    descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                                    //                      btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                        }
                        else
                        {
                            // description below expense

                            if (ContinueTopPadding == 1)
                            {// done button right under description

                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                      lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),//12
                                                                                                               //                               vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                        //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),


                                        descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(descriptionBackgroundView, 20),
                                    //btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                            else
                            {// done button right under referral view
                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                      lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),//12
                                                                                                               //                               vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                        //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),


                                        descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                                    //                      btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                        }

                    }
                    else
                    {

                        if (descriptionTopPadding == 1)
                        {
                            // description below claim

                            if (ContinueTopPadding == 1)
                            {// done button under description
                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.WithSameHeight(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                          lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                        //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                        descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.Height().EqualTo(40),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.Height().EqualTo(1),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(descriptionBackgroundView, 30),
                                    //                      btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                            else
                            {// done button under referral view

                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.WithSameHeight(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                          lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                        //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                        descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.Height().EqualTo(40),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.Height().EqualTo(1),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                                    //btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }

                        }
                        else
                        {
                            // description below expense

                            if (ContinueTopPadding == 1)
                            {// done button under description
                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.WithSameHeight(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                          lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.Height().EqualTo(40),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.Height().EqualTo(1),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(descriptionBackgroundView, 30),
                                    //                      btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                            else
                            {// done button under referral view

                                View.AddConstraints(

                                    scrollableContainer.AtTopOf(View),
                                    scrollableContainer.WithSameHeight(View),
                                    scrollableContainer.AtLeftOf(View),
                                    scrollableContainer.WithSameWidth(View),
                                    scrollableContainer.AtBottomOf(View),

                                    lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),
                                    //                          lblTypeOfClaim.Height ().EqualTo (Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfClaim.AtLeftOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfClaim.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                                    vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),
                                    //                              vewChooseTypeOfExpense.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    descriptionBackgroundView.Height().EqualTo(40),

                                    lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                                    imageView.AtTopOf(descriptionBackgroundView, 15),
                                    imageView.AtRightOf(descriptionBackgroundView, 10),
                                    imageView.Width().EqualTo(15),

                                    btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                                    btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                                    btnDropdown.Height().EqualTo(30),

                                    lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                                    lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                                    lblDescription.AtRightOf(descriptionBackgroundView, 10),
                                    lblDescription.Height().EqualTo(1),

                                    lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                                    lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                                    lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),

                                    vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                                    vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                                    //                              vewChooseTypeOfMedicalProfessional.Height ().EqualTo (Constants.DROPDOWN_HEIGHT),

                                    btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                                    //btnContinue.Below (vewChooseTypeOfMedicalProfessional, Constants.DRUG_LOOKUP_TOP_PADDING),
                                    btnContinue.WithSameCenterX(scrollableContainer),
                                    btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                                    btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                                    btnContinue.AtBottomOf(scrollableContainer, 100)

                                );
                            }
                        }
                    }




                }
            }


        }

        private void SetupConstraintsForIOS11()
        {
            View.RemoveConstraints(View.Constraints);
            descriptionBackgroundView.RemoveConstraints(View.Constraints);

            if (isExpanded == true)
            {
                if (descriptionTopPadding == 1)
                {
                    // description below claim

                    if (ContinueTopPadding == 1)
                    {// done button right under description

                        View.AddConstraints(
                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),//12

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(descriptionBackgroundView, 20),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)
                        );
                    }
                    else
                    {
                        // done button right under referral view
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)

                        );
                    }
                }
                else
                {
                    // description below expense

                    if (ContinueTopPadding == 1)
                    {// done button right under description

                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(descriptionBackgroundView, 20),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)

                        );
                    }
                    else
                    {
                        // done button right under referral view
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),//20
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.AtBottomOf(descriptionBackgroundView, 10),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)
                        );
                    }
                }

            }
            else
            {

                if (descriptionTopPadding == 1)
                {
                    // description below claim

                    if (ContinueTopPadding == 1)
                    {// done button under description
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            descriptionBackgroundView.Height().EqualTo(40),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.Height().EqualTo(1),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(descriptionBackgroundView, 30),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)

                        );
                    }
                    else
                    {// done button under referral view

                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfClaim, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            descriptionBackgroundView.Height().EqualTo(40),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.Height().EqualTo(1),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(descriptionBackgroundView, 30),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)

                        );
                    }

                }
                else
                {
                    // description below expense

                    if (ContinueTopPadding == 1)
                    {// done button under description
                        View.AddConstraints(

                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            descriptionBackgroundView.Height().EqualTo(40),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.Height().EqualTo(1),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(descriptionBackgroundView, 30),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)

                        );
                    }
                    else
                    {// done button under referral view

                        View.AddConstraints(
                            scrollableContainer.AtTopOf(View, View.SafeAreaInsets.Top),
                            scrollableContainer.AtLeftOf(View, View.SafeAreaInsets.Left),
                            scrollableContainer.AtRightOf(View, View.SafeAreaInsets.Right),
                            scrollableContainer.WithSameWidth(View).Minus(View.SafeAreaInsets.Left * 2),
                            scrollableContainer.AtBottomOf(View, View.SafeAreaInsets.Bottom),

                            lblTypeOfClaim.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfClaim.AtTopOf(scrollableContainer, Constants.TOP_PADDING),

                            vewChooseTypeOfClaim.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfClaim.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfClaim.Below(lblTypeOfClaim, Constants.DROPDOWN_TOP_PADDING),

                            lblTypeOfExpense.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfExpense.Below(vewChooseTypeOfClaim, Constants.LABEL_TOP_PADDING),
                            lblTypeOfExpense.Height().EqualTo(Constants.LABEL_HEIGHT),

                            vewChooseTypeOfExpense.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfExpense.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            vewChooseTypeOfExpense.Below(lblTypeOfExpense, Constants.DROPDOWN_TOP_PADDING),

                            descriptionBackgroundView.Below(vewChooseTypeOfExpense, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            descriptionBackgroundView.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),
                            descriptionBackgroundView.Height().EqualTo(40),

                            lblDesTitle.AtTopOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtLeftOf(descriptionBackgroundView, 10),
                            lblDesTitle.AtRightOf(descriptionBackgroundView, 50),

                            imageView.AtTopOf(descriptionBackgroundView, 15),
                            imageView.AtRightOf(descriptionBackgroundView, 10),
                            imageView.Width().EqualTo(15),

                            btnDropdown.AtTopOf(descriptionBackgroundView, 0),
                            btnDropdown.AtLeftOf(descriptionBackgroundView, 0),
                            btnDropdown.AtRightOf(descriptionBackgroundView, 0),
                            btnDropdown.Height().EqualTo(30),

                            lblDescription.Below(lblDesTitle, lblDescriptBelowMargin),
                            lblDescription.AtLeftOf(descriptionBackgroundView, 10),
                            lblDescription.AtRightOf(descriptionBackgroundView, 10),
                            lblDescription.Height().EqualTo(1),

                            lblTypeOfMedicalProfessional.Below(descriptionBackgroundView, Constants.LABEL_TOP_PADDING),
                            lblTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.AtRightOf(View, Constants.LABEL_SIDE_PADDING),
                            lblTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            vewChooseTypeOfMedicalProfessional.Below(lblTypeOfMedicalProfessional, Constants.DROPDOWN_TOP_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtLeftOf(scrollableContainer, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.AtRightOf(View, Constants.DROPDOWN_SIDE_PADDING),
                            vewChooseTypeOfMedicalProfessional.WithSameWidth(scrollableContainer).Minus(Constants.DROPDOWN_SIDE_PADDING * 2),

                            btnContinue.Below(vewChooseTypeOfMedicalProfessional, 30),
                            btnContinue.WithSameCenterX(scrollableContainer),
                            btnContinue.Height().EqualTo(Constants.GREEN_BUTTON_HEIGHT),
                            btnContinue.Width().EqualTo(Constants.GREEN_BUTTON_WIDTH),
                            btnContinue.AtBottomOf(scrollableContainer, 100)
                        );
                    }
                }
            }
        }

        public override bool AutomaticallyAdjustsScrollViewInsets
        {
            get
            {
                return false;
            }
            set
            {
                base.AutomaticallyAdjustsScrollViewInsets = value;
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }


        void HandleVisibilityToggled(object sender, EventArgs e)
        {

        }

        private ObservableCollection<ExpenseType> _claimExpenseTypesCollection;
        public ObservableCollection<ExpenseType> ClaimExpenseTypesCollection
        {
            get
            {
                return _claimExpenseTypesCollection;
            }
            set
            {
                _claimExpenseTypesCollection = value;
                if (value != null)
                {
                    if (!Constants.IsPhone())
                    {
                        vewChooseTypeOfExpense.RemoveFromSuperview();
                        vewChooseTypeOfExpense = new HCSADropdownView(this);
                        vewChooseTypeOfExpense.COMPONENT_HEIGHT = 280f;
                        if (_model.ClaimExpenseTypes.Count == 3)
                        {
                            vewChooseTypeOfExpense.COMPONENT_HEIGHT = tableHeightFor3Items;
                        }
                        else if (_model.ClaimExpenseTypes.Count == 4)
                        {
                            vewChooseTypeOfExpense.COMPONENT_HEIGHT = tableHeightFor4Items;
                        }

                        scrollableContainer.AddSubview(vewChooseTypeOfExpense);
                        tableSourceExpense = new DismmssVCTableViewSource(vewChooseTypeOfExpense.popoverController, vewChooseTypeOfExpense.listController.tableView, "TypeOfExpenseTableViewCell", typeof(TypeOfExpenseTableViewCell));
                        vewChooseTypeOfExpense.listController.tableView.Source = tableSourceExpense;

                        var set = this.CreateBindingSet<ClaimTypeView, ClaimTypeViewModel>();
                        set.Bind(vewChooseTypeOfExpense).For(b => b.Hidden).To(vm => vm.IsExpenseTypeVisible).WithConversion("BoolOpposite");
                        set.Bind(tableSourceExpense).To(vm => vm.ClaimExpenseTypes);
                        set.Bind(tableSourceExpense).For(s => s.SelectedItem).To(vm => vm.SelectedClaimExpenseType);
                        set.Bind(tableSourceExpense).For(s => s.SelectionChangedCommand).To(vm => vm.SelectExpenseTypeWithoutNavigatingCommand);
                        set.Bind(vewChooseTypeOfExpense.selectedTypeLabel).To(vm => vm.SelectedClaimExpenseType.Name);
                        set.Apply();

                        vewChooseTypeOfClaim.listController.tableView.ReloadData();
                    }
                    SetConstraints();
                }
            }

        }
    }
}