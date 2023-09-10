using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class NewsRegion
    {
        public int NewsId { get; set; }
        public int RegionId { get; set; }

        public virtual News News { get; set; }
        public virtual Region Region { get; set; }
    }
}
