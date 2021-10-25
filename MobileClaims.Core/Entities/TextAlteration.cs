using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class TextAlteration
    {

        [JsonProperty("alterationTag")]
        public string AlterationTag { get; set; }

        [JsonProperty("sequenceNo")]
        public int SequenceNo { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
