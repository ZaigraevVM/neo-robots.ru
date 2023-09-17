using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SMI.Data.Entities
{
    [Table("Cities")]
    public partial class City : IHistory, IAuditableEntity
    {
        public City()
        {
            NewsCities = new HashSet<NewsCities>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<NewsCities> NewsCities { get; set; }
    }
}
