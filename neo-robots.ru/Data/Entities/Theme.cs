using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class Theme : IHistory, IAuditableEntity
    {
        public Theme()
        {
            HashTagsThemes = new HashSet<HashTagsTheme>();
            NewsThemes = new HashSet<NewsTheme>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Sorting { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<HashTagsTheme> HashTagsThemes { get; set; }
        public virtual ICollection<NewsTheme> NewsThemes { get; set; }
    }
}
