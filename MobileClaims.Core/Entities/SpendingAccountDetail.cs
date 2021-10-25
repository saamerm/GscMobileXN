using Newtonsoft.Json;
using System;

namespace MobileClaims.Core.Entities
{
    public class SpendingAccountDetail
    {
        [JsonProperty("modelId")]
        public string ModelID { get; set; }

        [JsonProperty("name")]
        public string AccountName { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }
        public string YearAsString
        {
            get
            {
                return Year.ToString();
            }
        }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        public string StartDateAsString
        {
            get
            {
                return StartDate.ToString("MMMM dd, yyy");
            }
        }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        public string EndDateAsString
        {
            get
            {
                return EndDate.ToString("MMMM dd, yyy");
            }
        }

        [JsonProperty("deposited")]
        public double Deposited { get; set; }
        public string DepositedAsString 
        { 
            get
            {
                return Deposited.ToString("0.00");
            }
        }

        [JsonProperty("usedToDate")]
        public double UsedToDate { get; set; }
        public string UsedToDateAsString
        {
            get
            {
                return UsedToDate.ToString("0.00");
            }
        }

        [JsonProperty("remaining")]
        public double Remaining { get; set; }
        public string RemainingAsString
        {
            get
            {
                return Remaining.ToString("0.00");
            }
        }

        [JsonProperty("useByDate")]
        public DateTime UseByDate { get; set; }        
        public string UseByDateAsString 
        {
            get
            {
                return UseByDate.ToString("MMMM dd, yyy");
            }
        }
    }
}
