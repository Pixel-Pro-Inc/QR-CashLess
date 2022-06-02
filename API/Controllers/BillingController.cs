using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This sends emails to the debtors and confirmation messages when paid.
    /// 
    /// 
    /// </summary>
    /// <remarks>
    /// It will have very little to no business logic. Any computation should be service by
    /// <see cref="BillingServices"/>
    /// </remarks>
    public class BillingController:BaseApiController
    {
        // The service to calcutate all the business logic with. Noone but this controller should acess this so its private
        private IBillingServices _billingServices;
        private IFirebaseServices _firebaseServices;

        // I haven't tested the injection but it should work
        public BillingController(IBillingServices billingServices, IFirebaseServices firebaseServices)
        {
            _billingServices = billingServices;
            _firebaseServices = firebaseServices;
        }

        // NOTE: Within BillingServices there is a method that is to be called automatically that calls this method.
        /// <summary>
        /// This will be an automatic sender that will send the bill as an email.
        /// </summary>
        /// <remarks> It will be given the client info and if it is past due it will send this method
        /// <para>
        /// Its internal cause <see cref="BillingServices"/> need to use it, but its not public cause I don't want anyone to just tamper with it
        /// </para>
        /// </remarks>
        /// <param name="BillInfo"></param>
        /// <returns></returns>
        [HttpGet("sendbill/{BillInfo}")]
        internal void /*async Task<IActionResult>*/ BillSender (UserDto UserDto)
        {
            //Sets the parameters to do the billing logic with
            SetParameters(UserDto);

            // Checks if the time to send is not now
            // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
            if (!_billingServices.isPastDueDate(System.DateTime.Today)){  /*Should display a message $"You don't need to pay anything just yet. The due date is {Bill.DueDate()}"*/ }

            SendBillToUser();
            InformBillToDeveloper();
           
            // Now that you are finished with the logic it will set it back to nothing
            SetParameters(null);

            //return 

        }

        /// <summary>
        /// Send email to developers about the ability to stop the branches functionality.
        /// </summary>
        private void InformBillToDeveloper()
        {
            // TODO: Send email to developers about the ability to send Emails.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the bill as an Email to the debtor
        /// </summary>
        private void SendBillToUser()
        {
            //Calculates how much is due
            double payment=_billingServices.CalculateTotalPaymentDue();

            // TODO: Create message in the bill format we want

            #region Invoice Logic

             //Bill.CreateInvoice(payment);

            #endregion

            // TODO: Send email/ SMS wrapped in paypal intergration logic

        }

        // Updates the database on payment made

        // Could send SMS to the debtor

        // Could send Invoice Email to the debtor

        // REFACTOR: These two methods could have their information sent up with a simple dto that fits a model that gives the view everything it needs
        //Tells the debtor how much is due
        [HttpGet("paymentamount")]
        public string GetTotalPaymentDue() => _billingServices.CalculateTotalPaymentDue().ToString();

        // Gets the due date of the User
        [HttpGet("duedate")]
        public string GetDueDate() => _billingServices.DueDate().ToString();


        [HttpGet("getbilledusers")]
        public async Task<List<BilledUser>> GetBilledUsers() => await _firebaseServices.GetBilledAccounts();



        /// <summary>
        /// This is an Billing exclusive method that was extracted to set the user
        /// </summary>
        /// <param name="User"></param>
        private async void SetParameters(UserDto UserDto)
        {

            AppUser appUser = await _firebaseServices.GetUser(UserDto.Username);
            try 
            { 
                _billingServices.SetUser(appUser);
                _billingServices.SetCurrentDate(System.DateTime.Today);
            }
            catch (UnBillableUserException)
            {
                // Should display a message $"You aren't properly set up to be billed by Pixel Pro. Please contact them to recover Services"
                throw;
            }
        }

    }
}
