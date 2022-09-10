using Microsoft.AspNetCore.Mvc.Filters;
using API.Application.Extensions;
using API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Helpers
{
    public class LogUserActivity// : IAsyncActionFilter
    {
        /*
         public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            //var username = resultContext.HttpContext.User.GetUsername();

            //var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            var user = await repo.GetUserbyUsernameAsync(username);
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();

        }
         */

    }
}
