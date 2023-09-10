using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class Region
    {
        public Region()
        {
            NewsRegions = new HashSet<NewsRegion>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<NewsRegion> NewsRegions { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
