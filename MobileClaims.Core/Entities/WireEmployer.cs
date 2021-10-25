using System.Collections.Generic;

namespace MobileClaims.Core.Entities
{
    public class WireEmployer
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<WireLogo> Logos { get; set; }
    }
}
