using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Interface for the BillingService. 
    /// Will state the required methods and actions needed to perform the billing 
    /// </summary>
    public interface IBillingServices
    {
        /// <summary>
        /// This is to set the user so that the billing services can work in it
        /// </summary>
        /// <param name="user"> This will be given in by the Dto in the BillingController or by whatever is calling this method</param>
        public void SetUser(AppUser user);

        /// <summary>
        /// Finds the total payment due for the user
        /// </summary>
        /// <returns></returns>
        public double CalculateTotalPaymentDue();

        /// <summary>
        /// This should be creating the view of how the invoice would look like .
        /// 
        /// As its stands it void, but I expect it to return something
        /// </summary>
        /// <param name="payment"></param>
        public void CreateInvoice(float payment);

        /// <summary>
        /// Sets the current date so that BillingServices can use it to work the logic
        /// </summary>
        /// <param name="date">Given date by the user</param>
        public void SetCurrentDate(DateTime date);
        /// <summary>
        ///  Returns a bool if the payment is past the due date.
        /// </summary>
        /// <param name="Today"> This is hopefully what the system's date today is</param>
        /// <returns>True or false</returns>
        public bool isPastDueDate(DateTime Today);
        /// <summary>
        /// Sets the due date of the users next payment
        /// </summary>
        public void SetDueDate(DateTime DueDate);
        /// <summary>
        /// Sets the due date of the users next payment a certain number of months away from given date
        /// </summary>
        public void SetDueDate(DateTime Today, int MonthsfromDate);
        /// <summary>
        /// Gives the due date of the payment
        /// </summary>
        /// <returns> The date Today</returns>
        public DateTime DueDate();

    }
}
