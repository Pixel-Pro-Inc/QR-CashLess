using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// This is a Controller in charge of all Online payments
    /// </summary>
    public class OnlinePaymentController:BaseApiController
    {
        public OnlinePaymentController(IFirebaseServices firebaseServices) :base(firebaseServices)
        {

        }

        [HttpPost("getaccesstoken")]
        public async Task<IActionResult> GetAccessToken()
        {
            //var client = new RestClient("https://api-gateway.sandbox.ngenius-payments.com/identity/auth/access-token");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Accept", "application/vnd.ni-identity.v1+json");
            //request.AddHeader("Content-Type", "application/vnd.ni-identity.v1+json");
            //IRestResponse response = client.Execute(request);
            return Ok("sO THE actrion result isn't the problemds");
        }

    }
}
