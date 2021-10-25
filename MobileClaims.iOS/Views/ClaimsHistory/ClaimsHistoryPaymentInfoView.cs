using System;
using MobileClaims.Core.ViewModels.ClaimsHistory;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.ClaimsHistory;
using UIKit;
using MobileClaims.Core.Services;
using MobileClaims.iOS.UI;
using MvvmCross;
using MvvmCross.Binding.BindingContext;

namespace MobileClaims.iOS.Views.ClaimsHistory
{
    public class ClaimsHistoryPaymentInfoView : GSCBaseViewController
    { 
        private UIScrollView rootScrollView;
        private NunitoSemiBold1315Label GreenShieldIDNumberLab; //#1
        private LabelNunitoSemiBold13_NunitoBold12 GreenShieldIDNumberTxt; 
        private NunitoSemiBold1315Label PaymentMethodLab;//#2
        private LabelNunitoSemiBold13_NunitoBold12 PaymentMethodTxt;
        #region EFT
        private NunitoSemiBold1315Label DirectDepositNumberLab;//#3
        private LabelNunitoSemiBold13_NunitoBold12 DirectDepositNumberTxt;
        private NunitoSemiBold1315Label StatementDateLab;//#6
        private LabelNunitoSemiBold13_NunitoBold12 StatementDateTxt;
        private NunitoSemiBold1315Label DepositDateLab;//#7
        private LabelNunitoSemiBold13_NunitoBold12 DepositDateTxt;
        #endregion  
        #region Cheque 
        private NunitoSemiBold1315Label ChequeNumberLab;//#3
        private LabelNunitoSemiBold13_NunitoBold12 ChequeNumberTxt;
        private NunitoSemiBold1315Label ChequeStatusLab;// #6
        private LabelNunitoSemiBold13_NunitoBold12 ChequeStatusTxt;
        private NunitoSemiBold1315Label paymentDateLab;// #7
        private LabelNunitoSemiBold13_NunitoBold12 paymentDateTxt;

        private NunitoSemiBold1315Label CashedDateLab;//#8
        private LabelNunitoSemiBold13_NunitoBold12 CashedDateTxt;
        #endregion 
        private NunitoSemiBold1315Label PaymentAmountLab;//#4
        private LabelNunitoSemiBold13_NunitoBold12 PaymentAmountTxt;
        private NunitoSemiBold1315Label PaymentCurrencyLab;//#5
        private LabelNunitoSemiBold13_NunitoBold12 PaymentCurrencyTxt;
     
        private NunitoSemiBold1315Label DateOfInquiryLab;//#9
        private LabelNunitoSemiBold13_NunitoBold12 DateOfInquiryTxt;
        private NunitoSemiBold1315Label errorMessageLab; 

        private ClaimsHistoryPaymentInfoViewModel model;
        private bool hasSearchResult;

        private float topMargin = Helpers.BottomNavHeight() + 20f;
        private float leftMargin=10f;
        private float belowMargin=10f;
        private float leftSidePercentage=0.5f;  
        private float rightMargin=10f; 
        private float bottomMargin= Helpers.BottomNavHeight() +10f ;
        private string language; 
        private float topAdjustment=4f;

        public ClaimsHistoryPaymentInfoView()
        {
        }

        public override void ViewDidLoad()
        {  
            View = new GSCFluentLayoutBaseView(){ BackgroundColor = Colors.BACKGROUND_COLOR };
            base.ViewDidLoad();
            model = this.ViewModel as ClaimsHistoryPaymentInfoViewModel; 
   
            base.NavigationController.NavigationBarHidden = false;
            base.NavigationItem.SetHidesBackButton (false, false);
            base.NavigationItem.Title = model.PaymentInformation.ToUpper();
            UIBarButtonItem barButton = new UIBarButtonItem();
            barButton.Title = " ".tr();  
            this.NavigationController.NavigationBar.TopItem.BackBarButtonItem = barButton;
            ILanguageService langService= Mvx.IoCProvider.Resolve<ILanguageService>();
            language = langService.CurrentLanguage;

            rootScrollView = ((GSCFluentLayoutBaseView)View).baseScrollContainer;
            rootScrollView.BackgroundColor = Colors.BACKGROUND_COLOR;
            rootScrollView.ScrollEnabled = true; 
            View.Add(rootScrollView);

            #region LayoutElements
            GreenShieldIDNumberLab=new NunitoSemiBold1315Label ();
            GreenShieldIDNumberLab.Text = model.GreenShieldIDNumber;
            rootScrollView.AddSubview(GreenShieldIDNumberLab);

            GreenShieldIDNumberTxt=new LabelNunitoSemiBold13_NunitoBold12 ();  
            rootScrollView.AddSubview(GreenShieldIDNumberTxt);
             
            PaymentMethodLab=new NunitoSemiBold1315Label ();
            PaymentMethodLab.Text = model.PaymentMethod;
            rootScrollView.AddSubview(PaymentMethodLab);

            PaymentMethodTxt=new LabelNunitoSemiBold13_NunitoBold12(); 
            rootScrollView.AddSubview(PaymentMethodTxt);

            DirectDepositNumberLab=new NunitoSemiBold1315Label ();
            DirectDepositNumberLab.Text= model.DirectDepositNumber; 
            rootScrollView.AddSubview(DirectDepositNumberLab);

            ChequeNumberLab=new NunitoSemiBold1315Label ();
            ChequeNumberLab.Text=model.ChequeNumberLabel;
            rootScrollView.AddSubview(ChequeNumberLab);

            ChequeNumberTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(ChequeNumberTxt); 

            DirectDepositNumberTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(DirectDepositNumberTxt);

            PaymentAmountLab=new NunitoSemiBold1315Label ();
            PaymentAmountLab.Text = model.PaymentAmount;
            rootScrollView.AddSubview(PaymentAmountLab);

            PaymentAmountTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(PaymentAmountTxt);

            PaymentCurrencyLab=new NunitoSemiBold1315Label ();
            PaymentCurrencyLab.Text = model.PaymentCurrency;
            rootScrollView.AddSubview(PaymentCurrencyLab);

            PaymentCurrencyTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(PaymentCurrencyTxt);  

            StatementDateLab=new NunitoSemiBold1315Label ();
            StatementDateLab.Text = model.StatementDate;
            rootScrollView.AddSubview(StatementDateLab);

            StatementDateTxt=new LabelNunitoSemiBold13_NunitoBold12 (); 
            rootScrollView.AddSubview(StatementDateTxt);

            ChequeStatusLab =new NunitoSemiBold1315Label ();
            ChequeStatusLab .Text =model.ChequeStatusLabel;
            rootScrollView.AddSubview(ChequeStatusLab);

            ChequeStatusTxt =new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(ChequeStatusTxt); 

            DepositDateLab=new NunitoSemiBold1315Label ();
            DepositDateLab.Text = model.DepositDate;
            rootScrollView.AddSubview(DepositDateLab);

            DepositDateTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(DepositDateTxt);

            paymentDateLab=new NunitoSemiBold1315Label ();
            paymentDateLab .Text=model.PaymentDateLabel;
            rootScrollView.AddSubview(paymentDateLab);

            paymentDateTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview (paymentDateTxt );

            CashedDateLab=new NunitoSemiBold1315Label();
            CashedDateLab.Text=model.CashedDateLabel;
            rootScrollView.AddSubview(CashedDateLab);

            CashedDateTxt =new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(CashedDateTxt);

            DateOfInquiryLab=new NunitoSemiBold1315Label ();
            DateOfInquiryLab.Text = model.DateOfInquiryLabel;
            rootScrollView.AddSubview(DateOfInquiryLab);

            DateOfInquiryTxt=new LabelNunitoSemiBold13_NunitoBold12 ();
            rootScrollView.AddSubview(DateOfInquiryTxt);

            errorMessageLab=new NunitoSemiBold1315Label ();
            errorMessageLab.Text=model.ErrorMessage;
            rootScrollView.AddSubview(errorMessageLab);

            #endregion

            #region SetBinding
            var set = this.CreateBindingSet<ClaimsHistoryPaymentInfoView,ClaimsHistoryPaymentInfoViewModel>();
            set.Bind(GreenShieldIDNumberTxt).For(l=>l.TextContent).To(vm=>vm.SearchResult. Payment.PlanMemberDisplayID);
            set.Bind(PaymentMethodTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.PaymentMethod);
            set.Bind(DirectDepositNumberLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsDepositNumberVisible).WithConversion("BoolOpposite");
            set.Bind(DirectDepositNumberTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.DepositNumber);
            set.Bind(DirectDepositNumberTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsDepositNumberVisible).WithConversion("BoolOpposite");
            set.Bind(ChequeNumberLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsChequeNumberVisible).WithConversion("BoolOpposite");
            set.Bind(ChequeNumberTxt).For(l =>l.TextContent).To (vm=>vm.SearchResult.Payment.ChequeNumber);//CH data
            set.Bind(ChequeNumberTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsChequeNumberVisible).WithConversion("BoolOpposite");
            set.Bind(PaymentAmountTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.PaymentAmountString).WithConversion("DollarSignPrefix"); //("DollarSignDoublePrefix"); 
            set.Bind(PaymentCurrencyTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.Currency);
            set.Bind(StatementDateLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsStatementDateVisible).WithConversion("BoolOpposite");
            set.Bind(StatementDateTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.StatementDate).WithConversion("DefaultDateBlank");
            set.Bind(StatementDateTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsStatementDateVisible).WithConversion("BoolOpposite");
            set.Bind(ChequeStatusLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsStatusVisible).WithConversion("BoolOpposite"); 
            set.Bind(ChequeStatusTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.Status);//CH data
            set.Bind(ChequeStatusTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsStatusVisible).WithConversion("BoolOpposite");
            set.Bind(DepositDateLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsDepositDateVisible).WithConversion("BoolOpposite");
            set.Bind(DepositDateTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.DepositDate).WithConversion("DefaultDateBlank");
            set.Bind(DepositDateTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsDepositDateVisible).WithConversion("BoolOpposite");
            set.Bind(paymentDateLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsPaymentDateVisible).WithConversion("BoolOpposite");
            set.Bind(paymentDateTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.PaymentDate).WithConversion("DefaultDateBlank");//CH data
            set.Bind(paymentDateTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsPaymentDateVisible).WithConversion("BoolOpposite");
            set.Bind(CashedDateLab).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsCashedDateVisible).WithConversion("BoolOpposite");
            set.Bind(CashedDateTxt).For(l =>l.TextContent).To(vm=>vm.SearchResult.Payment.CashedDate).WithConversion("DefaultDateBlank");//CH data
            set.Bind(CashedDateTxt).For(l=>l.Hidden).To(vm=>vm.SearchResult.Payment.IsCashedDateVisible).WithConversion("BoolOpposite");
            set.Bind(DateOfInquiryTxt).For(l =>l.TextContent).To(vm=>vm.DateOfInquiry); 
            set.Bind(this).For(v=>v.SearchResultValue).To(vm=>vm.SearchResult);
            set.Apply();

            #endregion

            SetConstraints(); 
        }

        private void SetConstraints()
        {  
            SetMargin();
            View.RemoveConstraints(View.Constraints);
            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            rootScrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            if (hasSearchResult)
            {

                if(Constants.IS_OS_VERSION_OR_LATER(11, 0))
                {
                    View.AddConstraints(

    rootScrollView.AtTopOf(View, View.SafeAreaInsets.Top + 10),
    rootScrollView.AtLeftOf(View, View.SafeAreaInsets.Left),
    rootScrollView.AtRightOf(View, View.SafeAreaInsets.Right),
    rootScrollView.WithSameHeight(View).Minus(topMargin + bottomMargin + belowMargin * 2)


);

                    View.AddConstraints(

                        GreenShieldIDNumberLab.AtTopOf(rootScrollView, leftMargin),
                        GreenShieldIDNumberLab.AtLeftOf(rootScrollView, leftMargin),
                        GreenShieldIDNumberLab.WithRelativeWidth(rootScrollView, leftSidePercentage).Minus(leftMargin),

                        GreenShieldIDNumberTxt.ToRightOf(GreenShieldIDNumberLab, (nfloat)(leftMargin * 1.5)),
                        GreenShieldIDNumberTxt.WithRelativeWidth(rootScrollView, (1 - leftSidePercentage)).Minus((nfloat)(leftMargin * 2.5)),
                        GreenShieldIDNumberTxt.WithSameTop(GreenShieldIDNumberLab).Plus(topAdjustment),

                        PaymentMethodLab.WithSameLeft(GreenShieldIDNumberLab),
                        PaymentMethodLab.WithSameWidth(GreenShieldIDNumberLab)
                    );
                }
                else 
                {
                    View.AddConstraints(
                    rootScrollView.AtTopOf(View, topMargin),
                    rootScrollView.AtLeftOf(View),
                    rootScrollView.WithSameWidth(View),
                    rootScrollView.WithSameHeight(View).Minus(topMargin + bottomMargin + belowMargin * 2),

                    GreenShieldIDNumberLab.AtTopOf(rootScrollView, leftMargin),
                    GreenShieldIDNumberLab.AtLeftOf(rootScrollView, leftMargin),
                    GreenShieldIDNumberLab.WithRelativeWidth(rootScrollView, leftSidePercentage).Minus(leftMargin),

                    GreenShieldIDNumberTxt.ToRightOf(GreenShieldIDNumberLab, (nfloat)(leftMargin * 1.5)),
                    GreenShieldIDNumberTxt.WithRelativeWidth(rootScrollView, (1 - leftSidePercentage)).Minus((nfloat)(leftMargin * 2.5)),
                    GreenShieldIDNumberTxt.WithSameTop(GreenShieldIDNumberLab).Plus(topAdjustment),

                    PaymentMethodLab.WithSameLeft(GreenShieldIDNumberLab),
                    PaymentMethodLab.WithSameWidth(GreenShieldIDNumberLab)
                );
                }



                if (!string.IsNullOrEmpty(model.GreenShieldIDNumber))
                {
                    if (Helpers.IsInLandscapeMode())//label only one line
                    {    
                        View.AddConstraints(PaymentMethodLab.Below(GreenShieldIDNumberTxt, belowMargin)); 
                    }
                    else //label has two lines in French
                    {
                        if (language.Contains("fr") || language.Contains("Fr"))
                        {   
                            int MaxmumCharactersInOneLine = Constants.CLAIMHISTORY_PAYMENT_IDNUMBERFIELD_LIMATELENGTHPORTRAIT;
                            if (model.GreenShieldIDNumber.Length >MaxmumCharactersInOneLine * 2)
                            {
                                View.AddConstraints(PaymentMethodLab.Below(GreenShieldIDNumberTxt, belowMargin)); 
                            }
                            else
                            {
                                View.AddConstraints(PaymentMethodLab.Below(GreenShieldIDNumberLab, belowMargin));
                            }
                        }
                        else
                        {    
                            View.AddConstraints(PaymentMethodLab.Below(GreenShieldIDNumberTxt, belowMargin)); 
                        }
                    }
                }
                else
                {
                    View.AddConstraints(PaymentMethodLab.Below(GreenShieldIDNumberLab, belowMargin));  
                }

                View.AddConstraints(
                    PaymentMethodTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    PaymentMethodTxt.WithSameTop(PaymentMethodLab).Plus(topAdjustment), 

                    DirectDepositNumberLab.WithSameLeft(GreenShieldIDNumberLab),
                    DirectDepositNumberLab.WithSameWidth(PaymentMethodLab),
                    DirectDepositNumberLab.Below(PaymentMethodLab, belowMargin), 

                    DirectDepositNumberTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    DirectDepositNumberTxt.WithSameTop(DirectDepositNumberLab).Plus(topAdjustment), 

                    ChequeNumberLab.WithSameLeft(PaymentMethodLab),
                    ChequeNumberLab.WithSameWidth(PaymentMethodLab),
                    ChequeNumberLab.Below(PaymentMethodLab, belowMargin), 

                    ChequeNumberTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    ChequeNumberTxt.WithSameTop(DirectDepositNumberLab).Plus(topAdjustment),  
                     
                    PaymentAmountLab.WithSameLeft(PaymentMethodLab),
                    PaymentAmountLab.WithSameWidth(PaymentMethodLab),
                    PaymentAmountLab.Below(DirectDepositNumberLab, belowMargin), 

                    PaymentAmountTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    PaymentAmountTxt.WithSameWidth(GreenShieldIDNumberTxt),
                    PaymentAmountTxt.WithSameTop(PaymentAmountLab).Plus(topAdjustment), 

                    PaymentCurrencyLab.WithSameLeft(PaymentMethodLab),
                    PaymentCurrencyLab.WithSameWidth(PaymentMethodLab),
                    PaymentCurrencyLab.Below(PaymentAmountTxt, belowMargin), 

                    PaymentCurrencyTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    PaymentCurrencyTxt.WithSameTop(PaymentCurrencyLab).Plus(topAdjustment), 

                    StatementDateLab.WithSameLeft(PaymentMethodLab),
                    StatementDateLab.WithSameWidth(PaymentMethodLab),
                    StatementDateLab.Below(PaymentCurrencyLab, belowMargin), 

                    StatementDateTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    StatementDateTxt.WithSameTop(StatementDateLab).Plus(topAdjustment),

                    ChequeStatusLab.WithSameLeft(PaymentMethodLab),
                    ChequeStatusLab.WithSameWidth(PaymentMethodLab),
                    ChequeStatusLab.Below(PaymentCurrencyLab, belowMargin), 

                    ChequeStatusTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    ChequeStatusTxt.WithSameTop(StatementDateLab).Plus(topAdjustment), 
                     
                    DepositDateLab.WithSameLeft(PaymentMethodLab),
                    DepositDateLab.WithSameWidth(PaymentMethodLab),
                    DepositDateLab.Below(StatementDateLab, belowMargin),  

                    DepositDateTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    DepositDateTxt.WithSameTop(DepositDateLab).Plus(topAdjustment), 

                    paymentDateLab.WithSameLeft(DepositDateLab),
                    paymentDateLab.WithSameWidth(PaymentMethodLab),
                    paymentDateLab.Below(ChequeStatusLab, belowMargin),  

                    paymentDateTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    paymentDateTxt.WithSameTop(paymentDateLab).Plus(topAdjustment), 

                    CashedDateLab.WithSameLeft(PaymentMethodLab),
                    CashedDateLab.WithSameWidth(PaymentMethodLab),
                    CashedDateLab.Below(paymentDateLab, belowMargin),  

                    CashedDateTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    CashedDateTxt.WithSameTop(CashedDateLab).Plus(topAdjustment),

                    DateOfInquiryLab.WithSameLeft(PaymentMethodLab),
                    DateOfInquiryLab.WithSameWidth(PaymentMethodLab), 

                    DateOfInquiryTxt.WithSameLeft(GreenShieldIDNumberTxt),
                    DateOfInquiryTxt.WithSameTop(DateOfInquiryLab).Plus(topAdjustment),
                    DateOfInquiryTxt.WithSameWidth(GreenShieldIDNumberTxt)
                );
                if (!DepositDateLab.Hidden) // Date of Inquiry will be diffrent location base on EFT and CH
                { 
                    View.AddConstraints(DateOfInquiryLab.Below(DepositDateLab, belowMargin)); 
                }
                else
                {  
                    View.AddConstraints(DateOfInquiryLab.Below(CashedDateLab, belowMargin));
                } 
                View.AddConstraints(DateOfInquiryLab.AtBottomOf (rootScrollView ));

            }
            else
            {
                View.AddConstraints(
                    errorMessageLab.AtTopOf(View,topMargin),
                    errorMessageLab.AtLeftOf(View,leftMargin), 
                    errorMessageLab.WithSameWidth(View ).Minus(leftMargin*2),
                    errorMessageLab.WithSameTop(View)
                   
                );
            }
        }

        private ClaimState _searchResultValue;
        public ClaimState SearchResultValue
        {
            get { return _searchResultValue; }
            set
            {
                if (_searchResultValue != value)
                {
                    _searchResultValue = value;

                }
                hasSearchResult = (value == null) ? false : true;
                SetConstraints();
            }
        }
        public override void ViewWillAppear(bool animated) 
        {
            base.NavigationItem.Title = model.PaymentInformation.ToUpper();
            base.NavigationController.NavigationBar.TopItem.BackBarButtonItem .Title =" ".tr();  
            base.ViewWillAppear(animated);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            SetConstraints();
        }

        private void SetMargin()
        {
            topMargin = Helpers.BottomNavHeight() + 20f;
            if (Constants.IsPhone())
            {
                if (Helpers.IsInLandscapeMode())
                    topMargin = Helpers.BottomNavHeight() - 15f;
            }
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        { 
            SetConstraints();
            base.DidRotate(fromInterfaceOrientation);
        }
    }
}

