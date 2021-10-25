namespace MobileClaims.Core.Entities
{
    public class YearTotalRemain
    {
        public string AccountName { get; set; }
        public int preYear { get; set; }
        public int currentYear { get; set; }
        public double PreYearTotal { get; set; }
        public double currentYearTotal { get; set; }
        public double SumofTotalRemaining { get; set; }
    }
}
