using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace API.Controllers
{
    public class SMSController : BaseApiController
    {
        public SMSController() { }


        [HttpPost("send/complete/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendOrderCompleteSMS(string phoneNumber, string orderNumber)
        {
            string accountSid = Configuration["twillosettings:accountSid"];

            string apiKeySid = Configuration["twillosettings:apiKeySid"];
            string apiKeySecret = Configuration["twillosettings:apiKeySecret"];// Do not put this in git hub at all, right now its in the gitignore keep it there

            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = $"Rodizio Express\n\nYour order #{orderNumber} is ready! Go to the till to collect. Thank you for your purchase.\n\nPowered by Pixel Pro";

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

            string msgBody = $"Rodizio Express\n\nYour order #{orderNumber} has been cancelled.\n\nPowered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            return phoneNumber;
        }

        //Make sure the token is made using QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param), it won't make sense if you try to use your own.
        [HttpPost("send/resetpassword/{phoneNumber}/{token}")]
        public async Task<ActionResult<string>> SendResetPasswordSMS(string phoneNumber, string token)
        {
            string accountSid = Configuration["twillosettings:accountSid"];

            string apiKeySid = Configuration["twillosettings:apiKeySid"];
            string apiKeySecret = Configuration["twillosettings:apiKeySecret"];// Do not put this in git hub at all, right now its in the gitignore keep it there

            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = $"Rodizio Express\n\nThis is your password reset token {token}.\n\nPowered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            return phoneNumber;
        }
    }
}
