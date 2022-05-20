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
        /// Finds the payment due for the user
        /// </summary>
        /// <returns></returns>
        public float CalculatePaymentDue();

        /// <summary>
        /// Returns a bool if the payment is past the due date.
        /// </summary>
        /// <returns> DateTime</returns>
        public bool isPastDueDate(DateTime Today);

        /// <summary>
        /// Gives the due date of the payment
        /// </summary>
        /// <returns> The date Today</returns>
        public DateTime DueDate();

        /// <summary>
        /// This should be creating the view of how the invoice would look like .
        /// 
        /// As its stands it void, but I expect it to return something
        /// </summary>
        /// <param name="payment"></param>
        public void CreateInvoice(float payment);

    }
}
