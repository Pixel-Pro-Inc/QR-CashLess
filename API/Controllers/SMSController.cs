using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace API.Controllers
{
    public class SMSController : BaseApiController
    {
        public SMSController() :base() { }


        [HttpPost("send/complete/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendOrderCompleteSMS(string phoneNumber, string orderNumber)
        {
            string accountSid = Configuration["twillosettings:accountSid"];

            string apiKeySid = Configuration["twillosettings:apiKeySid"];
            string apiKeySecret = Configuration["twillosettings:apiKeySecret"];// Do not put this in git hub at all, right now its in the gitignore keep it there

            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = "Rodizio Express. Your order #" + orderNumber + " is ready! Go to the till to collect.\n Thank you for your purchase.\n Powered by Pixel Pro";

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
            string accountSid = Configuration["twillosettings:accountSid"];

            string apiKeySid = Configuration["twillosettings:apiKeySid"];
            string apiKeySecret = Configuration["twillosettings:apiKeySecret"];// Do not put this in git hub at all, right now its in the gitignore keep it there

            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = "Rodizio Express. Your order #" + orderNumber + " has been cancelled. Powered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            return phoneNumber;
        }

        //Make sure the token is made using QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param), it won't make sense if you try to use your own.
        [HttpPost("send/resetpassword/{phoneNumber}")]
        public async Task<ActionResult<string>> SendResetPasswordSMS(string phoneNumber, string temporarypasswordtoken)
        {
            string accountSid = Configuration["twillosettings:accountSid"];

            string apiKeySid = Configuration["twillosettings:apiKeySid"];
            string apiKeySecret = Configuration["twillosettings:apiKeySecret"];// Do not put this in git hub at all, right now its in the gitignore keep it there

            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = $"Rodizio Express. Please click this {temporarypasswordtoken} link to get your temporary password token. Please change your password withing 2 hours.\n Powered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            return phoneNumber;
        }
    }
}
