using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class ProductSelectedDto
    {
        public Guid value { get; set; }
        public string text { get; set; }

        public Guid SupplierId { get; set; }
    }
}
