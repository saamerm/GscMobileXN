namespace MobileClaims.Core.Entities
{
    public class Address
    {
        public string PlanMemberId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string ProvinceCode { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}