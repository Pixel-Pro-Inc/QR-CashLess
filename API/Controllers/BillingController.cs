﻿using API.DTOs;
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
    /// I don't expect to put any if only a little of the business logic here.
    /// It will have very little to no business logic. 
    /// </summary>
    public class BillingController:BaseApiController
    {
        // The service to calcutate all the business logic with. Noone but this controller should acess this so its private
        private IBillingServices Bill;

        // I haven't tested the injection but it should work
        public BillingController(IBillingServices billingServices)
        {
            Bill = billingServices;
        }

        // REFACTOR: Within BillingServices there is a method that is to be called automatically that calls this method.
        /// <summary>
        /// This will be an automatic sender that will send the bill email.
        /// It will be given the date of the client, and client info and if it is past due it will send this method
        /// 
        /// It doesn't need to be public, cause its called automatically, and noone should really tamper with it. Its internal cause Services need to use it
        /// </summary>
        /// <param name="BillInfo"></param>
        /// <returns></returns>
        [HttpGet("sendbill/{BillInfo}")]
        internal void /*async Task<IActionResult>*/ BillSender (UserDto UserDto)
        {
            //Sets the parameters to do the billing logic with
            SetParameters(UserDto);

            // Checks if the time to send is not now
            // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
            if (!Bill.isPastDueDate(System.DateTime.Today)){  /*Should display a message $"You don't need to pay anything just yet. The due date is {Bill.DueDate()}"*/ }

            SendBillToUser();
            InformBillToDeveloper();
           
            // Now that you are finished with the logic it will set it back to nothing
            SetParameters(null);

            //return 

        }

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
            double payment=Bill.CalculateTotalPaymentDue();

            // TODO: Create message in the bill format we want

            #region Invoice Logic

             //Bill.CreateInvoice(payment);

            #endregion

            // TODO: Send email/ SMS wrapped in paypal intergration logic

        }

        // Updates the database on payment made

        // Could send SMS to the debtor

        // Could send Invoice Email to the debtor

        //Tells the debtor how much is due
        [HttpGet("paymentamount")]
        public string GetTotalPaymentDue() => Bill.CalculateTotalPaymentDue().ToString();

        // Gets the due date of the User
        [HttpGet("duedate")]
        public string GetDueDate() => Bill.DueDate().ToString();

        // TODO: Doesn't have GetBilledAccounts in the interface because interface can't define async methods
        [HttpGet("getbranches")]
        public async Task<List<BilledUser>> GetUsersBilledBranches() => await Bill.GetBilledAccounts();



        /// <summary>
        /// This is an Billing exclusive method that was extracted to set the user
        /// </summary>
        /// <param name="User"></param>
        private void SetParameters(UserDto UserDto)
        {
            try 
            { 
                // FIXME: This should be got from the database as a user from the UserDto 
                Bill.SetUser(BillInfoDto.User);
                Bill.SetCurrentDate(System.DateTime.Today);
            }
            catch (UnBillableUserException)
            {
                // Should display a message $"You aren't properly set up to be billed by Pixel Pro. Please contact them to recover Services"
                throw;
            }
        }

    }
}
