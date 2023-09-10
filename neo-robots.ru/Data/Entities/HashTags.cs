using System;
using System.Collections.Generic;

#nullable disable

namespace SMI.Data.Entities
{
    public partial class HashTag
    {
        public HashTag()
        {
            HashTagsNews = new HashSet<HashTagsNews>();
            HashTagsThemes = new HashSet<HashTagsTheme>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<HashTagsNews> HashTagsNews { get; set; }
        public virtual ICollection<HashTagsTheme> HashTagsThemes { get; set; }
    }
}
