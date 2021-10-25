using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    [JsonObject(Description = "Holds all data persisted to the device")]
    public class PersistedData
    {
        [JsonProperty("lastloginUTC")]
        public string lastloginUTC { get; set; }
        [JsonProperty("cardplanmember")]
        public PlanMember cardplanmember { get; set; }
        [JsonProperty("username")]
        public string username { get; set; }
        [JsonProperty("claim")]
        public Claim claim { get; set; }
        [JsonProperty("acceptedTC")]
        public bool acceptedTC { get; set; }
        [JsonProperty("idcard")]
        public IDCard idcard
        {
            get;
            set;
        }
        [JsonProperty("isloggedin")]
        public bool isloggedin { get; set; }
        [JsonIgnore]
        public AuthInfo authinfo { get; set; }
        [JsonProperty("planmember")]
        public PlanMember planmember { get; set; }
        [JsonProperty("lastusertologin")]
        public string lastusertologin { get; set; }
        [JsonProperty("hcsaclaim")]
        public Entities.HCSA.Claim hcsaclaim { get; set; }
        [JsonProperty("hcsaclaimtype")]
        public Entities.HCSA.ClaimType HCSAClaimType {get;set;}
        [JsonProperty("hcsaexpensetype")]
        public Entities.HCSA.ExpenseType HCSAExpenseType { get; set; }
        [JsonProperty("usebiometrics")]
        public bool? usebiometrics { get; set; }
        public PersistedData()
        {
            this.lastloginUTC = string.Empty; // DateTime.Now.AddYears(-5).ToUniversalTime();
            this.cardplanmember = new PlanMember();
            this.username = string.Empty;
            this.claim = new Claim();
            this.acceptedTC = false;
            this.idcard = new IDCard();
            this.lastusertologin = string.Empty;
            this.usebiometrics = null;
        }

    }
}
