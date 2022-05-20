using API.Entities;
using API.Exceptions;
using API.Interfaces;
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
        #region Properties

        // The user will be set by whatever calls Billingservices, but it should be accessed by anyone outside Billing. Since all the information
        // needed that will be possible to get from billing will be handed off by the methods themselves
        /// <summary>
        /// You can only set this property. 
        /// </summary>
        public BilledUser _User { private get; set; }

        #endregion

        #region Methods
        public float CalculatePaymentDue()
        {
            // TODO: CalculatePaymentDue() hasn't been done
            throw new NotImplementedException();
        }


        public DateTime DueDate()
        {
            // TODO: DueDate() hasn't been done
            throw new NotImplementedException();
        }


        // TODO: Delete the testing branch

        public bool isPastDueDate(DateTime Today) => Today > DueDate();

        // Since this will be automatic, I expect this method to be fired everytime a user logs in, but that should be changed in the future
        // That is why the commented thrown exception is relevant and wasn't deleted
        // We also explicity cast to (BilledUser) cause we have it specifically set as (BilledUser) in this services
        public void SetUser(AppUser user) => _User = user is BilledUser? (BilledUser)user : (BilledUser)user/*throw new UnBillableUserException()*/;

        public void CreateInvoice(float payment)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
