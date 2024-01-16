using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intra.Api.Domain.Dto
{
    public class ActivitiesDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime StartedDate { get; set; }
        public DateTime EndedDate { get; set; }
        public bool Repeat { get; set; }
        public string Color { get; set; }
        public int RolesId { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
        public string Statement { get; set; }

        //Generic
        public bool IsStatus { get; set; }
    }
}
