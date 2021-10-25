using MobileClaims.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MobileClaims.Core.Entities
{
    [JsonObject(Description = "")]
    public class Participant : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private bool _lastthenfirst = false;
        public bool LastThenFirst
        {
            get
            {
                return _lastthenfirst;
            }
            set
            {
                _lastthenfirst = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LastThenFirst"));
                RaisePropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }
        private string _planmemberid;
        public string PlanMemberID
        {
            get
            {
                return _planmemberid;
            }
            set
            {
                _planmemberid = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("PlanMemberID"));
            }
        }
        private string _firstname;
        public string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("FirstName"));
                RaisePropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }
        public string _lastname;
        public string LastName
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("LastName"));
                RaisePropertyChanged(new PropertyChangedEventArgs("FullName"));
            }
        }
        public string FullName
        {
            get
            {
                if (_lastthenfirst)
                {
                    return string.Format("{0}, {1}", _lastname, _firstname);
                }
                else
                {
                    return string.Format("{0} {1}", _firstname, _lastname);
                }
            }
        }
        private bool _travelcovered = false;
        public bool TravelCovered
        {
            get
            {
                return _travelcovered;
            }
            set
            {
                _travelcovered = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("TravelCovered"));
            }
        }
        private byte[] _cardimagerybytes;
        public byte[] CardImageryBytes
        {
            get
            {
                return _cardimagerybytes;
            }
            set
            {
                _cardimagerybytes = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("CardImageryBytes"));
            }
        }
        private string _cardimagepath;
        public string CardImagePath
        {
            get
            {
                return _cardimagepath;
            }
            set
            {
                _cardimagepath = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("CardImagePath"));
            }
        }
        private string _cardlogofilename = "";
        public string CardLogoFileName
        {
            get
            {
                return _cardlogofilename;
            }
            set
            {
                _cardlogofilename = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("CardLogoFileName"));
            }
        }

        private string _status;

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        private string _provinceCode;
        private DateTime _dateOfBirth;

        public string ProvinceCode
        {
            get => _provinceCode;
            set
            {
                _provinceCode = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("ProvinceCode"));
            }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("DateOfBirth"));
            }
        }

        public bool IsResidentOfQuebecProvince()
        {
            return IsResidentOfProvice("QC");
        }

        public bool IsOrUnderAgeOf18()
        {
            return IsOrUnderAgeOf(18);
        }

        private bool IsResidentOfProvice(string targetedProvinceCode)
        {
            return string.Equals(targetedProvinceCode, ProvinceCode, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsOrUnderAgeOf(int targetedAge)
        {
            return DateOfBirth.CalculateAge() <= targetedAge;
        }
    }
}