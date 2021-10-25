using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class IDCard
    {
        private string _planMemberID;
        public string PlanMemberID
        {
            get
            {
                if (string.IsNullOrEmpty(_planMemberID))
                    _planMemberID = PlanMemberDisplayID;

                return _planMemberID;
            }
            set
            {
                _planMemberID = value;
            }
        }

        [JsonProperty("planMemberDisplayId")]
        public string PlanMemberDisplayID { get; set; }

        [JsonProperty("policyNo")]
        public string PolicyNo { get; set; }

        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("planMemberFullName")]
        public string PlanMemberFullName { get; set; }

        [JsonProperty("planMemberFirstName")]
        public string PlanMemberFirstName { get; set; }

        [JsonProperty("planMemberLastName")]
        public string PlanMemberLastName { get; set; }

        [JsonProperty("clientBusinessName")]
        public string ClientBusinessName { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("frontImageUri")]
        public string FrontImageUri { get; set; }

        [JsonProperty("backImageUri")]
        public string BackImageUri
        {
            get;
            set;
        }

        [JsonProperty("frontLeftLogoImageUri")]
        public string FrontLeftLogoImageUri { get; set; }

        [JsonProperty("frontRightLogoImageUri")]
        public string FrontRightLogoImageUri { get; set; }

        [JsonProperty("backTopLogoImageUri")]
        public string BackTopLogoImageUri { get; set; }

        [JsonProperty("travelGroupPolicyNumber")]
        public string TravelGroupPolicyNumber { get; set; }

        [JsonProperty("participants")]
        public List<IDCardParticipant> Participants { get; set; }

        public string FrontImageFilePath { get; set; }

        public string BackImageFilePath { get; set; }

        public string FrontLeftLogoFilePath { get; set; }

        public string FrontRightLogoFilePath { get; set; }

        public string BackTopLogoFilePath { get; set; }

        public byte[] FrontImageByteArray { get; set; }

        public byte[] BackImageByteArray { get; set; }

    }

    public class IDCardParticipant
    {
        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("participantFullName")]
        public string ParticipantFullName { get; set; }

        [JsonProperty("participantType")]
        public string ParticipantType { get; set; }

        [JsonProperty("participantFirstName")]
        public string ParticipantFirstName { get; set; }

        [JsonProperty("participantLastName")]
        public string ParticipantLastName { get; set; }

        [JsonIgnore]
		public string PlanMemberDisplayID { get; set; }
    }
}
