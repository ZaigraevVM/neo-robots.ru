using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class News
    {
        public News()
        {
            HashTagsNews = new HashSet<HashTagsNews>();
            NewsCities = new HashSet<NewsCities>();
            NewsRegions = new HashSet<NewsRegion>();
            NewsThemes = new HashSet<NewsTheme>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? AuthorId { get; set; }
        public int? PhotoId { get; set; }
        public DateTime? Date { get; set; }
        public string Text { get; set; }
        public int? NewspapersId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Newspaper Newspapers { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual ICollection<HashTagsNews> HashTagsNews { get; set; }
        public virtual ICollection<NewsCities> NewsCities { get; set; }
        public virtual ICollection<NewsRegion> NewsRegions { get; set; }
        public virtual ICollection<NewsTheme> NewsThemes { get; set; }

        public string GetPhotoFile()
        {
            return Photo != null ? Photo?.FileName : "nophoto.png";
        }
    }
}
