using SMI.Data.Entities.Properties;
using System;

namespace SMI.Data.Entities
{
    public class AggregatorNews : IHistory, IAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public DateTime? SourceDate { get; set; }
        public string ImageUrl { get; set; }
        public string Html { get; set; }
        public int AggregatorSourceId { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual AggregatorSource AggregatorSource { get; set; }
        public virtual News News { get; set; }
    }
}
