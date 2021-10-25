using Newtonsoft.Json;
using System;

namespace MobileClaims.Core.Entities
{
    public class ParticipantEligibilityResult
    {
        private const string ELIGIBILITY_RESULT_DATE_FORMAT = "MMM dd, yyyy";

        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("participantFullName")]
        public string ParticipantFullName { get; set; }

        [JsonProperty("recallExamEligibilityDate")]
        public string RawRecallExamEligibilityDate { get; set; }

        [JsonIgnore]
        public DateTime RecallExamEligibilityDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(RawRecallExamEligibilityDate, out dt);
                return dt;
            }
        }

        [JsonIgnore]
        public string RecallExamEligibilityDateFormatted
        {
            get
            {
                return RecallExamEligibilityDate.ToString(ELIGIBILITY_RESULT_DATE_FORMAT);
            }
        }

        [JsonProperty("recallExamEligibilityStatus")]
        public string RecallExamEligibilityStatus { get; set; }

        [JsonProperty("scalingEligibilityDate")]
        public string RawScalingEligibilityDate { get; set; }

        [JsonIgnore]
        public DateTime ScalingEligibilityDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(RawScalingEligibilityDate, out dt);
                return dt;
            }
        }

        [JsonIgnore]
        public string ScalingEligibilityDateFormatted
        {
            get
            {
                return ScalingEligibilityDate.ToString(ELIGIBILITY_RESULT_DATE_FORMAT);
            }
        }

        [JsonProperty("scalingEligibilityStatus")]
        public string ScalingEligibilityStatus { get; set; }

        [JsonProperty("polishingEligibilityDate")]
        public string RawPolishingEligibilityDate { get; set; }

        [JsonIgnore]
        public DateTime PolishingEligibilityDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(RawPolishingEligibilityDate, out dt);
                return dt;
            }
        }

        [JsonIgnore]
        public string PolishingEligibilityDateFormatted
        {
            get
            {
                return PolishingEligibilityDate.ToString(ELIGIBILITY_RESULT_DATE_FORMAT);
            }
        }

        [JsonProperty("polishingEligibilityStatus")]
        public string PolishingEligibilityStatus { get; set; }

        [JsonProperty("orthoticEligibilityDate")]
        public string RawOrthoticEligibilityDate { get; set; }

        [JsonIgnore]
        public DateTime OrthoticEligibilityDate
        {
            get
            {
                DateTime dt;
                DateTime.TryParse(RawOrthoticEligibilityDate, out dt);
                return dt;
            }
        }

        [JsonIgnore]
        public string OrthoticEligibilityDateFormatted
        {
            get
            {
                return OrthoticEligibilityDate.ToString(ELIGIBILITY_RESULT_DATE_FORMAT);
            }
        }

        [JsonProperty("orthoticEligibilityStatus")]
        public string OrthoticEligibilityStatus { get; set; }
    }
}
