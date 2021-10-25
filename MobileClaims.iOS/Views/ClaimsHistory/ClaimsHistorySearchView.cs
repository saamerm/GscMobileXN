using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MobileClaims.Core;
using MobileClaims.Core.Entities.ClaimsHistory;
using MobileClaims.Core.Services.ClaimsHistory;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MobileClaims.iOS.Views.ClaimsHistory
{
    [Register ("ClaimsHistorySearchView")]
    public class ClaimsHistorySearchView : GSCBaseViewController 
    {
        private UIScrollView rootScroll;
        private ClaimsSearchLabel claimTypeLab;
        private GreenNavButton claimsButt;
        private GreenNavButton dentalButt;
        private GreenNavButton medicalButt;  
        private ClaimsSearchLabel claimsPaidToLab;
        private UIButton payeeErrorButton;
        private ClaimForTxtLabel planMemberLab;
        private UISwitch planMemberSwitch;
        private ClaimForTxtLabel serviceProviderLab;
        private UISwitch serviceProviderSwitch;
        private ClaimsSearchLabel participantLab;
        private GSButtonGrayTextUpdate participantButt;
        private ClaimsSearchLabel benefitLab;
        private UIButton benefitErrorButton;
        private ClaimForTxtLabel allLab;
        private UISwitch allBenefitSwitch; 
        private ClaimsSearchLabel displayByLabel;
        private GSButtonGrayTextUpdate displayByButt;
        private GCButtonFixedFontSize updateButt;
        private UIView planMemberSwitchContainer;
        private UIView serviceProvSwitchContainer;
        private UIView allBenefitSwitchContainer;

        private UIView benefitsContainner;
        private ClaimForTxtLabel dentalLab;
        private UIView dentalSwitchContainer;
        private UISwitch dentalSwitch;
        private ClaimForTxtLabel drugLab;
        private UIView drugSwitchContainer;
        private UISwitch drugSwitch;
        private ClaimForTxtLabel ehsLab;
        private UIView ehsSwitchContainer;
        private UISwitch ehsSwitch;
        private ClaimForTxtLabel hcsaLab;
        private UIView hcsaSwitchContainer;
        private UISwitch hcsaSwitch;
        private ClaimForTxtLabel nohcsaLab;
        private UIView nohcsaSwitchContainer;
        private UISwitch nohcsaSwitch;  
        private ObservableCollection<ClaimHistoryToggleSubComponent> otherBenefits;
 
        private float topMargin=10f + Constants.NAV_HEIGHT; 
        private float leftMargin = 10f;
        private float belowMargin = 10f; 
        private float typeTabPercent=0.3333f;//because three buttons
        private float participantButtonHeight=50f;
        private float threeButtonsHeight=40f;
        private float threeButtonsSpace=6f;//value must be times of 3 because there are three buttons 
        private float planMemberSwitchHeight=30f;
        private float planMemberLabWidthPercent=0.83f;
        private float updateButtHeight=60f;
        private float benefitsContainnerHeight = 0f;//300f; 
        private float ToggleSubComponentHeight = 50f;
        private ClaimsHistorySearchViewModel model;
     
        public ClaimsHistorySearchView()
        {
            
        }

        public override void ViewDidLoad()
        {
            View = new GSCFluentLayoutBaseView(){ BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            model = ViewModel as ClaimsHistorySearchViewModel;

            base.NavigationController.NavigationBarHidden = false;  
            base.NavigationItem.Title = model.SearchCriteria.ToUpper();  
            base.NavigationController.NavigationBar.BackItem.Title = " ".tr();

            LayoutComponents();
            InitialBinding();
            SetConstraints();
        }

        private void LayoutComponents() 
        {  
            rootScroll = ((GSCFluentLayoutBaseView)View).baseScrollContainer; 
            rootScroll.UserInteractionEnabled = true;
            View.AddSubview(rootScroll);

            claimTypeLab=new ClaimsSearchLabel();
            claimTypeLab.Text = model.ClaimType.ToUpper();
            rootScroll.AddSubview(claimTypeLab);

            claimsButt = new GreenNavButton(false); 
			if(model.ClaimHistoryTypes.Count > 0)
            	claimsButt.SetTitle(model.ClaimHistoryTypes[0].Name,UIControlState .Normal);
			else 
				claimsButt.SetTitle("Claim" ,UIControlState .Normal);
            claimsButt.TouchUpInside += ClaimsButtonClick;
            rootScroll.AddSubview(claimsButt);

            dentalButt = new GreenNavButton(false);
            if(model.ClaimHistoryTypes.Count > 1)
                dentalButt.SetTitle(model.ClaimHistoryTypes[1].Name,UIControlState .Normal);
            else 
                dentalButt.SetTitle("Dental" ,UIControlState .Normal);
            dentalButt.TitleLabel.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)10f); 
            dentalButt.TouchUpInside += DentalButtonClick;
            rootScroll.AddSubview(dentalButt);

            medicalButt = new GreenNavButton(false);
            if(model.ClaimHistoryTypes.Count > 2)
                medicalButt.SetTitle(model.ClaimHistoryTypes[2].Name,UIControlState .Normal);
            else 
                medicalButt.SetTitle("Medical" ,UIControlState .Normal);
            medicalButt.TitleLabel.Font = UIFont.FromName (Constants.NUNITO_REGULAR, (nfloat)10f); 
            medicalButt.TouchUpInside += MedicalButtonClick;
            rootScroll.AddSubview(medicalButt);

            claimsPaidToLab = new ClaimsSearchLabel();
            claimsPaidToLab.Text = model.ClaimsPaidTo.ToUpper();
            rootScroll.AddSubview(claimsPaidToLab); 

            payeeErrorButton = new UIButton ();
            payeeErrorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            payeeErrorButton.AdjustsImageWhenHighlighted = true;
            payeeErrorButton.Alpha = 1;
            payeeErrorButton.TouchUpInside += HandleErrorButton;
            rootScroll.AddSubview (payeeErrorButton); 

            planMemberLab = new ClaimForTxtLabel();
            planMemberLab.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)12f); 
            rootScroll.AddSubview(planMemberLab);


            planMemberSwitchContainer = new UIView();  
            rootScroll.AddSubview(planMemberSwitchContainer);

            planMemberSwitch = new UISwitch();
            planMemberSwitch.OnTintColor = Colors.HIGHLIGHT_COLOR;
            planMemberSwitchContainer.AddSubview(planMemberSwitch); 

            serviceProviderLab = new ClaimForTxtLabel();
            serviceProviderLab.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)12f); 
            rootScroll.AddSubview(serviceProviderLab);

            serviceProvSwitchContainer = new UIView();
            rootScroll.AddSubview(serviceProvSwitchContainer);

            serviceProviderSwitch = new UISwitch();
            serviceProviderSwitch.OnTintColor =Colors.HIGHLIGHT_COLOR;
            serviceProvSwitchContainer.AddSubview(serviceProviderSwitch); 

            participantLab = new ClaimsSearchLabel();
            participantLab.Text = model.PlanMember.ToUpper();
            View.AddSubview(participantLab);

            participantButt = new GSButtonGrayTextUpdate(); 
            participantButt.SetTitle(model.SelectedParticipant.FullName,UIControlState.Normal);
            rootScroll.AddSubview(participantButt);

            benefitLab = new ClaimsSearchLabel();
            benefitLab.Text = model.Benefit.ToUpper();
            rootScroll.AddSubview(benefitLab);  

            benefitErrorButton = new UIButton ();
            benefitErrorButton.SetImage (UIImage.FromBundle("ErrorFlag"), UIControlState.Normal);
            benefitErrorButton.AdjustsImageWhenHighlighted = true;
            benefitErrorButton.Alpha = 1;
            benefitErrorButton.TouchUpInside += HandleBenefitErrorButton;
            rootScroll.AddSubview (benefitErrorButton);

            allLab = new ClaimForTxtLabel();
            allLab.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)12f); 
            rootScroll.AddSubview(allLab);

            allBenefitSwitchContainer = new UIView(); 
            rootScroll.AddSubview(allBenefitSwitchContainer);

            allBenefitSwitch = new UISwitch();
            allBenefitSwitch.OnTintColor=Colors.HIGHLIGHT_COLOR;
            allBenefitSwitchContainer.AddSubview(allBenefitSwitch);  
             
            LayoutBenefitsComponents();
              
            displayByLabel = new ClaimsSearchLabel();
            displayByLabel.Text = model.DisplayByLabel.ToUpper();
            rootScroll.AddSubview(displayByLabel);

            displayByButt = new GSButtonGrayTextUpdate(); 
            displayByButt.SetTitle(model.SelectedDisplayBy.Value.ToUpper() ,UIControlState.Normal);
            rootScroll.AddSubview(displayByButt);

            updateButt = new GCButtonFixedFontSize();
            updateButt.TitleLabel.Font = UIFont.FromName (Constants.LEAGUE_GOTHIC, (nfloat)Constants.HEADING_CLAIMTYPES);
            updateButt.SetTitle(model.Update.ToUpper(),UIControlState .Normal);
            rootScroll.AddSubview(updateButt); 
        }

        private void InitialBinding()
        {
            var set = this.CreateBindingSet <ClaimsHistorySearchView,ClaimsHistorySearchViewModel>();
            if (model.ClaimHistoryPayees != null && model.ClaimHistoryPayees.Count > 0)
            {
                set.Bind(planMemberLab).To(vm => vm.ClaimHistoryPayees[0].Name); 
                set.Bind(planMemberSwitch).To(vm => vm.ClaimHistoryPayees[0].IsSelected);
                if (model.ClaimHistoryPayees.Count > 1)
                {
                    set.Bind(serviceProviderLab).To(vm => vm.ClaimHistoryPayees[1].Name);
                    set.Bind(serviceProviderSwitch).To(vm => vm.ClaimHistoryPayees[1].IsSelected);
                }
            }
            set.Bind(serviceProviderSwitch).For(s => s.Selected).To(vm => vm.ClaimHistoryPayees).WithConversion("ClaimHistoryPayees", "S");
            set.Bind(claimsButt).For(b=>b.Selected ).To(vm=>vm.SelectedClaimHistoryType).WithConversion ("ClaimHistoryTypeHighlight","1");
            set.Bind(dentalButt).For(b=>b.Selected ).To(vm=>vm.SelectedClaimHistoryType).WithConversion ("ClaimHistoryTypeHighlight","2");
            set.Bind(medicalButt).For(b=>b.Selected ).To(vm=>vm.SelectedClaimHistoryType).WithConversion ("ClaimHistoryTypeHighlight","3");
            set.Bind(payeeErrorButton).For(b => b.Hidden ).To(vm => vm.NoPayeeSelected).WithConversion("BoolOpposite");
            set.Bind(claimsPaidToLab).For(l => l.TextColor).To(vm => vm.NoPayeeSelected).WithConversion("TextColorError"); 
            set.Bind(participantButt).To(vm => vm.SelectParticipantCommand); 
            set.Bind(benefitErrorButton).For(b => b.Hidden ).To(vm => vm.NoBenefitSelected).WithConversion("BoolOpposite");
            set.Bind(benefitLab).For(l => l.TextColor).To(vm => vm.NoBenefitSelected).WithConversion("TextColorError"); 
            set.Bind(allLab).To(vm => vm.AllClaimHistoryBenefit.Name);
            set.Bind(allBenefitSwitch).To(vm => vm.AllClaimHistoryBenefitIsSelected);
            set.Bind(allBenefitSwitch).For(c=>c.UserInteractionEnabled ).To(vm => vm.IsClaimHistoryBenefitEnabled); 
            set.Bind(benefitsContainner).For(v => v.Hidden).To(vm => vm.AllClaimHistoryBenefitIsSelected);
            set.Bind(this).For(v=>v.IsFullBenefitsVisible).To(vm => vm.IsFullBenefitsListVisible);
            set.Bind(this).For(v=>v.BenefitsWithoutAll).To(vm => vm.ClaimHistoryBenefitsWithoutAll);
            set.Bind(displayByButt).To(vm => vm.SelectDisplayByCommand);
            set.Bind(updateButt).To(vm => vm.SearchClaimsHistoryCommand); 
            set.Apply(); 
        }

        private void SetConstraints()
        {  
            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            rootScroll.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            planMemberSwitchContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            serviceProvSwitchContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            allBenefitSwitchContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            benefitsContainner.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints(); 

            if (Constants.IS_OS_VERSION_OR_LATER(11, 0))
            {
                View.AddConstraints(
                    rootScroll.AtTopOf(View, View.SafeAreaInsets.Top + 10),
                    rootScroll.AtLeftOf(View, View.SafeAreaInsets.Left + 10),
                    rootScroll.AtRightOf(View, View.SafeAreaInsets.Right + 10)
                    //rootScroll.WithSameWidth(View)
                );
            }
            else
            {
                View.AddConstraints(
                    rootScroll.AtTopOf(View, topMargin),
                    rootScroll.AtLeftOf(View, leftMargin),
                    rootScroll.WithSameWidth(View).Minus(leftMargin * 2)
                );
            }

            #region fixed problem of can't scroll to bottom to see the update button 
            if (Helpers.IsInLandscapeMode())
            {
                if (!Constants.IsPhone())
                {
                    View.AddConstraints(rootScroll.WithSameHeight(View).Minus((nfloat)(topMargin * 2)));
                }
                else
                {
                    View.AddConstraints(rootScroll.WithSameHeight(View).Minus((nfloat)(topMargin)));
                }
            }
            else
            {
                View.AddConstraints(rootScroll.WithSameHeight(View).Minus((nfloat)(topMargin)));
            }
            #endregion

            View.AddConstraints (
                claimTypeLab.WithSameTop(rootScroll),
                claimTypeLab.WithSameLeft(rootScroll),
                claimTypeLab.WithSameWidth(rootScroll), 
                 
                claimsButt.WithSameLeft(rootScroll), 
                claimsButt.WithRelativeWidth(rootScroll, typeTabPercent).Minus(threeButtonsSpace * 2 / 3),
                claimsButt.Height().EqualTo(threeButtonsHeight),
                claimsButt.Below(claimTypeLab, belowMargin),

                dentalButt.WithSameCenterY(claimsButt),
                dentalButt.ToRightOf(claimsButt).Plus(threeButtonsSpace),
                dentalButt.WithSameWidth(claimsButt), 
                dentalButt.WithSameHeight(claimsButt),

                medicalButt.WithSameCenterY(dentalButt),
                medicalButt.ToRightOf(dentalButt).Plus(threeButtonsSpace),
                medicalButt.WithSameWidth(dentalButt),
                medicalButt.WithSameHeight(dentalButt),

                claimsPaidToLab.WithSameLeft(rootScroll), 
                claimsPaidToLab.Below(claimsButt, belowMargin * 2),

                payeeErrorButton.ToRightOf(claimsPaidToLab),
                payeeErrorButton.WithSameCenterY(claimsPaidToLab),

                planMemberLab.WithSameLeft(claimsPaidToLab),
                planMemberLab.WithRelativeWidth(rootScroll, planMemberLabWidthPercent),
                planMemberLab.Below(claimsPaidToLab, belowMargin), 

                planMemberSwitchContainer.WithSameCenterY(planMemberLab),
                planMemberSwitchContainer.WithRelativeWidth(rootScroll, (1 - planMemberLabWidthPercent)),
                planMemberSwitchContainer.Height().EqualTo(planMemberSwitchHeight),  
                planMemberSwitchContainer.ToRightOf(planMemberLab),
               
                planMemberSwitch.WithSameCenterY(planMemberSwitchContainer),
                planMemberSwitch.WithSameRight(planMemberSwitchContainer).Minus(2f), 

                serviceProviderLab.WithSameLeft(planMemberLab),
                serviceProviderLab.WithRelativeWidth(rootScroll, planMemberLabWidthPercent),
                serviceProviderLab.Below(planMemberLab, belowMargin * 3),

                serviceProvSwitchContainer.WithSameCenterY(serviceProviderLab),
                serviceProvSwitchContainer.WithRelativeWidth(rootScroll, (1 - planMemberLabWidthPercent)),
                serviceProvSwitchContainer.Height().EqualTo(planMemberSwitchHeight),  
                serviceProvSwitchContainer.ToRightOf(serviceProviderLab),

                serviceProviderSwitch.WithSameCenterY(serviceProvSwitchContainer),
                serviceProviderSwitch.WithSameRight(serviceProvSwitchContainer).Minus(2f),  
                 
                participantLab.WithSameLeft(serviceProviderLab), 
                participantLab.Below(serviceProviderLab, belowMargin * 3),

                participantButt.WithSameLeft(serviceProviderLab),
                participantButt.WithSameWidth(rootScroll),
                participantButt.Height().EqualTo(participantButtonHeight),
                participantButt.Below(participantLab),

                benefitLab.WithSameLeft(participantButt),
                benefitLab.Below(participantButt, belowMargin),

                benefitErrorButton.ToRightOf(claimsPaidToLab),
                benefitErrorButton.WithSameCenterY(benefitLab),


                allLab.WithSameLeft(benefitLab), 
                allLab.WithRelativeWidth(rootScroll, planMemberLabWidthPercent),
                allLab.Below(benefitLab, belowMargin),

                allBenefitSwitchContainer.WithSameCenterY(allLab),
                allBenefitSwitchContainer.WithRelativeWidth(rootScroll, (1 - planMemberLabWidthPercent)),
                allBenefitSwitchContainer.Height().EqualTo(planMemberSwitchHeight),  
                allBenefitSwitchContainer.ToRightOf(serviceProviderLab),

                allBenefitSwitch.WithSameCenterY(allBenefitSwitchContainer),
                allBenefitSwitch.WithSameRight(allBenefitSwitchContainer).Minus(2f),  

                benefitsContainner.AtLeftOf(rootScroll, 0),
                benefitsContainner.WithSameWidth(rootScroll),
                benefitsContainner.Height().EqualTo(benefitsContainnerHeight),
                benefitsContainner.Below(allLab, belowMargin * 3),
                   
                displayByLabel.WithSameLeft(allLab),
                displayByLabel.WithSameWidth(rootScroll),
                displayByLabel.Below(benefitsContainner, belowMargin),

                displayByButt.WithSameLeft(displayByLabel),
                displayByButt.WithSameWidth(rootScroll),
                displayByButt.WithSameHeight(participantButt), 
                displayByButt.Below(displayByLabel),

                updateButt.WithSameLeft(displayByButt),
                updateButt.WithSameWidth(rootScroll),
                updateButt.Height().EqualTo(updateButtHeight),
                updateButt.Below(displayByButt, (nfloat)belowMargin * 2),
                updateButt.AtBottomOf(rootScroll, belowMargin * 2)
            );
            if (otherBenefits != null)
            {
                int totalComponents = otherBenefits.Count;
                if (totalComponents > 0)
                {
                    for (int i = 0; i < totalComponents; i++)
                    {
                        ClaimHistoryToggleSubComponent component=  otherBenefits[i] as ClaimHistoryToggleSubComponent;
                        View.AddConstraints(
                            component.AtTopOf(benefitsContainner,50f*i),
                            component .AtLeftOf(benefitsContainner,0),
                            component .WithSameWidth(benefitsContainner),
                            component.Height().EqualTo(50f)

                        );
                    }
                }
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }

        private void ClaimsButtonClick(object sender, EventArgs e)
        {   
            model.SelectedClaimHistoryType = model.ClaimHistoryTypes[0];
            benefitsContainner.RemoveFromSuperview();
            LayoutBenefitsComponents(); 
            var set = this.CreateBindingSet <ClaimsHistorySearchView,ClaimsHistorySearchViewModel>();
            set.Bind(benefitsContainner).For(v => v.Hidden).To(vm => vm.AllClaimHistoryBenefitIsSelected);
            benefitsContainnerHeight = (model.AllClaimHistoryBenefitIsSelected) ? 0f : ((model.ClaimHistoryBenefitsWithoutAll != null) ? (ToggleSubComponentHeight * model.ClaimHistoryBenefitsWithoutAll.Count) : 0f);
            set.Apply(); 
            SetConstraints(); 
        } 

        private void DentalButtonClick(object sender, EventArgs e)
        { 
            model.SelectedClaimHistoryType = model.ClaimHistoryTypes[1]; 
            benefitsContainner.RemoveFromSuperview();
            LayoutBenefitsComponents();
            IClaimsHistoryService sv = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();
            displayByButt.SetTitle(sv.SelectedDisplayBy.Value.ToUpper(), UIControlState.Normal); 
            SetConstraints();
        }

        private void LayoutBenefitsComponents()
        {
            benefitsContainner = new UIView();
            benefitsContainner.UserInteractionEnabled = true; 
            rootScroll.AddSubview(benefitsContainner);  
         
            if (model.ClaimHistoryBenefitsWithoutAll != null)
            {
                int totalOtherBenefits = model.ClaimHistoryBenefitsWithoutAll.Count;
                if (totalOtherBenefits > 0)
                {
                    ObservableCollection<ClaimHistoryBenefit> coll = model.ClaimHistoryBenefitsWithoutAll as ObservableCollection<ClaimHistoryBenefit>;
                    otherBenefits = new ObservableCollection<ClaimHistoryToggleSubComponent>();
                    for (int i = 0; i < totalOtherBenefits; i++)
                    { 
                        ClaimHistoryBenefit chb = coll[i] as ClaimHistoryBenefit; 
                        ClaimHistoryToggleSubComponent chbtc = new ClaimHistoryToggleSubComponent(chb.Name);
                        chbtc.toggleSwitch.On = chb.IsSelected;
                        allLab.TextColor = Colors.Black;
                        if (model.SelectedClaimHistoryType != null)
                        {
                            if (model.SelectedClaimHistoryType.ID == GSCHelper.ClaimHistoryDentalPredeterminationID
                                    || model.SelectedClaimHistoryType.ID == GSCHelper.ClaimHistoryMedicalItemID)
                            {
                                allLab.TextColor = Colors.MED_GREY_COLOR;
                                if ((chb.IsSelected))
                                { 
                                    chbtc.toggleSwitch.OnTintColor = Colors.DARK_GREY_COLOR;
                                } 
                                chbtc.typeName.TextColor = Colors.MED_GREY_COLOR;
                            }
                        } 
                        chbtc.toggleSwitch.UserInteractionEnabled = model.IsClaimHistoryBenefitEnabled;
                        chbtc.VisibilityToggled += BenefitToggle;
                        benefitsContainner.AddSubview(chbtc);
                        otherBenefits.Add(chbtc);
                        ToggleSubComponentHeight = chbtc.componetHeight;
                    }
                    if (!model.AllClaimHistoryBenefitIsSelected)
                    {
                        benefitsContainnerHeight = ToggleSubComponentHeight * (totalOtherBenefits);
                    }
                    else
                    {
                        benefitsContainnerHeight = 0f;
                        benefitsContainner.Hidden = true;
                    }
                } 
            } 
            
        }
        

        private void MedicalButtonClick(object sender, EventArgs e)
        { 
            model.SelectedClaimHistoryType = model.ClaimHistoryTypes[2];
            benefitsContainner.RemoveFromSuperview(); 
            LayoutBenefitsComponents(); 
            IClaimsHistoryService sv = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();
            displayByButt.SetTitle(sv.SelectedDisplayBy.Value.ToUpper(), UIControlState.Normal); 
            SetConstraints();
        }


        private void BenefitToggle(object sender, EventArgs e)
        {
            ClaimHistoryToggleSubComponent component = sender as ClaimHistoryToggleSubComponent; 
            bool allSelected = true;
            foreach (ClaimHistoryBenefit chb in model.ClaimHistoryBenefitsWithoutAll)
            {
                if (chb.Name== component .typeName .Text)
                {
                    chb.IsSelected = component.toggleSwitch.On;  
                } 
                allSelected = allSelected && chb.IsSelected;
            } 
            if (allSelected)
                model.AllClaimHistoryBenefitIsSelected = allSelected;
        }

        #region caculate the benefits containner height because iOS is not support multiple binding
        private bool _isFullBenefitsVisible;
        public bool IsFullBenefitsVisible
        {
            get { return _isFullBenefitsVisible; }
            set
            {
                if (_isFullBenefitsVisible != value)
                {
                    _isFullBenefitsVisible = value;
                    benefitsContainnerHeight = 0f;
                    if (value)
                    {
                        if (BenefitsWithoutAll != null)
                        {
                            if (BenefitsWithoutAll.Count > 0)
                            {
                                benefitsContainnerHeight = ToggleSubComponentHeight * BenefitsWithoutAll.Count;
                            }
                        }
                    }
                    benefitsContainner.RemoveFromSuperview();
                    LayoutBenefitsComponents();
                    SetConstraints();
                }
            }
        }

        private ObservableCollection<ClaimHistoryBenefit> _benefitsWithoutAll;
        public ObservableCollection<ClaimHistoryBenefit> BenefitsWithoutAll
        {
            get { return _benefitsWithoutAll; }
            set
            {
                if (_benefitsWithoutAll != value)
                {
                    _benefitsWithoutAll = value; 
                    benefitsContainnerHeight = 0f;
                    if (_benefitsWithoutAll != null)
                    {
                        if (_benefitsWithoutAll.Count > 0)
                        {
                            if (IsFullBenefitsVisible)
                            {
                                benefitsContainnerHeight = ToggleSubComponentHeight * _benefitsWithoutAll.Count;
                            }
                        }
                    }
                    SetConstraints();
                        
                }
            }
        } 
        #endregion


        protected void HandleErrorButton (object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView ("", model.PayeeValidationMessage , null, "ok".tr(), null);

            _error.Show ();
        }


        protected void HandleBenefitErrorButton (object sender, EventArgs e)
        {
            UIAlertView _error = new UIAlertView ("", model.BenefitValidationMessage , null, "ok".tr(), null);

            _error.Show ();
        }

        private void Model_CloseClaimsHistorySearchVM(object sender, EventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                if (base.NavigationController != null)
                {
                    if (base.NavigationController.ViewControllers != null)
                    {
                        base.NavigationController.PopViewController(true);
                    }
                }
            });
        }

        public override void ViewWillAppear(bool animated)
        {
            NavigationController.NavigationBar.BackItem.Title = " ".tr();
            base.NavigationItem.Title = model.SearchCriteria.ToUpper(); 

            IClaimsHistoryService sv = Mvx.IoCProvider.Resolve<IClaimsHistoryService>();
            displayByButt.SetTitle(sv.SelectedDisplayBy.Value.ToUpper(), UIControlState.Normal);
            participantButt.SetTitle(sv.SelectedParticipant.FullName,UIControlState.Normal);

            model.CloseClaimsHistorySearchVM += Model_CloseClaimsHistorySearchVM;
            model.PropertyChanged += Model_PropertyChanged;

            base.ViewWillAppear(animated);
        } 

		public override void ViewWillDisappear (bool animated)
		{
            base.ViewWillDisappear (animated);
            model.CloseClaimsHistorySearchVM -= Model_CloseClaimsHistorySearchVM;
            model.PropertyChanged -= Model_PropertyChanged;
            model.IsRightSideGreyedOut = false;
		}

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AllClaimHistoryBenefitIsSelected")
            {
                benefitsContainnerHeight = model.AllClaimHistoryBenefitIsSelected ? 0f : ((model.ClaimHistoryBenefitsWithoutAll==null) ?0f:ToggleSubComponentHeight * model.ClaimHistoryBenefitsWithoutAll.Count); 
                SetConstraints();
            }
        }
    }

  }

