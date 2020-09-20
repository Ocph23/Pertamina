using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Data;

namespace WebApp.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           // var user = (IdentityUser)context.HttpContext.Items["User"];
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            if (!string.IsNullOrEmpty(Roles))
            {
                var splite =Roles.Split(",");
                var found = false;
                foreach (var item in splite)
                {
                    var role = item.ToLower().Trim();
                    if (context.HttpContext.User.IsInRole(role))
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
                                       
        public string Roles { get; set; }
    }
}