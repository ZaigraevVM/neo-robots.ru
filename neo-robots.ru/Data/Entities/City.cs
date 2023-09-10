using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace SMI.Data.Entities
{
    [Table("Cities")]
    public partial class City
    {
        public City()
        {
            NewsCities = new HashSet<NewsCities>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<NewsCities> NewsCities { get; set; }
    }
}
