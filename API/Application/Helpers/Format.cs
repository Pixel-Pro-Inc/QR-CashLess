using System;
using System.Collections.Generic;

namespace API.Helpers
{
    /// <summary>
    /// This class changes certain inputs into whatever format those inputs could be in
    /// </summary>
    public static class Format
    {
        public static string ListToString(List<string> input)
        {
            string x = "";

            foreach (var item in input)
            {
                x += input.IndexOf(item) == 0? item :", " + item;
            }

            return x;
        }
        /// <summary>
        /// Takes a full invoiceNumber of an order and returns just the specific order number
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns> Actual order number</returns>
        public static string OrderNumber(string OrderNumber) => OrderNumber.Substring(OrderNumber.IndexOf('_') + 1, 4);

        public static string AmountToString(float amount) // format 1,000,000.00
        {
            return String.Format("{0:n}", amount);
        }
    }
}
