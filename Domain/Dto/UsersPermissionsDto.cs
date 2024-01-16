using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intra.Api.Domain.Dto
{
    public class UsersPermissionsDto
    {

        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string LangCode { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }


        //Generic
        public bool IsStatus { get; set; }
        public bool IsArchive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
