using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class ProductsPostDto
    {
        public Guid GroupId { get; set; }
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Property { get; set; }
        public string Image { get; set; }
    }
}
