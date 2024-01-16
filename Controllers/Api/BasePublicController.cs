using IdentityServer4.AccessTokenValidation;
using Meb.Api.Framework.Controllers;
using Meb.Api.Framework.Infrastructure.Wrapper.ExceptionHandling;
using Meb.Api.Framework.Infrastructure.Wrapper.Validation;
using Meb.Api.Framework.Mvc.Filters;
using Intra.Api.Data;
using Intra.Api.Domain.Entities;
using Intra.Api.Infrastructure.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Intra.Api.Controllers.Api
{
    //Model Validate exception durumunda hata standart response dönüştürülür.
    [ApiValidationFilter]
    //Herhangi bir exception (Validate hariç) durumda hata standart response dönüştürülür.
    [ApiExceptionFilter]
    //Servis loglarını tutar.
    //[StoreIpAddress]
    //Error loglarını tutar.
    //[SerilogFilter]
    [EnableCors("CorsPolicy")]
    public class BasePublicController : BaseController
    {

        #region Constants

        public Guid LOGIN_USER_ID;
        public Users LOGIN_USER;


        #endregion

        #region Fields

        #endregion

        #region Ctor

        public BasePublicController()
        {
        }

        #endregion

        public Guid GetUserId(IHttpContextAccessor _httpContext, PostgreRepository<Users> usersRepository)
        {
            Guid userId = Guid.NewGuid();

            var authorization = _httpContext.HttpContext.Request.Headers.Where(t => t.Key == "Authorization").ToList();

            if (authorization.Count <= 0)
                return Guid.Empty;

            string jwtToken = authorization.First().Value;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken.Replace("Bearer ", ""));
            var cleams = token.Claims.ToList().FirstOrDefault(t => t.Type == "UserId");
            if (cleams != null)
            {
                Guid.TryParse(cleams.Value, out userId);
            }

            LOGIN_USER_ID = userId;

            if (usersRepository != null && usersRepository.Table.Count() > 0)
            {
                LOGIN_USER = usersRepository.Table.FirstOrDefault(t => t.Id == userId);
            }

            return userId;
        }

    }


}