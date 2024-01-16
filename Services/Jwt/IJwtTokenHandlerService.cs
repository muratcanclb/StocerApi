using Intra.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Intra.Api.Services.Jwt
{
    public interface IJwtTokenHandlerService
    {
        AuthenticateResponse CreateAccessToken(List<Claim> user, Guid uId);
        string CreateRefreshToken();
    }
}
