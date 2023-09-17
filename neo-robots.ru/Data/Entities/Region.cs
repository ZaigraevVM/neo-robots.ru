using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class Region : IHistory, IAuditableEntity
    {
        public Region()
        {
            NewsRegions = new HashSet<NewsRegion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<NewsRegion> NewsRegions { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
