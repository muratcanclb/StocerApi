using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class UsersRolesDto
    {
        public int Id { get; set; }
        public Guid? TenantsId { get; set; }
        public string TenantsName { get; set; }
        public string Name { get; set; }
        public string LangCode { get; set; }
        public bool IsAdmin { get; set; }

        //Generic
        public bool IsStatus { get; set; }
    }
}
