using System.ComponentModel;

namespace MobileClaims.Core.Entities.HCSA
{
    public class ExpenseType : SelectableItem
    {
        public ExpenseType() : base() { }
        private long _id;
        public long ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ID"));
                }
            }
        }

        private long _claimtypeid;
        public long ClaimTypeID
        {
            get => _claimtypeid;
            set
            {
                if (_claimtypeid != value)
                {
                    _claimtypeid = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("ClaimTypeID"));
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                if (_description!=value)
                {
                    _description = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }
    }
}
