using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class SalePostDto
    {
        public Guid DepartmentId { get; set; }
        public Guid ProductsId { get; set; }
        public float TotalPrice { get; set; }
    }
}
