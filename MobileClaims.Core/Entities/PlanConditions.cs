using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class PlanConditions
    {
        [JsonProperty("visionPlanType")]
        public int VisionPlanType { get; set; }

        [JsonProperty("visionRequiresPrescription")]
        public bool VisionRequiresPrescription { get; set; }

        [JsonProperty("spendingAccountExists")]
        public bool SpendingAccountExists { get; set; }

        [JsonProperty("canAutoCoordinate")]
        public bool CanAutoCoordinate { get; set; }

        [JsonProperty("isAutoCoordinationOn")]
        public bool IsAutoCoordinationOn { get; set; }

    }
}
