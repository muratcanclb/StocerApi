using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class StockListDto
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid ProductId { get; set; }
        public string SupplierName { get; set; }
        public string DepartmentName { get; set; }
        public string ProductName { get; set; }
        public string Productimg { get; set; }
        public bool Status { get; set; }
        public float Price { get; set; }
        public int Total { get; set; }
        public int Value { get; set; }


    }
}
