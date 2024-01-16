using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class StockDto
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid ProductId { get; set; }
        public int Total { get; set; }

    }
}
