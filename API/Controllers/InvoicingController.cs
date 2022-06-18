using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    // TODO: Invoicing has to be done so that branch managers know how much to pay
    // This is the controller that will be used to invoice the other branch managers ( or every branch manager the cost we have for our service.
    /*
     Since we still don't have a concrete plan yet we are yet to populate the controller, but the Idea is that we send them an email/ fax/ to tell 
    them how much they owe us. And if the payment isn't made the service would be discontinue ready to be reinstated once the requirement and 
    full payment is made. 
    When the payments are expected is also up for discussion. Will it be at a specific date, regardless of start time? or will it be different for every 
    manager who opens ( seems ideal for them, not really for us) or will it be at the end of the business month
     */
    public class InvoicingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
