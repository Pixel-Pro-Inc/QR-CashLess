using API.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    /// <summary>
    /// This is the contrete application logic so that other layers that should be contracted to Services 
    /// </summary>
    public class _BaseService: IBaseService
    {
        /// <summary>
        /// This is so Services can access the information found in appsettings 
        /// </summary>
        static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        protected static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true).Build();

    }
}
