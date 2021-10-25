using System;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.ClaimsHistory;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;


namespace MobileClaims.iOS
{ 
    [Foundation .Register ("ClaimHistoryCountTableViewCell")]
    public class ClaimHistoryCountTableViewCell : MvxTableViewCell 
	{
        private CountTableViewCellLabel _itemlabel; 
        public CountTableViewCellGreenLabel _itemButton;
        public  UIView _spacing, countSpacing; 
  
        private float itemsLeftMargin=15f; 
        private float itemsBelowMargin=5f;
        private float rightSidePercentage = 0.32f;//0.307f;
        private float cellSpaceHeight=10f;
        private float cellHeight=Constants.ClaimHistoryCountTableViewCellHeight;
        private float claimCountlabelHeight=40f;
        private float claimCountlabelWidth=73f;
        private float spaceBetweenLabels=15f;
        private  float cellBorderWith=5f;
         
        private bool _isCellSelected;
        public bool IsCellSelected
        {
            get
            { 
                return _isCellSelected;
            }
            set
            {
                _isCellSelected = value;
                SetHighlight();
            }
        }

        private bool _isCellSelectable;
        public bool IsCellSelectable
        {
            get
            { 
                return _isCellSelectable;
            }
            set
            {
                _isCellSelectable = value;
                SetHighlight ();
            }
        }

        public ClaimHistoryCountTableViewCell( )
        {   
            CreateLayout();
            InitializeBindings(); 
            SetConstraints();
            SetSelectedBackground(); 
        }
           
        public ClaimHistoryCountTableViewCell(IntPtr handle) : base(handle)
		{   
			CreateLayout();
			InitializeBindings();
            SetConstraints();
            SetSelectedBackground();
              
		}

        public override void SetSelected (bool selected, bool animated)
        {
            SetHighlight (); 
        }

        private bool _userInteractionEnabled;
        public override bool UserInteractionEnabled
        {
            get
            {
                return _userInteractionEnabled;
            }
            set
            {
                _userInteractionEnabled= value; 
            }
        }

       
		private void InitializeBindings()
        {
           
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<ClaimHistoryCountTableViewCell, ClaimHistorySearchResultSummary>();
                set.Bind(_itemlabel).To(vm => vm.BenefitName);
                //   set.Bind(_itemlabel).For(l=>l.TextColor).To(vm => vm.SearchResultCount).WithConversion("ClaimsHistoryCountToTextColor");
                set.Bind(_itemButton).For(b => b.Text).To(vm => vm.SearchResultCountLabel);
                set.Bind(_itemButton).For(l => l.IsButtSelectable).To(vm => vm.SearchResultCount).WithConversion("ClaimsHistoryCountToBool");
                set.Bind(this).For(c => c.UserInteractionEnabled).To(vm => vm.SearchResultCount).WithConversion("ClaimsHistoryCountToBool");
                //  set.Bind (ContentView ).For (v=>v. BackgroundColor ).To (vm => vm.SearchResultCount).WithConversion("ClaimsHistoryCountToColor");
                set.Bind(this).For(c => c.IsCellSelectable).To(vm => vm.SearchResultCount).WithConversion("ClaimsHistoryCountToBool");
                set.Apply();
            });
        }
		private void CreateLayout()
        {
            ContentView.Layer.BorderColor = Colors.BACKGROUND_COLOR.CGColor; 
            ContentView.Layer.BorderWidth = cellBorderWith; 

            _itemlabel = new CountTableViewCellLabel(); 
            ContentView.AddSubview(_itemlabel);  

			countSpacing = new UIView ();
          //  countSpacing.Layer.CornerRadius =cornerRadiusValue;
			ContentView.AddSubview (countSpacing);

            _itemButton = new CountTableViewCellGreenLabel();
            // fixing the corner radius same for both english and french
            _itemButton.Layer.CornerRadius = 9f;
			_itemButton.setToParent = true;
			countSpacing.AddSubview(_itemButton); 
			countSpacing.BackgroundColor = _itemButton.BackgroundColor; 
        } 

        private void SetConstraints()
        { 
            SetitemButtWidthPercentageByLang();
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			countSpacing.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();
            ContentView.AddConstraints(
                ContentView.Height().EqualTo(cellHeight),

                _itemlabel.AtLeftOf(ContentView, itemsLeftMargin),
                _itemlabel.WithRelativeWidth(ContentView, (nfloat)(1 - rightSidePercentage)).Minus(itemsLeftMargin * 2 + spaceBetweenLabels),
                _itemlabel.WithSameCenterY(ContentView),

                countSpacing.Right().EqualTo().RightOf(ContentView).Minus(itemsLeftMargin),
                countSpacing.WithRelativeWidth(ContentView, rightSidePercentage),
                _itemButton.Height().EqualTo(claimCountlabelHeight),
				countSpacing.WithSameCenterY(ContentView),

				_itemButton.AtLeftOf(countSpacing),
				_itemButton.AtRightOf(countSpacing),
				_itemButton.AtTopOf(countSpacing,5),
				_itemButton.AtBottomOf(countSpacing,5)
            );
        }

        public void SetHighlight()
        {
            if (IsCellSelected) {
                UIView newBackGround = this.BackgroundView = new UIView();
                newBackGround.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                this.BackgroundView = newBackGround;
                _itemlabel.TextColor = Colors.BACKGROUND_COLOR;
            } 
            else
            {
                if (IsCellSelectable)
                {
                    UIView newBackGround = this.BackgroundView = new UIView();
                    newBackGround.BackgroundColor = Colors.LightGrayColor;
                    this.BackgroundView = newBackGround;
                        _itemlabel.TextColor = Colors.DarkGrayColor;
                }
                else
                {
                    UIView newBackGround = this.BackgroundView = new UIView();
                    newBackGround.BackgroundColor = Colors.VeryLightGrayColor;
                    this.BackgroundView = newBackGround;
                    _itemlabel.TextColor = Colors.MED_GREY_COLOR;
                }
            }
        }


        private void SetSelectedBackground()
        {
            if (SelectedBackgroundView != null)
            {
                this.SelectedBackgroundView.Layer.BorderColor = Colors.BACKGROUND_COLOR.CGColor;
                this.SelectedBackgroundView.Layer.BorderWidth = cellBorderWith;
            }
        }

        private void SetitemButtWidthPercentageByLang()
        {
            string currLang = System.Globalization.CultureInfo.CurrentUICulture.Name.ToString(); 
            rightSidePercentage = (currLang.Contains("fr") || currLang.Contains("Fr")) ? 0.35f : 0.25f; 
        }
       
    }
}

