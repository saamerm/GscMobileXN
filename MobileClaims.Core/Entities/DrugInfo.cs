using Newtonsoft.Json;
namespace MobileClaims.Core.Entities
{

    public class DrugInfo
    {
        [JsonProperty("id")]
        public int ID
        {
            get;
            set;
        }

        [JsonProperty("din")]
        public int DIN
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }
        public string Gender
        {
            get;
            set;
        }
        public bool SpecialAuthRequired
        {
            get;
            set;
        }
        public byte[] SpecialAuthForm
        {
            get;
            set;
        }
        public string SpecialAuthFormName
        {
            get;
            set;
        }
        public string AuthorizationMessage
        {
            get;
            set;
        }
        [JsonIgnore]
        public Participant PlanParticipant
        {
            get;
            set;
        }
        [JsonIgnore]
        public bool Covered
        {
            get;
            set;
        }
        [JsonIgnore]
        public string CoveredMessage
        {
            get;set;
        }
        [JsonIgnore]
        public string Reimbursement
        {
            get;
            set;
        }

        public string Notes { get; set; }

        [JsonIgnore]
        public bool LowCostReplacementOccurred
        {
            get;
            set;
        }
    }
}
