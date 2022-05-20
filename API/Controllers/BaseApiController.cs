using API.Data;
using API.Entities;
using API.Helpers;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    //[ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        protected readonly FirebaseDataContext _firebaseDataContext;
        protected static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        protected static readonly HttpClient client = new HttpClient();

        public BaseApiController()
        {
            // @Yewo: NOTE: It appears we use legacy deprecated credentials and we should switch to Admin SDK, so that's why it is throwing the No host exception ........prolly
            _firebaseDataContext = new FirebaseDataContext();
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }

}
