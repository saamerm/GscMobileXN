using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using CoreText;
using Foundation;
using MobileClaims.iOS.UI.TableView.Cells;
using ObjCRuntime;
using UIKit;
using static MobileClaims.Core.ViewModels.RefineSearchViewModel;

namespace MobileClaims.iOS.UI
{
    [Register("AutoCompleteTextField"), DesignTimeVisible(true)]
    public class AutoCompleteTextField : UITextField
    {
        /// Manages the instance of tableview
        private UITableView autoCompleteTableView;
        /// Holds the collection of attributed strings
        public List<NSAttributedString> attributedAutoCompleteStrings { get; private set; }
        /// Handles user selection action on autocomplete table view
        public Action<string, NSIndexPath> onSelect;
        /// Handles textfield's textchanged
        public Action<string> onTextChange;

        /// Font for the text suggestions
        public UIFont autoCompleteTextFont = UIFont.SystemFontOfSize(12);
        /// Color of the text suggestions
        public UIColor autoCompleteTextColor = Colors.Black;
        /// The maximum visible suggestion
        public int maximumAutoCompleteCount = 3;
        /// Used to set your own preferred separator inset
        public UIEdgeInsets autoCompleteSeparatorInset = UIEdgeInsets.Zero;
        /// Shows autocomplete text with formatting
        public bool enableAttributedText = false;
        /// User Defined Attributes
        public Dictionary<string, NSObject> autoCompleteAttributes;
        /// Hides autocomplete tableview after selecting a suggestion
        public bool hidesWhenSelected = true;
        /// Hides autocomplete tableview when the textfield is empty
        private bool? _hidesWhenEmpty;
        public bool? HidesWhenEmpty
        {
            get => _hidesWhenEmpty;
            set
            {
                _hidesWhenEmpty = value;
                if(autoCompleteTableView != null)
                    autoCompleteTableView.Hidden = value ?? false;
            }
        }
        /// The table view height

        private float _autoCompleteTableHeight = float.MinValue;
        public float autoCompleteTableHeight
        {
            get => _autoCompleteTableHeight;
            set
            {
                _autoCompleteTableHeight = value;
                redrawTable();
            }
        }
        /// The strings to be shown on as suggestions, setting the value of this automatically reload the tableview
        private ObservableCollection<ItemList> _autoCompleteStrings;
        public ObservableCollection<ItemList> autoCompleteStrings
        {
            get => _autoCompleteStrings;
            set{ _autoCompleteStrings = value; reload(); }
        }


        public AutoCompleteTextField(IntPtr p) : base(p)
        {
            commonInit();
        }

        public AutoCompleteTextField(CGRect frame) : base(frame)
        {
            commonInit();
        }

        public override void AwakeFromNib()
        {
            commonInit();
        }
        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            commonInit();
            setupAutocompleteTable(newsuper);
        }

        private void commonInit()
        {
            HidesWhenEmpty = true;
            autoCompleteAttributes = new Dictionary<string, NSObject>() { { UIStringAttributeKey.ForegroundColor, Colors.Black } };
            autoCompleteAttributes[UIStringAttributeKey.Font] = UIFont.BoldSystemFontOfSize(12);
            this.ClearButtonMode = UITextFieldViewMode.Always;
            this.AddTarget(textFieldDidChange, UIControlEvent.EditingChanged);
            this.AddTarget(textFieldDidEndEditing, UIControlEvent.EditingDidEnd);
        }

        private void setupAutocompleteTable(UIView view)
        {
            var screenSize = UIScreen.MainScreen.Bounds.Size;
            var tableView = new UITableView(new CGRect(Frame.Location.X, Frame.Location.Y + Frame.Height, screenSize.Width - (Frame.Location.X * 2), 30.0));
            tableView.RegisterNibForCellReuse(LocationAutocompleteTableViewCell.Nib, "LocationAutocompleteTableViewCell");
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            var source = new AutoCompleteTextFieldDataSource(this);

            tableView.Source = source;
            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.Hidden = HidesWhenEmpty ?? true;
            view.AddSubview(tableView);
            autoCompleteTableView = tableView;

            autoCompleteTableHeight = 75;
            view.BringSubviewToFront(autoCompleteTableView);
        }

        private void redrawTable()
        {
            if (autoCompleteTableView == null || autoCompleteTableHeight < 0)
                return;
            var newFrame = new CGRect(autoCompleteTableView.Frame.Location, new CGSize(autoCompleteTableView.Frame.Width, autoCompleteTableHeight));
            autoCompleteTableView.Frame = newFrame;
        }

        //MARK: - Private Methods
        private void reload()
        {
            if (enableAttributedText)
            {
                var attrs = NSDictionary.FromObjectsAndKeys(new string[] { UIStringAttributeKey.ForegroundColor, UIStringAttributeKey.Font }, new NSObject[] { autoCompleteTextColor, autoCompleteTextFont });
                if (attributedAutoCompleteStrings.Count > 0) 
                {
                    attributedAutoCompleteStrings.Clear();
                }

                if (autoCompleteStrings == null || autoCompleteAttributes == null)
                    return;
                for (int i = 0; i < autoCompleteStrings.Count; i++)
                {
                    var str = new NSString(autoCompleteStrings.ToArray()[i].Title);

                    //var range = str.rangeOfString(text!, options: .CaseInsensitiveSearch)
                    var range = CalcRangeFor(str, Text);
                    var attString = new NSMutableAttributedString(autoCompleteStrings[i].Title, attrs);
                    attString.AddAttributes(new CTStringAttributes(NSDictionary.FromObjectsAndKeys(autoCompleteAttributes.Values.ToArray(), autoCompleteAttributes.Keys.ToArray())), range);
                    attributedAutoCompleteStrings.Append(attString);
                }
            }
            autoCompleteTableView.ReloadData();
        }

        private NSRange CalcRangeFor(string source, string substring)
        {
            var range = new NSRange
            {
                Location = source.IndexOf(substring),
                Length = substring.Length
            };

            return range;
        }

        void textFieldDidChange(object sender, EventArgs e)
        {
            if (Text == null)
                return;
            onTextChange?.Invoke(Text);

            if (string.IsNullOrEmpty(Text))
                autoCompleteStrings = null;
            InvokeOnMainThread(() => autoCompleteTableView.Hidden = HidesWhenEmpty.Value ? string.IsNullOrEmpty(Text) : false);
        }

        void textFieldDidEndEditing(object sender, EventArgs e)
        {
            autoCompleteTableView.Hidden = true;
        }
    }

    //MARK: - UITableViewDataSource - UITableViewDelegate
    public class AutoCompleteTextFieldDataSource: UITableViewSource
    {
        AutoCompleteTextField autoCompleteTextField;

        public AutoCompleteTextFieldDataSource(AutoCompleteTextField autoCompleteTextField)
        {
            this.autoCompleteTextField = autoCompleteTextField;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cellIdentifier = "LocationAutocompleteTableViewCell";
            var cell = (LocationAutocompleteTableViewCell)tableView.DequeueReusableCell(cellIdentifier);

            if (autoCompleteTextField.enableAttributedText)
            {
                cell.Label.AttributedText = autoCompleteTextField.attributedAutoCompleteStrings.ToArray()[indexPath.Row];
            }
            else
            {
                cell.Label.Font = autoCompleteTextField.autoCompleteTextFont;
                //cell.Label.TextColor = autoCompleteTextField.autoCompleteTextColor;
                cell.Label.Text = autoCompleteTextField.autoCompleteStrings.ToArray()[indexPath.Row].Title;
            }

            cell.ContentView.GestureRecognizers = null;
            return cell;
        }

        public override bool CanFocusRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override bool CanPerformAction(UITableView tableView, Selector action, NSIndexPath indexPath, NSObject sender)
        {
            return true;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return autoCompleteTextField.autoCompleteStrings != null ? (autoCompleteTextField.autoCompleteStrings.Count > autoCompleteTextField.maximumAutoCompleteCount ? autoCompleteTextField.maximumAutoCompleteCount : autoCompleteTextField.autoCompleteStrings.Count) : 0;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (LocationAutocompleteTableViewCell)tableView.CellAt(indexPath);

            var selectedText = cell.Label.Text;
            autoCompleteTextField.Text = selectedText;
            autoCompleteTextField.onSelect?.Invoke(selectedText, indexPath);
            InvokeOnMainThread(() => tableView.Hidden = autoCompleteTextField.hidesWhenSelected);
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            cell.SeparatorInset = autoCompleteTextField.autoCompleteSeparatorInset;
            cell.PreservesSuperviewLayoutMargins = false;
            cell.LayoutMargins = autoCompleteTextField.autoCompleteSeparatorInset;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }
    }
}
