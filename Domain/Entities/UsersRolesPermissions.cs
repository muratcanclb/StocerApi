using Meb.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intra.Api.Domain.Entities
{
    public class UsersRolesPermissions : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int UsersRolesId { get; set; }
        public int UsersPermissionsId { get; set; }

        //Generic
        public bool IsStatus { get; set; }
        public bool IsArchive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
