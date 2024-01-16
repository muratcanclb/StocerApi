using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class GroupsPostDto
    {
        public string Name { get; set; }

        //Generic
        public bool IsStatus { get; set; }
    }
}
