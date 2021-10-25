namespace MobileClaims.Core.ViewModels
{
    public class DrugLookupSplitViewModel : ViewModelBase
    {
        private DrugLookupModelSelectionViewModel _leftpane;

        public DrugLookupModelSelectionViewModel LeftPane
        {
            get { return _leftpane; }
            set
            {
                if (_leftpane != value)
                {
                    _leftpane = value;
                    RaisePropertyChanged(() => LeftPane);
                };
            }
        }

        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set
            {
                if (myVar != value)
                {
                    myVar = value;
                    RaisePropertyChanged(() => MyProperty);
                };
            }
        }
        
    }
}
