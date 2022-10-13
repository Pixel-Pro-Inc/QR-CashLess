using Microsoft.Extensions.Configuration;
using RodizioSmartKernel.Infrastructure.Services;
using System;

namespace API.Infrastructure.Services
{
    public class BaseService:_BaseService
    {
        protected static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        protected static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true).Build();


    }
}
