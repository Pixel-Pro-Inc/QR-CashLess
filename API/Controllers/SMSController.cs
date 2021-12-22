using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace API.Controllers
{
    public class SMSController : BaseApiController
    {
        
        public SMSController() :base() { }

        [HttpPost("send/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendSMS(string phoneNumber, string orderNumber)
        {
            string accountSid = Configuration["twillosettings:accountSid"];
            string authToken = Configuration["twillosettings:authToken"];// Do not put this in git hub at all, right now its in the gitignore keep it there

            TwilioClient.Init(accountSid, authToken);

            string msgBody = "Rodizio Express. Your order #" + orderNumber + " is ready! Go to the till to collect.\n Thank you for your purchase.\n Powered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );
            return phoneNumber;
        }
    }
}
