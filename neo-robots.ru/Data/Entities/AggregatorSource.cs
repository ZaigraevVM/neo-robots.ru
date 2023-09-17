using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;

namespace SMI.Data.Entities
{
    public partial class AggregatorSource : IHistory, IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<AggregatorPage> AggregatorPages { get; set; }
        public virtual ICollection<AggregatorNews> AggregatorNewsList { get; set; }
        public virtual ICollection<AggregatorList> AggregatorLists { get; set; }
    }
}
