using API.Entities;
using API.Controllers;
using API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Interface for the BillingService. 
    /// It states the required methods and actions needed to perform the billing 
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
        /// <para> It does this by finding the number of the overdue months and multiplying them by how much we are charging for that branch. 
        /// <para>This is to be changed cause we have a different way of calculating this now</para>
        /// </para>
        /// </summary>
        /// <returns></returns>
        public float CalculateTotalPaymentDue();

        /// <summary>
        /// This should be creating the view/PDf of how the invoice would look like .
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
        ///  <para> This does a comparison:</para>
        ///  <para> If <paramref name="Today"/> > <see cref="AdminUser.DuePaymentDate"/>, returns true</para>
        /// </summary>
        /// <param name="Today"> This is hopefully what the system's date today is</param>
        /// <returns><see cref="Boolean"/></returns>
        public bool isPastDueDate(DateTime Today);
        /// <summary>
        /// Sets the due date of the users next payment
        /// <para> It is important that the due date is only set when registering <see cref="AppUser"/> in <see cref="AccountController.Register(RegisterDto)"/>
        /// <para> And again only in the BillingServices. Cause if we have too many of them it will be difficult to maintain</para></para>
        /// </summary>
        public void SetDueDate(DateTime DueDate);
        /// <summary>
        /// Sets the due date of the users next payment a certain number of months away from given date
        /// <para> It is important that the due date is only set when registering <see cref="AppUser"/> in <see cref="AccountController.Register(RegisterDto)"/>
        /// <para> And again only in the BillingServices. Cause if we have too many of them it will be difficult to maintain</para></para>
        /// </summary>
        public void SetDueDate(DateTime Today, int MonthsfromDate);
        /// <summary>
        /// Gives the due date of the payment
        /// </summary>
        /// <returns> The date Today</returns>
        public DateTime DueDate();
        /// <summary>
        /// This is so we have the sales the the users branch generated
        /// 
        /// <para>
        /// I expect this to get the <see cref="ReportDto.StartDate"/> and <see cref="ReportDto.EndDate"/> it needs from the billingServices properties. </para>
        /// <para> It also resets the Duepaymentdate if the method fired correctly</para>
        /// </summary>
        public void SetSalesinUsersBranch();

        /// <summary>
        /// If you are looking at this, you really need to refactor <see cref="BillingController.SetParameters(DTOs.BilledUserDto)"/>
        /// <para> Its clusmy and we can do better, but I was in a rush. Navigate to the method for more info</para>
        /// </summary>
        /// <param name="smses"></param>
        public void SetSMSSent(List<SMS> smses);
    }
}
