using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class SaleListDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid OrderNo { get; set; }
        public string DepartmentName { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Total { get; set; }

        //
        public DateTime CreatedDate { get; set; }

    }
}
