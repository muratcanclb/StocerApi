using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class SupplierDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

    }
}
