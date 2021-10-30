using API.Entities;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
       

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /*
         *  IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "KIxlMLOIsiqVrQmM0V7pppI1Ao67UPZv5jOdU0QJ",
            BasePath = "https://rodizoapp-default-rtdb.firebaseio.com/",

        };
        IFirebaseClient firebaseClient;

         [HttpPost]
        public IActionResult PostNewData(AppUser user)
        {
            try
            {
                SendDataToDatabase(user);
                ModelState.AddModelError(string.Empty, "Added successfully");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw;
            }
            return View();
        }

        
         private void SendDataToDatabase(AppUser user)
         {
            firebaseClient = new FireSharp.FirebaseClient(config);
            PushResponse response = firebaseClient.Push("Info/", user);
            user.UserName = response.Result.name;
            SetResponse setResponse = firebaseClient.Set("Info/" + user.UserName, user);
         }*/
    }

}
