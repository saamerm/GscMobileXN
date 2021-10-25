using MobileClaims.Core.Extensions;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    public class MenuItemLink
    {
        private string _rel;

        [JsonProperty("rel")]
        public string Rel
        {
            get => _rel;
            set
            {
                _rel = value;
                MenuItemRel = _rel.ToEnum<MenuItemRel>();
            }
        }

        [JsonProperty("href")]
        public string HRef { get; set; }

        [JsonIgnore]
        public MenuItemRel MenuItemRel { get; set; }
    }
}