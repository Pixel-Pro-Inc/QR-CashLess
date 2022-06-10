using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBillingServices, BillingServices>();
            services.AddScoped<IFirebaseServices, FirebaseServices>();

            // NOTE: This sets up AutoMapper in our project. It will need a Profile class that defines named configurations
            // you plan to use. See AutoMapperProfiles for clarification
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            //services.AddScoped<LogUserActivity>();

            return services;
        }
    }
}
