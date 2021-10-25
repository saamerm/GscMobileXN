using System;
using UIKit;
using System.Collections.ObjectModel;
using Foundation;
using MobileClaims.Core.Entities.ClaimsHistory;
using MvvmCross.Platforms.Ios.Binding.Views;


namespace MobileClaims.iOS  
{
    public class YearPickerView :  UIPickerView
    {
        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler HideErrorButtonEvent;
       
        public YearPickerView() 
        {
            this.UserInteractionEnabled = true;
        } 
       
        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt); 
        } 

        
        public override void DidChange(NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey)
        {
            base.DidChange(changeKind, indexes, forKey);

        }
        public override nint SelectedRowInComponent(nint component)
        {
           
            if (this.HideErrorButtonEvent != null)
            {
                this.HideErrorButtonEvent(this, EventArgs.Empty);
            }

            return base.SelectedRowInComponent(component);
        }
    }  
    public class YearPickerSource :  MvxPickerViewModel 
    { 
        public YearPickerSource (UIPickerView pickerView)
            :base(pickerView)
        {
            
        } 

        #region implemented abstract members of UIPickerViewDataSource 
        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            nint result = 0; 
            if (this.ItemsSource is ObservableCollection<DisplayByYear>)
            { 
                ObservableCollection<DisplayByYear> rows = this.ItemsSource as ObservableCollection<DisplayByYear>;
                if(rows!=null )
                {
                    result =(nint) rows.Count;
                } 
            } 
            return result; 
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {  
            string result =string.Empty; 
            if (this.ItemsSource is ObservableCollection<DisplayByYear>)
            { 
                ObservableCollection<DisplayByYear> yearColl = this.ItemsSource as ObservableCollection<DisplayByYear>;
                if(yearColl!=null )
                {
                    DisplayByYear dby = yearColl[(int)row] as DisplayByYear;
                    result = dby.DisplayString;
                } 
            } 
            return result; 
        }

        protected override string RowTitle(nint row, object item)
        { 
            string result =string.Empty;  
             ObservableCollection<DisplayByYear> yearColl = this.ItemsSource as ObservableCollection<DisplayByYear>;
            if(yearColl!=null )
            {
                DisplayByYear dby = yearColl[(int)row] as DisplayByYear;
                result = dby.DisplayString;
            } 
            return result; 
        }   
        #endregion
    } 
}
 
