using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Models
{
    public class AuthenticateResponse
    {
        public bool Status { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }

        public List<string> Permissions { get; set; }
    }
}
