using System;
using SMI.Data.Entities.Properties;

namespace SMI.Data.Entities
{
    public class AggregatorDownload : IHistory, IAuditableEntity
    {
        public int Id { get; set; }
        public int? AggregatorListId { get; set; }
        public int? AggregatorPageId { get; set; }
        public string RequestUrl { get; set; }
        public string ResponseHtml { get; set; }
        public string ResponseStatus { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsError { get; set; }
        public string Error { get; set; }
        public virtual AggregatorList AggregatorList { get; set; }
        public virtual AggregatorPage AggregatorPage { get; set; }
    }
}