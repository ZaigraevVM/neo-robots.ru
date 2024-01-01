using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class News : IHistory, IAuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int? PhotoId { get; set; }
        public DateTime? Date { get; set; }
        public string Intro { get; set; }
        public string Text { get; set; }
        public int? NewspapersId { get; set; }
        public string Path { get; set; }
        public bool IsPublish { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int? AggregatorNewsId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Newspaper Newspapers { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual AggregatorNews AggregatorNews { get; set; }
        public virtual ICollection<HashTagsNews> HashTagsNews { get; set; } = new List<HashTagsNews>();
        public virtual ICollection<NewsCities> NewsCities { get; set; } = new List<NewsCities>();
        public virtual ICollection<NewsRegion> NewsRegions { get; set; } = new List<NewsRegion>();
        public virtual ICollection<NewsTheme> NewsThemes { get; set; } = new List<NewsTheme>();

        public string GetPhotoFile()
        {
            return Photo != null ? Photo?.FileName : "nophoto.png";
        }
    }
}
