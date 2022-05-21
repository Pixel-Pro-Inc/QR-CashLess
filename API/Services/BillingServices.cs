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
        public BillingServices()
        {
            _firebaseDataContext = new FirebaseDataContext();
        }

        #region Properties

        // The user will be set by whatever calls Billingservices, but it should be accessed by anyone outside Billing. Since all the information
        // needed that will be possible to get from billing will be handed off by the methods themselves
        /// <summary>
        /// You can only set this property. 
        /// </summary>
        internal BilledUser _User { private get; set; }

        // This is the date only used for by the Services but it indeed should be the current date
        DateTime Today { get; set; }

        FirebaseDataContext _firebaseDataContext;

        #endregion

        #region Methods

        //I thought, we could just store the value but then how will we accomodate for changes, We would have to have logic
        //to calculate those changes. It would be easier to simply calculate how much is due, each time, assuming a change has been made, (eg date,
        // ammount, number of branches being managed)
        public double CalculatePaymentDue()
        {
            // calculates the overdue span by finding how much longer Today is after due date
            TimeSpan overdueSpan = Today-_User.DuePaymentDate;
            // If today is before due date the user owes nothing
            if (Today.CompareTo(_User.DuePaymentDate) < 0) return 0.0f;

            // divides it by a month of Febuary, 28 days and takes the whole number
            int NumerofOverdueMonths = (int)overdueSpan.TotalDays / DateTime.DaysInMonth(Today.Year, 2);
            
            double result=0;
            // gets the monthly fee per branch and multiples it by the NumerofOverdueMonths
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
        // We also explicity cast to (BilledUser) cause we have it specifically set as (BilledUser) in this services
        // we also won't use isBilled in the Appuser to determine if we should use these services because if it is billed we should already have the user as a BilledUser type.
        public void SetUser(AppUser user) => _User = user is BilledUser? (BilledUser)user : (BilledUser)user/*throw new UnBillableUserException("This user isn't billed at the end of the month")*/;
       
        public void CreateInvoice(float payment)
        {
            throw new NotImplementedException();
        }

        // TODO: This is to be called by the automatic call. I'm thinking maybe it should be done by the hosting, azure and only 4 times a month
        /// <summary>
        /// Checks for the Users with overdue branches and sends them the bill while also sending the developers an email
        /// about the option of disabling functionality temporally.
        /// </summary>
        /// <returns></returns>
        private async Task ReportOverDueStatements()
        {
            List<BilledUser> OwingUsers = new List<BilledUser>();

            // TODO: It appears that you need to define the extension in the class that it should work on here.
            // I was thinking of defining an interface for the JsonConvertExtension and having that implemented in a Base Entity
            // Because the error it is throwing is saying that it needs to be defined in the class
            //List<BilledUser> billedUsers= await _firebaseDataContext.GetData("Account/BilledAccounts").FromJsonToObject<BilledUser>();

            List<BilledUser> billedUsers = await GetBilledAccounts();
            // Collects all the users who owe 
            foreach (var user in billedUsers)
            {

                // REFACTOR: Consider having this set to a specific period of time like, 8:00 in the morning
                if (user.DuePaymentDate < System.DateTime.Today)
                    OwingUsers.Add(user);
                continue;
            }

            // foreach OwingUser it should make a BillingDto and send it to the BillingController
            foreach (var user in OwingUsers)
            {
                BillInfoDto dto = new BillInfoDto
                {
                    User = user,
                    Date = System.DateTime.Today
                };
                new BillingController().BillSender(dto);

            }

        }

        /// <summary>
        /// This gets all the Billed Users from the database.
        /// 
        /// It wasn't defined in the interface because an async method has to have a body
        /// </summary>
        /// <returns></returns>
        private async Task<List<BilledUser>> GetBilledAccounts()
        {
            var response = await _firebaseDataContext.GetData("Account/BilledAccounts");
            List<BilledUser> items = new List<BilledUser>();
            for (int i = 0; i < response.Count; i++)
            {
                BilledUser item = JsonConvert.DeserializeObject<BilledUser>(((JObject)response[i]).ToString());

                items.Add(item);
            }
            return items;
        }


        #endregion

    }
}
