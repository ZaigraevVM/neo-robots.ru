using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class Photo : IHistory, IAuditableEntity
    {
        public Photo()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string SourceUrl { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
