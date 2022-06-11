using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    /// <summary>
    /// This is so We can have the datetime changed to the format PixelPro uses
    /// </summary>
    public static class DateTimeFormatExtensions
    {
        /// <summary>
        /// This is so we can have the datetime changed to the forwardSlash format PixelPro uses
        /// <para> This adds a forward slash between the double digits day, Month and 4 digit Year</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns> A <see cref="string"/> in the format we use for orders and other database entities </returns>
        public static string ToPixelProForwardSlashFormat(this DateTime source)
        {
            return source.Day.ToString("00") + "/" + source.Month.ToString("00") + "/" + source.Year.ToString("0000");
        }

        /// <summary>
        /// This is so we can have the datetime changed to the dash format PixelPro uses
        /// <para> This adds a dash between the double digits day, Month and 4 digit Year</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns> A <see cref="string"/> in the format we use for orders and other database entities </returns>
        public static string ToPixelProDashFormat(this DateTime source)
        {
            return source.Day.ToString("00") + "-"+ source.Month.ToString("00") + "-" + source.Year.ToString("0000");
        }

        /// <summary>
        /// This is so we can have the datetime changed to the Invoice format PixelPro uses
        /// <para> This just takes the double digits day, Month and 4 digit Year and addes them together</para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns> A <see cref="string"/> in the format we use for invoice numbers </returns>
        public static string ToPixelProInvoiceFormat(this DateTime source)
        {
            return source.Day.ToString("00") + source.Month.ToString("00") + source.Year.ToString("0000");
        }

        /// <summary>
        /// This is so we can have OrderNumbers changed to the Date in the format PixelPro uses
        /// <para> This just takes the double digits day, Month and 4 digit Year makes a new <see cref="DateTime"/></para>
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns> A <see cref="DateTime"/> in the format Year/Month/day </returns>
        public static DateTime FromPixelProOrderNumbertoDateFormat(this string OrderNumber)
        {
            string orderDate = OrderNumber.Substring(0, 10);
            string real = orderDate.Replace("-", "/");

            int day = 0;
            int month = 0;
            int year = 0;
            // see to see if the string is indeed an order number, if not it won't work and throw an exception
            try
            {
                day = Int32.Parse(real.Substring(0, 2));
                month = Int32.Parse(real.Substring(3, 2));
                year = Int32.Parse(real.Substring(6, 4));
            }
            catch (Exception ex)
            {
                throw new NotOrderNumberException("The string you gave is not an OrderNumber/ In the correct OrdernNumber format. Check the DateTimeFormatExtension and" +
                    "compare the formating you used and the one used in the logic ", ex);
            }

            return  new DateTime(year, month, day);
        }

        /// <summary>
        /// Gets the first day of the month
        /// </summary>
        /// <param name="value"></param>
        /// <returns> <see cref="DateTime"/> date which is the first day of the month</returns>
        public static DateTime FirstDayOfMonth(this DateTime value)=> new DateTime(value.Year, value.Month, 1);

        /// <summary>
        /// Gets the last day of the month
        /// </summary>
        /// <param name="value"></param>
        /// <returns> <see cref="DateTime"/> date which is the last day of the month</returns>
        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return value.FirstDayOfMonth()
                .AddMonths(1)
                .AddMinutes(-1);
        }

    }
}
