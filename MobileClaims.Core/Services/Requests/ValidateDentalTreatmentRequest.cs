using System;
using Newtonsoft.Json;

namespace MobileClaims.Core.Services.Requests
{
    public class ValidateDentalTreatmentRequest
    {
        [JsonProperty("planMemberId")]
        public string PlanMemberId { get; set; }

        [JsonProperty("dentalProcedureCode")]
        public string DentalProcedureCode { get; set; }

        [JsonProperty("providerProvinceCode")]
        public string ProviderProvinceCode { get; set; }

        [JsonProperty("serviceDate")]
        public string ServiceDate { get; set; }

        [JsonProperty("toothCode")]
        public string ToothCode { get; set; }

        [JsonProperty("toothSurface")]
        public string ToothSurface { get; set; }
    }
}