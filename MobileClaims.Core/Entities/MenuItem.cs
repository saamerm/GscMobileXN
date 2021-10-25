using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MobileClaims.Core.Entities
{
    [DebuggerDisplay("{DisplayLabel} - {Children.Count}")]
    public class MenuItem
    {
        [JsonProperty("displayLabel")]
        public string DisplayLabel { get; set; }

        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; }

        [JsonProperty("totalCount")]
        public int Count { get; set; }

        [JsonProperty("children")]
        public IList<MenuItem> Children { get; set; }

        [JsonProperty("links")]
        public IList<MenuItemLink> Links { get; set; }

        [JsonIgnore]
        public bool ShouldShowCounter { get; set; }

        [JsonIgnore]
        public string CountDisplayValue
        {
            get
            {
                if (Count != 0)
                {
                    return Count.ToString();
                }
                return string.Empty;
            }
        }
    }
}