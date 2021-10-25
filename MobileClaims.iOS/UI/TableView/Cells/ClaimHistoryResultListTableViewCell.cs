using System;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using MobileClaims.Core.Entities.ClaimsHistory;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace MobileClaims.iOS
{
    [Foundation .Register ("ClaimHistoryResultListTableViewCell")]
    public class ClaimHistoryResultListTableViewCell : MvxTableViewCell
    {
        
        private ClaimForLabel dateLabel; 
        private ClaimForLabel numberLabel; 
        private UILabel typeLabel;
        private ClaimForLabel amountLabel;
        private UILabel amountTxtLabel; 
     
        private float itemsLeftMargin=10f; 
        private float itemsBelowMargin=5f;
        private float rightSidePercentage=0.25f;
        private float topMargin=10f;
        private float cellHeight=90f;
        private float claimCountlabelHeight=25f;
        private float spaceBetweenLabels=2f;
        private float cellBorderWith=5f;
        private UIStringAttributes AttrStrikeThrough; 

        public ClaimHistoryResultListTableViewCell( )
        {  
            CreateLayout();
            InitializeBindings(); 
            SetConstraints();
            SetSelectedBackground(); 

        }

        public ClaimHistoryResultListTableViewCell(IntPtr handle) : base(handle)
        {  
            CreateLayout();
            InitializeBindings();
            SetConstraints();
            SetSelectedBackground();

        }

        public override void SetSelected(bool selected, bool animated)
        { 
            SetHighlighted(selected); 
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
                var set = this.CreateBindingSet<ClaimHistoryResultListTableViewCell, ClaimState>();
			   set.Bind(dateLabel).To(e => e.ServiceDate).WithConversion("StringFromDate"); 
                set.Bind(numberLabel).To(e=>e.ClaimFormID);
                set.Bind(typeLabel).To(e => e.ServiceDescription).WithConversion ("TextTrim",25); 
                set.Bind (amountLabel ). To (e => e.ClaimedAmountLabel); 
                set.Bind(amountTxtLabel).To(e=>e.ClaimedAmount).WithConversion("DollarSignDoublePrefix");
                //set.Bind (ContentView ).For (v=>v. BackgroundColor ).To (vm => vm.SearchResultCount).WithConversion("ClaimsHistoryCountToColor");
                set.Bind(this).For(c=>c.Item).To(e=>e);
                set.Apply();
            });
        }
        private void CreateLayout()
        {
            
            dateLabel = new ClaimForLabel();
            if (Item != null)
            {
                if (Item.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Item.ServiceDate.ToString ("yyyy-MM-dd"), AttrStrikeThrough);  
                    dateLabel.AttributedText = textAttributed; 
                }
                else
                { 
                    dateLabel.Text = Item.ServiceDate.ToString ("yyyy-MM-dd");
                }
            }
            ContentView.AddSubview(dateLabel); 

            numberLabel = new ClaimForLabel();
            if (Item != null)
            {
                if (Item.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(Item.ClaimFormID.ToString(), AttrStrikeThrough);  
                    numberLabel.AttributedText = textAttributed; 
                }
                else
                { 
                    numberLabel.Text = Item.ClaimFormID.ToString();
                }
            }
            ContentView.AddSubview(numberLabel); 
          
            typeLabel = new UILabel();
            typeLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.TEXT_FIELD_SUB_HEADING_SIZE);
            typeLabel.TextAlignment = UITextAlignment.Left; 
            typeLabel.LineBreakMode = UILineBreakMode.WordWrap;
            typeLabel.Lines = 0; 
            if (Item != null)
            {
                string textTrimed= TrimText(Item.ServiceDescription, 25);
                if (Item.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString(textTrimed, AttrStrikeThrough);  
                    typeLabel.AttributedText = textAttributed; 
                }
                else
                { 
                    typeLabel.Text = textTrimed;
                }
            } 
            ContentView.AddSubview(typeLabel);

            amountLabel = new ClaimForLabel(); 
            amountLabel.TextAlignment = UITextAlignment.Right;
            ContentView.AddSubview(amountLabel); 

            amountTxtLabel = new UILabel(); 
            amountTxtLabel.TextAlignment = UITextAlignment.Right;
            amountTxtLabel.Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.SMALL_FONT_SIZE); 
            if (Item != null)
            {
                if (Item.IsStricken)
                {
                    var textAttributed = new NSMutableAttributedString( "$" + Item.ClaimedAmount.ToString(), AttrStrikeThrough);  
                    amountTxtLabel.AttributedText = textAttributed; 
                }
                else
                { 
                    amountTxtLabel.Text = "$" + Item.ClaimedAmount.ToString();
                }
            }
            ContentView.AddSubview(amountTxtLabel); 
        } 

        private void SetConstraints()
        { 
            ContentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints(); 
            ContentView.AddConstraints(
                ContentView.Height().EqualTo(cellHeight),  

                dateLabel.AtTopOf(ContentView, cellBorderWith + itemsLeftMargin),
                dateLabel.AtLeftOf(ContentView, itemsLeftMargin+cellBorderWith), 
                dateLabel.WithRelativeWidth(ContentView, (nfloat)(1 - rightSidePercentage)).Minus(spaceBetweenLabels / 2 + cellBorderWith+itemsLeftMargin),
                  
                numberLabel.WithSameCenterY(dateLabel ),
                numberLabel.WithSameRight(ContentView).Minus (itemsLeftMargin+cellBorderWith),

                typeLabel.WithSameLeft(dateLabel), 
                typeLabel.WithSameWidth(ContentView).Minus((nfloat)(itemsLeftMargin+cellBorderWith) * 2), 
                typeLabel.Below(dateLabel, itemsBelowMargin), 

                amountLabel.WithSameLeft(typeLabel),
                amountLabel.Below(typeLabel,itemsBelowMargin), 

                amountTxtLabel.Right().EqualTo().RightOf(ContentView).Minus(itemsLeftMargin + cellBorderWith),
                //amountTxtLabel.WithRelativeWidth(ContentView, rightSidePercentage).Minus(cellBorderWith+itemsLeftMargin), 
                amountTxtLabel.WithSameCenterY(amountLabel) 
            );
        }

        private void SetHighlighted(bool selected)
        { 
            if (selected) {
//                UIView newBackGround = this.BackgroundView = new UIView();
//                newBackGround.BackgroundColor = Colors.HIGHLIGHT_COLOR;
//                this.BackgroundView = newBackGround; 
                ContentView.BackgroundColor = Colors.HIGHLIGHT_COLOR;
                dateLabel.TextColor=Colors.BACKGROUND_COLOR; 
                numberLabel.TextColor=Colors.BACKGROUND_COLOR; 
                typeLabel.TextColor=Colors.BACKGROUND_COLOR;
                amountLabel.TextColor=Colors.BACKGROUND_COLOR;
                amountTxtLabel.TextColor=Colors.BACKGROUND_COLOR; 
            } 
            else
            {
                ContentView.BackgroundColor = Colors.LightGrayColor;
                dateLabel.TextColor=Colors.DARK_GREY_COLOR; 
                numberLabel.TextColor=Colors.DARK_GREY_COLOR;  
                typeLabel.TextColor=Colors.DARK_GREY_COLOR; 
                amountLabel.TextColor=Colors.DARK_GREY_COLOR; 
                amountTxtLabel.TextColor=Colors.DARK_GREY_COLOR;  
            } 
        } 
         

        private void SetSelectedBackground()
        {
            ContentView.BackgroundColor = Colors.LightGrayColor; 
            ContentView.Layer.BorderColor = Colors.BACKGROUND_COLOR.CGColor; 
            ContentView.Layer.BorderWidth = cellBorderWith;
            this.UserInteractionEnabled = true;
            if (SelectedBackgroundView != null)
            {
                this.SelectedBackgroundView.Layer.BorderColor = Colors.BACKGROUND_COLOR.CGColor;
                this.SelectedBackgroundView.Layer.BorderWidth = cellBorderWith;
            }
        }

        private float _cellHeight=70f;
        public float CellHeight
        {
            get
            { 
                return _cellHeight;
            }
            set
            { 
                _cellHeight = value;
            }
        } 

        private ClaimState _item;
        public ClaimState Item
        {
            get
            { 
                return _item;
            }
            set
            { 
                _item = value;
                #region set strikeThrough
                if (_item != null)
                {
                    if (_item.IsStricken)
                    {
                        AttrStrikeThrough = new UIStringAttributes()
                        { 
                            Font = UIFont.FromName(Constants.NUNITO_BOLD, (nfloat)Constants.GS_SELECTION_BUTTON),
                            StrokeWidth = 0, 
                            StrikethroughStyle = NSUnderlineStyle.Single,
                            StrikethroughColor = Colors.Black
                        }; 
                        dateLabel.RemoveFromSuperview();
                        numberLabel.RemoveFromSuperview();
                        typeLabel.RemoveFromSuperview();
                        amountLabel.RemoveFromSuperview();
                        amountTxtLabel.RemoveFromSuperview();
                        CreateLayout();
                        var set = this.CreateBindingSet<ClaimHistoryResultListTableViewCell, ClaimState>(); 
                        set.Bind(amountLabel).To(e => e.ClaimedAmountLabel);  
                        set.Apply();
                        SetConstraints();
                    } 
                }
                #endregion
            }
        }

        private string TrimText(string value, int parameter)
        {
            string result = string.Empty; 
            if (!string.IsNullOrEmpty(value.ToString()))
            { 
                int count = value.ToString().Length;
                if (count > 0)
                {
                    result = value.ToString();
                    if (parameter != null)
                    {
                        int longShouldBe = int.Parse(parameter.ToString());
                        if (longShouldBe < count)
                        {
                            result = result.Substring(0, longShouldBe); 
                            result += "...";
                        }
                    }
                } 
            }
            return result;
        }


    }
}

