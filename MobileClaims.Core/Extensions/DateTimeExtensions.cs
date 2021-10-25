using System;

namespace MobileClaims.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dateTime)
        {
            int years = DateTime.Now.Year - dateTime.Year;

            if ((dateTime.Month > DateTime.Now.Month)
                || (dateTime.Month == DateTime.Now.Month && dateTime.Day > DateTime.Now.Day))
            {
                years--;
            }

            return years;
        }

        public static bool IsNotFutureDate(this DateTime date)
        {
            int dc = DateTime.Compare(date.Date, DateTime.Now.Date);
            if (dc <= 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsWithin24Months(this DateTime date)
        {
            DateTime twoYearsAgo = DateTime.Today.AddMonths(-24);
            if (date >= twoYearsAgo)
            {
                return true;
            }
            return false;
        }
    }
}