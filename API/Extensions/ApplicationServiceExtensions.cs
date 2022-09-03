using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddScoped<IBaseService, _BaseService>();

            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();

            // Services that are used across the Infrastructre Controller layer
            services.AddScoped<IBillingServices, BillingServices>();
            services.AddScoped<IReportServices, ReportServices>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFirebaseServices, FirebaseServices>();

            // NOTE: This sets up AutoMapper in our project. It will need a Profile class that defines named configurations
            // you plan to use. See AutoMapperProfiles for clarification
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            //services.AddScoped<LogUserActivity>();

            return services;
        }
    }
}
