using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class Photo
    {
        public Photo()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
