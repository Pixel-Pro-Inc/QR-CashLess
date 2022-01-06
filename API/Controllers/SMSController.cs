using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace API.Controllers
{
    public class SMSController : BaseApiController
    {

        [HttpPost("send/complete/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendOrderCompleteSMS(string phoneNumber, string orderNumber)
        {
            //From rodizio express api key
            string accountSid = "SK4dfe74260f4947fc4be3c85fe774c21a";
            string authToken = "IR0mVNFst7gx7f8vQMebM9pGwyx2DJ2l";

            TwilioClient.Init(accountSid, authToken);

            string msgBody = "Rodizio Express. Your order #" + orderNumber + " is ready! Go to the till to collect. Thank you for your purchase. Powered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            return phoneNumber;
        }

        [HttpPost("send/cancel/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendOrderCancelSMS(string phoneNumber, string orderNumber)
        {
            //From rodizio express api key
            string accountSid = "SK4dfe74260f4947fc4be3c85fe774c21a";
            string authToken = "IR0mVNFst7gx7f8vQMebM9pGwyx2DJ2l";

            TwilioClient.Init(accountSid, authToken);

            string msgBody = "Rodizio Express. Your order #" + orderNumber + " has been cancelled. Powered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            return phoneNumber;
        }
    }
}
