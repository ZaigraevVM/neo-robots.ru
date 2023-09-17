using SMI.Data.Entities.Enums;
using SMI.Data.Entities.Properties;
using System;

namespace SMI.Data.Entities
{
    public class AggregatorList : IHistory, IAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string LinkHtmlPath { get; set; }
        public string LoadedHtml { get; set; }
        public DateTime? LoadedDate { get; set; }
        public AggregatorListType Type { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int AggregatorSourceId { get; set; }
        public virtual AggregatorSource AggregatorSource { get; set; }
    }
}