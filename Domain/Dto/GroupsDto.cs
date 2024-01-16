using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class GroupsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Generic
        public bool IsStatus { get; set; }
    }
}
