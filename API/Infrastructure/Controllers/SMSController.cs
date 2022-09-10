using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
﻿using API.Interfaces;

using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using RodizioSmartKernel.Entities;

namespace API.Controllers
{
    public class SMSController : BaseApiController
    {
        // REFACTOR: Twillo sensitive date, we need to also deal with this. You wont find this in the base API controller
        private readonly string accountSid = Configuration["twillosettings:accountSid"];
        private readonly string apiKeySid = Configuration["twillosettings:apiKeySid"];
        private readonly string apiKeySecret = Configuration["twillosettings:apiKeySecret"];
        public SMSController(IFirebaseServices firebaseServices): base(firebaseServices){ }


        [HttpPost("send/complete/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendOrderCompleteSMS(string phoneNumber, string orderNumber)
        {
            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = $"Rodizio Express\n\nYour order #{orderNumber} is ready! Go to the till to collect. Thank you for your purchase order again at https://rodizioexpress.com.\n\nPowered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            StoreSMS("Order_Complete");

            return phoneNumber;
        }

        [HttpPost("send/cancel/{phoneNumber}/{orderNumber}")]
        public async Task<ActionResult<string>> SendOrderCancelSMS(string phoneNumber, string orderNumber)
        {
            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = $"Rodizio Express\n\nYour order #{orderNumber} has been cancelled order again at https://rodizioexpress.com.\n\nPowered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            StoreSMS("Order_Cancel");

            return phoneNumber;
        }

        [HttpPost("send/resetpassword/{phoneNumber}/{token}")]
        public async Task<ActionResult<string>> SendResetPasswordSMS(string phoneNumber, string token)
        {
            TwilioClient.Init(apiKeySid, apiKeySecret, accountSid);

            string msgBody = $"Rodizio Express\n\nThis is your password reset token {token}.\n\nPowered by Pixel Pro";

            var message = await MessageResource.CreateAsync(
                body: msgBody,
                from: "Rodizio",
                to: new Twilio.Types.PhoneNumber("+267" + phoneNumber)
            );

            StoreSMS("Account_Reset");

            return phoneNumber;
        }

        public async void StoreSMS(string origin)
        {
            int Id = (await _firebaseServices.GetData<SMS>("SMS")).Count;

            _firebaseServices.StoreData("SMS/" + Id, new SMS()
            {
                Id = Id,
                Origin = origin,
                DateSent = DateTime.UtcNow
            });
        }
    }
}
