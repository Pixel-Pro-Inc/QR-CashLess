
using API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace API.Infrastructure.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Microsoft.AspNetCore.Mvc.Controller
    {
        // OBSOLETE: We are phasing this out to use firebaseServices
        //protected readonly FirebaseDataContext _firebaseDataContext;

        protected readonly IFirebaseServices _firebaseServices;
        static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        protected static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true).Build();
        protected static readonly HttpClient client = new HttpClient();

        public BaseApiController(IFirebaseServices firebaseServices)
        {
            // NOTE: The below comment is independent of of the firebase refactor
            // @Yewo: NOTE: It appears we use legacy deprecated credentials and we should switch to Admin SDK, so that's why it is throwing the No host exception ........prolly
            //_firebaseDataContext = new FirebaseDataContext();
            _firebaseServices = firebaseServices;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }

}
