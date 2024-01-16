using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Services.PasswordHash
{
    public interface IPasswordHashService
    {
        string Hash(string password);
    }
}
