using Meb.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intra.Api.Domain.Entities
{
    public class Users : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid TenantsId { get; set; }
        public int RolesId { get; set; }
        public string Lang { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public string ConfirmToken { get; set; }

        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }

        public string ForgetToken { get; set; }
        public DateTime? ForgetTokenEndDate { get; set; }

        public string ProfileImagePath { get; set; }

        public string Email { get; set; }
        public string Ldap { get; set; }
        public bool IsLdapUser { get; set; }


        //Generic
        public bool IsStatus { get; set; }
        public bool IsArchive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }

    }
}
