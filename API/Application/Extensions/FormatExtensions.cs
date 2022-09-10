using API.Application.Exceptions;
using System;
using System.Collections.Generic;

namespace API.Application.Extensions
{
    /// <summary>
    /// These are methods used to structure data in desired ways. From Ordernumber to the date
    /// </summary>
    public static class FormatExtensions
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

        /// <summary>
        /// Takes a list of string and makes them into a single string.
        /// It just uses the index of the string in the list and adds it respectively
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ListToString(this List<string> input)
        {
            string x = "";

            foreach (var item in input)
            {
                x += input.IndexOf(item) == 0 ? item : ", " + item;
            }

            return x;
        }
        /// <summary>
        /// Takes a full invoiceNumber of an order and returns just the specific order number
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns> Actual order number</returns>
        public static string OrderNumber(this string OrderNumber) => OrderNumber.Substring(OrderNumber.IndexOf('_') + 1, 4);

        /// <summary>
        /// Takes an <paramref name="amount"/> and format it as a string as such: 1,000,000.00
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string AmountToString(this float amount) => String.Format("{0:n}", amount);




    }
}
