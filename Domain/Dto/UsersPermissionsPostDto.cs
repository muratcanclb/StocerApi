using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class UsersPermissionsPostDto
    {

       
       
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public string LangCode { get; set; }

        //Generic
        public bool IsStatus { get; set; }

    }
}
