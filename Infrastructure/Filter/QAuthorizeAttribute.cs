using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Infrastructure.Filter
{
    public class QAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            if (actionDescriptor == null || string.IsNullOrEmpty(actionDescriptor.ControllerName) || string.IsNullOrEmpty(actionDescriptor.ActionName))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string controlPermission = actionDescriptor.ControllerName + "." + actionDescriptor.ActionName;

            List<KeyValuePair<string, StringValues>> authorization = context.HttpContext.Request.Headers.ToList().Where(t => t.Key == "Authorization").ToList();

            if (authorization == null || authorization.Count == 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string jwtToken = authorization.First().Value;

            var handler = new JwtSecurityTokenHandler();
            //var token = handler.ReadJwtToken(jwtToken.Replace("Bearer ", ""));
            // handler.ValidateToken(token,)
            //if (token.Claims.ToList().Where(t => t.Type == controlPermission).ToList() == null)
            //{

            //    context.Result = new ForbidResult();
            //    return;
            //}

            return;

        }
    }
}