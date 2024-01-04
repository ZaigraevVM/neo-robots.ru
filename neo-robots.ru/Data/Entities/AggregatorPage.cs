using SMI.Data.Entities.Enums;
using SMI.Data.Entities.Properties;
using System;

namespace SMI.Data.Entities
{
    public class AggregatorPage : IHistory, IAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlRegex { get; set; }
        public string TitleHtmlPath { get; set; }
        public string TextHtmlPath { get; set; }
        public string ImageHtmlPath { get; set; }
        public string DateHtmlPath { get; set; }
        public string SouceUrlHtmlPath { get; set; }
        public string SouceTitleHtmlPath { get; set; }
        public AggregatorPageType Type { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int AggregatorSourceId { get; set; }
        public bool IsActive { get; set; }
        public virtual AggregatorSource AggregatorSource { get; set; }
    }
}