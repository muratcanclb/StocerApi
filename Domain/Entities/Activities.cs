using System;
using Meb.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intra.Api.Domain.Entities
{
    public class Activities : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime EndedDate { get; set; }
        public bool Repeat { get; set; }
        public int RolesId { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
        public string Statement { get; set; }
      
        //Generic
        public bool IsStatus { get; set; }
        public bool IsArchive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }

    }
}
