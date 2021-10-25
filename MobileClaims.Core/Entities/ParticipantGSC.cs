using System;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class ParticipantGSC
    {
        [JsonProperty("participantNumber")]
        public string ParticipantNumber { get; set; }

        [JsonProperty("participantFirstName")]
        public string FirstName { get; set; }

        [JsonProperty("participantLastName")]
        public string LastName { get; set; }

        [JsonProperty("participantStatus")]
        public string Status { get; set; }

		public string FullName
		{
			get
			{
				return string.Format("{0} {1}", FirstName, LastName);
			}
		}


        public string FullNameWithID
        {
            get
            {
                return string.Format("{0} {1} {2}", ParticipantNumber, FirstName, LastName);
            }
        }

        [JsonProperty("participantBirthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("participantProvinceCode")]
        public string ProviceCode { get; set; }
    }
}
