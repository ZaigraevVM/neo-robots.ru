using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class NewsCities
    {
        public int NewsId { get; set; }
        public int CityId { get; set; }

        public virtual City Cities { get; set; }
        public virtual News News { get; set; }
    }
}
