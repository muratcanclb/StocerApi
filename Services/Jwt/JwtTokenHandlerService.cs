using Intra.Api.Data;
using Intra.Api.Domain.Entities;
using Intra.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Intra.Api.Services.Jwt
{
    public class JwtTokenHandlerService : IJwtTokenHandlerService
    {
        #region Fields

        public IConfiguration Configuration { get; set; }
        private readonly PostgreRepository<Users> _UsersRepository;

        #endregion

        #region Ctor

        public JwtTokenHandlerService(
            IOptions<AppSettings> appSettings,
            IConfiguration configuration,
            PostgreRepository<Users> usersRepository)
        {
            Configuration = configuration;
            // _appSettings = appSettings.Value;
            _UsersRepository = usersRepository;
        }

        //private readonly AppSettings _appSettings;


        #endregion


        #region Methods

        //Token üretecek metot.
        public AuthenticateResponse CreateAccessToken(List<Claim> claim, Guid uId)
        {

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["Token:SecurityKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            AuthenticateResponse tokenInstance = new AuthenticateResponse();
            tokenInstance.AccessToken = tokenHandler.WriteToken(token);
            tokenInstance.Expiration = DateTime.UtcNow.AddDays(7);

            //Write To DB Reflesh Token
            string refreshToken = CreateRefreshToken();
            var user = _UsersRepository.GetById(uId);
            user.RefreshToken = refreshToken;
            _UsersRepository.Update(user);


            tokenInstance.RefreshToken = refreshToken;


            return tokenInstance;
        }

        //private static IEnumerable<Claim> GetTokenClaims(IAppUser user)
        //{
        //    return new List<Claim> {
        //        new Claim("DisplayName", user.DisplayName),
        //        new Claim("UserName", user.Username)
        //        };
        //}

        //Refresh Token üretecek metot.
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }

        #endregion



    }
}
