using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Exceptions;
using API.Extensions;
using API.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    /// <summary>
    /// This is service that will calculate all the necessary information that needs to be sent to and fro the debtor.
    /// It will deal with all the billing buisness logic.
    /// </summary>
    public class BillingServices : IBillingServices
    {
        IFirebaseServices _IFirebaseServices;
        public BillingServices(IFirebaseServices firebaseServices)
        {
            _IFirebaseServices = firebaseServices;
        }

        #region Properties
 
        /// <summary>
        /// This is what the billingService uses to represent the user. You can only set this property. 
        /// </summary>
        /// <remarks>
        /// The user will be set by whatever calls Billingservices, but it should not be accessed by anyone outside Billing. Since all the information
        /// needed that will be possible to get from billing will be handed off by the methods themselves
        /// </remarks>
        internal AdminUser _User { private get; set; }

        // This is the date only used for by the Services but it indeed should be the current date
        DateTime Today { get; set; }

        #endregion

        #region Methods

        //I thought, we could just store the value but then how will we accomodate for changes, We would have to have logic
        //to calculate those changes. It would be easier to simply calculate how much is due, each time, assuming a change has been made, (eg date,
        // ammount, number of branches being managed)
        public double CalculateTotalPaymentDue()
        {
            // calculates the overdue span by finding how much longer Today is after due date
            TimeSpan overdueSpan = Today-_User.DuePaymentDate;
            // If today is before due date the user owes nothing
            if (Today.CompareTo(_User.DuePaymentDate) < 0) return 0;

            // divides it by a month of Febuary, 28 days and takes the whole number
            int NumerofOverdueMonths = (int)overdueSpan.TotalDays / DateTime.DaysInMonth(Today.Year, 2);
            
            double result=0;
            // gets the current charge per branch and multiples it by the NumerofOverdueMonths
            foreach (var branch in _User.BilledBranchIds)
            {
                result += branch.Value * NumerofOverdueMonths;
            }
            // returns the product of the result and the monthly fee per branch
            return result;
        }

        public void SetCurrentDate(DateTime date) => Today = date;
        public DateTime DueDate() => _User.DuePaymentDate;
        public void SetDueDate(DateTime DueDate) => _User.DuePaymentDate = DueDate;
        public void SetDueDate(DateTime Today, int MonthsfromDate) => _User.DuePaymentDate=Today.AddMonths(MonthsfromDate);
        public bool isPastDueDate(DateTime Today) => Today > DueDate();

        // Since this will be automatic, I expect this method to be fired everytime a user logs in, but that should be changed in the future
        // That is why the commented thrown exception is relevant and wasn't deleted
        // We also explicity cast to (AdminUser) cause we have it specifically set as (AdminUser) in this services
        // we also won't use isBilled in the Appuser to determine if we should use these services because if it is billed we should already have the user as a AdminUser type.
        public void SetUser(AppUser user) => _User = user is AdminUser? (AdminUser)user : (AdminUser)user/*throw new UnBillableUserException("This user isn't billed at the end of the month")*/;
       
        public void CreateInvoice(float payment)
        {
            throw new NotImplementedException();
        }

        // TODO: This is to be called by the automatic call. I'm thinking maybe it should be done by the hosting, azure and only 4 times a month
        /// <summary>
        /// Checks for the Users with overdue branches and reports the findings
        /// </summary>
        /// <remarks>It does this by sending them the bill while also sending the developers an email
        /// about the option of disabling functionality temporally.</remarks>
        /// <returns></returns>
        private async Task ReportOverDueStatements()
        {
            List<AdminUser> OwingUsers = new List<AdminUser>();

            List<AdminUser> adminUsers = await _IFirebaseServices.GetAdminAccounts();
            // Collects all the users who owe 
            foreach (var user in adminUsers)
            {

                // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
                if (user.DuePaymentDate < System.DateTime.Today)
                    OwingUsers.Add(user);
                continue;
            }

            // foreach OwingUser it should make a BillingDto and send it to the BillingController
            foreach (var user in OwingUsers)
            {

                UserDto dto = new UserDto()
                {
                    Username = user.UserName
                };
                new BillingController(this,_IFirebaseServices).BillSender(dto);

            }

        }

        
       



        #endregion

    }
}
