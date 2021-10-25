namespace MobileClaims.Core.Entities
{
    public class WireLogo
    {
        public int ID { get; set; }
        public int EmployerID { get; set; }
        public string ImageName { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageContentType { get; set; }
    }
}
