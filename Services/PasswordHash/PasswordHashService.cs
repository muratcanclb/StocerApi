using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Intra.Api.Services.PasswordHash
{
    public class PasswordHashService : IPasswordHashService
    {
        #region Fields


        #endregion

        #region Ctor

        public PasswordHashService()
        {
        }

        //private readonly AppSettings _appSettings;


        #endregion

        #region Fields


        public string Hash(string password)
        {
            // SHA256 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                // Print the string.   
            }
        }

        #endregion
    }
}
