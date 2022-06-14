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
    // TODO: Have the Git commit explain why we fixed it and also add it to the error list we have in Notes
    // GITRACK: look up

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

        // I haven't tested the injection but it should work
        public BillingController(IBillingServices billingServices, IFirebaseServices firebaseServices):base(firebaseServices)
        {
            _billingServices = billingServices;
        }

        // NOTE: Within BillingServices there is a method that is to be called automatically that calls this method.

        /// <summary>
        /// This will be an automatic sender that will send the bill as an email.
        /// </summary>
        /// <remarks> 
        /// <para>
        /// It will be given the <see cref="BilledUserDto"/>, checks if they are past due,
        /// </para>
        /// <para>
        /// fires <see cref="_billingServices.CalculateTotalPaymentDue()"/>,
        /// </para> 
        /// <para>
        /// Then sends the bill to the user and developer and resets the billingService properties
        /// </para>
        /// <para>
        /// Its internal cause <see cref="BillingServices"/> need to use it, but its not public cause I don't want anyone to just tamper with it
        /// </para>
        /// </remarks>
        /// <param name="BillingDto"></param>
        /// <returns></returns>
        [HttpPost("sendbill/{BillingDto}")]
        public async Task<IActionResult> BillSender (BilledUserDto BillingDto)
        {
            //Sets the parameters to do the billing logic with
            await SetParameters(BillingDto);

            // Checks if the time to send is not now
            if (!_billingServices.isPastDueDate(System.DateTime.Now))
            {
                // in case this was requested by a user
                return Ok($"You don't need to pay anything just yet. The due date is {_billingServices.DueDate()}");
            }

            //Calculates how much is due
            // REFACTOR: You could just remove the calculatePaymentDue() here, or at least extract it to work within CreateInvoice
            //float payment = _billingServices.CalculateTotalPaymentDue();

            //FIXME: Throws error here
            // Creates a pdf to attach to the email
            _billingServices.CreateInvoice();

            // TESTING: When done testing the invoice creation remove this
            //SendBillToUser();
            //InformBillToDeveloper();
           
            // Now that you are finished with the logic it will set it back to nothing
            await SetParameters(null);

            return Ok();

        }

        // TODO: put this in the report service
        /// <summary>
        /// Send email to developers about the ability to stop the branches functionality. Since the bill had already informed them it can terminate
        /// </summary>
        private void InformBillToDeveloper()
        {
            // get word document from billingservices

            // turn it into a pdf

            // add pdf to an email

            // TODO: Send email to developers informing of sent bills.
            throw new NotImplementedException("InformBillToDeveloper() not done");
        }

        // TODO: put this in the report service
        /// <summary>
        /// Sends the bill as an Email to the debtor
        /// </summary>
        private void SendBillToUser()
        {

            // get word document from billingservices

            // turn it into a pdf

            // add pdf to an email

            // TODO: Send email/ SMS 
            // TODO: Try merging work from the emailService branch since recent work was done in it

            throw new NotImplementedException("SendBillToUser() not done");

        }

        // Updates the database on payment made

        // Could send SMS to the debtor

        // REFACTOR: These two methods could have their information sent up with a simple dto that fits a model that gives the view everything it needs
        //Tells the debtor how much is due
        [HttpGet("paymentamount")]
        public string GetTotalPaymentDue() => _billingServices.CalculateTotalPaymentDue().ToString();

        // Gets the due date of the User
        [HttpGet("duedate")]
        public string GetDueDate() => _billingServices.DueDate().ToString();

        /// <summary>
            /// This is an Billing exclusive method that was extracted to set all the properties <see cref="BillingServices"/> will use
            /// </summary>
            /// <param name="User"></param>
        private async Task SetParameters(BilledUserDto BillingDto)
        {
            // Gets the user by the username
            AppUser appUser = await _firebaseServices.GetUser(BillingDto.Username);

            // REFACTOR: Figure out a way to coalesce the parameter setting in the service to include Sms as well I could put in the service but it was easier to put it here
            // Gets the sms sent by the branch.
            // TESTING: Removing this to test the doc creation proper
            //List<SMS> Smses = await _firebaseServices.GetSMSinBranch();

            try 
            { 
                _billingServices.SetUser(appUser);
                _billingServices.SetCurrentDate(System.DateTime.Today);
                // FIXME: 655 orders is too large so I will dealwith this later
                //_billingServices.SetSalesinUsersBranch();
                //_billingServices.SetSMSSent(Smses);
            }
            catch (UnBillableUserException)
            {

                throw;
            }
        }

        // We were using this to test in post man
        //[HttpPost("testing/{BillingDto}")]
        //public async Task<IActionResult> Testing(BilledUserDto BillingDto)
        //{
        //    await _firebaseServices.GetAdminAccounts();
        //    Console.WriteLine(BillingDto.Username + "the syntax is wrong");
        //    return Ok("sO THE actrion result isn't the problemds");
        //}

    }
}
