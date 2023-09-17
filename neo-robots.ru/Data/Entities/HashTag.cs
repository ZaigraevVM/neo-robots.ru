using SMI.Data.Entities.Properties;
using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class HashTag : IHistory, IAuditableEntity
    {
        public HashTag()
        {
            HashTagsNews = new HashSet<HashTagsNews>();
            HashTagsThemes = new HashSet<HashTagsTheme>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string History { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<HashTagsNews> HashTagsNews { get; set; }
        public virtual ICollection<HashTagsTheme> HashTagsThemes { get; set; }
    }
}
