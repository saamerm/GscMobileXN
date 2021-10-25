using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MobileClaims.Core.Services.Responses
{
    public class ValidateDentalTreatmentResponse
    {
        [JsonProperty("dentalProcedureCode")]
        public string DentalClaimProcedureCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("labFeeInd")]
        public bool IsLaboratoryChargeRequired { get; set; }

        [JsonProperty("isToothCodeRequired")]
        public bool IsToothCodeRequired { get; set; }

        [JsonProperty("isToothSurfaceRequired")]
        public bool IsToothSurfaceRequired { get; set; }

        [JsonProperty("requiredToothSurfaceCount")]
        public int RequiredToothSurfaceCount { get; set; }

        [JsonProperty("isToothCodeValid")]
        public bool IsToothCodeValid { get; set; }

        [JsonProperty("isToothSurfaceValid")]
        public bool IsToothSurfaceValid { get; set; }

        [JsonProperty("istoothSurfaceCountMatched")]
        public bool IstoothSurfaceCountMatched { get; set; }

        [JsonProperty("links")]
        public List<object> Links { get; set; }
    }
}