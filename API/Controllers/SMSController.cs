using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private static readonly HttpClient client = new HttpClient();

        public SMSController():base()
        {

        }
        [HttpPost("send/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendSMS(string phoneNumber, string orderNumber)
        {
            string accountSid = "ACb6bff2fe1dd75e7f0ef7ec2c0d4d7b84";
            string authToken = "71c2b37eb6e48f1eb985e0bce05861f0";

            TwilioClient.Init(accountSid, authToken);

            string msgBody = "Rodizio Express. Your order #" + orderNumber + " is ready! Go to the till to collect. Thank you for your purchase. Powered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );
            return phoneNumber;
        }
    }
}
