using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MobileClaims.Core.Entities.HCSA
{
    public class Claim : NotifyingBase, IDisposable
    {
        public Claim()
        {
            Details = new ObservableCollection<ClaimDetail>();
            Details.CollectionChanged += Details_CollectionChanged;
        }

        private void Details_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach(ClaimDetail cd in Details.Where(detail => detail.ParentClaim ==null))
            {
                cd.ParentClaim = this;
            }
            RaisePropertyChanged(new PropertyChangedEventArgs("Details"));
        }

        public void Dispose()
        {
            Details.CollectionChanged -= Details_CollectionChanged;
        }

        private long _planmemberid;
        [JsonProperty(PropertyName ="planMemberId")]
        public long PlanMemberID
        {
            get => _planmemberid;
            set
            {
                _planmemberid = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("PlanMemberID"));
            }
        }

        private string _participantnumber;
        [JsonProperty(PropertyName ="participantNumber")]
        public string ParticipantNumber
        {
            get => _participantnumber;
            set
            {
                _participantnumber = value; 
                RaisePropertyChanged(new PropertyChangedEventArgs(ParticipantNumber));
            }
        }

        private long _claimtypeid;
        [JsonProperty(PropertyName = "claimTypeId")]
        public long ClaimTypeID
        {
            get => _claimtypeid;
            set
            {
                _claimtypeid = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ClaimTypeID"));
            }
        }

        private ExpenseType _expensetype;
        [JsonIgnore]
        public ExpenseType ExpenseType
        {
            get => _expensetype;
            set
            {
                _expensetype = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ExpenseType"));
            }
        }

        private ClaimType _claimType;
        //[JsonIgnore]
        public ClaimType ClaimType
        {
            get => _claimType;
            set
            {
                _claimType = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ClaimType"));
            }
        }


        private long _expensetypeid;
        [JsonProperty(PropertyName = "expenseTypeId")]
        public long ExpenseTypeID
        {
            get => _expensetypeid;
            set
            {
                _expensetypeid = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ExpenseTypeID"));
            }
        }

        private string _medicalprofessionalid;
        [JsonProperty(PropertyName = "medicalProfessionalId")]
        public string MedicalProfessionalID
        {
            get => _medicalprofessionalid;
            set
            {
                _medicalprofessionalid = value;
                RaisePropertyChanged(new PropertyChangedEventArgs(";MedicalProfessionalID"));
            }
        }

        private bool _referralrequired;
        [JsonProperty(PropertyName ="referralRequired")]
        public bool ReferralRequired
        {
            get => _referralrequired;
            set
            {
                _referralrequired = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ReferralRequired"));
            }
        }


        private ObservableCollection<ClaimDetail> _details;
        [JsonProperty(PropertyName ="details")]
        public ObservableCollection<ClaimDetail> Details
        {
            get => _details;
            set
            {
                _details = value;
                foreach(ClaimDetail cd in _details)
                {
                    cd.ParentClaim = this;
                }
                RaisePropertyChanged(new PropertyChangedEventArgs("Details"));
            }
        }

        private ObservableCollection<ClaimResultGSC> _results;
        [JsonProperty(PropertyName ="results")]
        public ObservableCollection<ClaimResultGSC> Results
        {
            get => _results;
            set
            {
                _results = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("Results"));
            }
        }

        private int _validationstatuscode;
        public int ValidationStatusCode
        {
            get => _validationstatuscode;
            set
            {
                _validationstatuscode = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ValidationStatusCode"));
            }
        }

        private bool _termsandconditionsaccepted;
        [JsonIgnore]
        public bool TermsAndConditionsAccepted
        {
            get => _termsandconditionsaccepted;
            set
            {
                _termsandconditionsaccepted = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TermsAndConditionsAccepted"));
            }
        }
    }
}
