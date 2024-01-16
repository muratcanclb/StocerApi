using System;
using Meb.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intra.Api.Domain.Entities
{
    public class Products : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid SupplierId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Property { get; set; }
        public string Image { get; set; }
      
        //Generic
        public bool IsArchive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }

    }
}
