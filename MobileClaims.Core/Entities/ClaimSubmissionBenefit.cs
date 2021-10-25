using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ClaimSubmissionBenefit
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("procedureCode")]
        public string ProcedureCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
