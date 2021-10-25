using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace MobileClaims.Core.Entities
{
    [JsonObject(Description="")]
    public class PlanMember : Participant, INotifyPropertyChanged
    {
        private List<Participant>_dependents = new List<Participant>();
        public List<Participant> Dependents
        {
            get => _dependents;
            set
            {
                _dependents = value;
                base.RaisePropertyChanged(new PropertyChangedEventArgs("Dependents"));
            }
        }
        private string _employername = "";
        public string EmployerName
        {
            get => _employername;
            set
            {
                _employername = value;
                base.RaisePropertyChanged(new PropertyChangedEventArgs("EmployerName"));
            }
        }

        private PlanConditions _planConditions;
        public PlanConditions PlanConditions
        {
            get => _planConditions;
            set
            {
                _planConditions = value;
                base.RaisePropertyChanged(new PropertyChangedEventArgs("PlanConditions"));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                base.RaisePropertyChanged(new PropertyChangedEventArgs("Email"));
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }
    }
}
