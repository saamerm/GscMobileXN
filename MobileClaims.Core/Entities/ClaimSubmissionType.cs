using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimSubmissionType
    {
        private string _id;
        [JsonProperty("id")]
        public string ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set
            {
                if(_name != value)
                {
                    _name = value;
                }
            }
        }

        private bool _isAutoCoordinationOn;
        [JsonProperty("isAutoCoordinationOn")]
        public bool IsAutoCoordinationOn
        {
            get => _isAutoCoordinationOn;
            set
            {
                if (_isAutoCoordinationOn != value)
                {
                    _isAutoCoordinationOn = value;
                }
            }
        }
    }
}
