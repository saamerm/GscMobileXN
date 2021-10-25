using Newtonsoft.Json;

namespace MobileClaims.Core.Entities.ClaimsHistory
{
    public class ClaimHistoryPayee
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        private bool _isSelected = true;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                }
            }
        }
    }
}
