using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class SaleUpdateDto
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid ProductsId { get; set; }
        public Guid OrderNo { get; set; }
        public float TotalPrice { get; set; }
        //Generic
        public bool IsArchive { get; set; }
    }
}
