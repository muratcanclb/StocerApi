using System;
using Meb.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intra.Api.Domain.Entities
{
    public class Sale : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid OrderNo { get; set; }
        public Guid SupplierId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid ProductId { get; set; }
        public int Total { get; set; }
    
        //Generic
        public bool IsArchive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }

    }
}
